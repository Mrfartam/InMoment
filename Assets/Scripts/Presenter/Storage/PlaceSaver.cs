using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class PlaceSaver : MonoBehaviour
{
    private static string savePath => Path.Combine(Application.persistentDataPath, "Files", "places.json");
    private void Start()
    {
        if (!Directory.Exists(Path.GetDirectoryName(savePath)))
            Directory.CreateDirectory(Path.GetDirectoryName(savePath));
    }
    public static PlaceList LoadPlaces()
    {
        if (!File.Exists(savePath))
            return new PlaceList
            {
                fetches = new(),
                places = new(),
            };

        string json = File.ReadAllText(savePath);
        return JsonUtility.FromJson<PlaceList>(json);
    }
    public static void MergeJson(List<Place> places, GeoLocation loc)
    {
        PlaceList oldPlaces = LoadPlaces();
        oldPlaces.places ??= new();
        oldPlaces.fetches ??= new();

        List<double> distances = oldPlaces.fetches.Select(f => LocationTransform.DistanceBetween(loc,
            new GeoLocation(f.latitude, f.longitude))).ToList();
        bool isFetched = distances.Any(d => d < 2);

        if (!isFetched)
        {
            Fetch newFetch = new Fetch
            {
                dateTime = DateTime.UtcNow.ToString("yyyy-MM-ddTHH:mm:ssZ"),
                latitude = loc.latitude,
                longitude = loc.longitude,
            };
            oldPlaces.fetches.Add(newFetch);
        }
        
        foreach (var place in places)
        {
            bool exist = oldPlaces.places.Any(p => p.id == place.id);
            if (!exist)
                oldPlaces.places.Add(new PlaceData(place));
        }
        string json = JsonUtility.ToJson(oldPlaces, true);
        File.WriteAllText(savePath, json);
    }
}
