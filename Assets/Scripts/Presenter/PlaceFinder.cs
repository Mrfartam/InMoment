using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Rendering;

public class PlaceFinder : MonoBehaviour
{
    public List<Place> places;
    public MenuAgregator agregator;

    public bool isFinding;
    private bool firstSearch;
    private void Start()
    {
        places = new();
        firstSearch = true;
    }
    public IEnumerator FindPlacesInRadius(GeoLocation loc, float radius)
    {
        isFinding = true;
        try
        {
            if (firstSearch)
            {
                PlaceList placeList = PlaceSaver.LoadPlaces();
                foreach (var fetch in placeList.fetches)
                    if (LocationTransform.DistanceBetween(loc, new GeoLocation(fetch.latitude, fetch.longitude)) < 1.0)
                    {
                        foreach (var place in placeList.places)
                            places.Add(new Place(place));
                        break;
                    }

                if (places.Count != 0)
                {
                    agregator.FillMainMenu(places);
                    firstSearch = false;
                    yield break;
                }
            }
            
            string query = $@"[out:json][timeout:60];
(
  node[""amenity""~""restaurant|cafe|bar|fast_food|food_court|cinema|night_club|internet_cafe""]
(around:{radius * 1000},{loc.latitude.ToString(CultureInfo.InvariantCulture)},{loc.longitude.ToString(CultureInfo.InvariantCulture)});
);
out body;
>;
out skel qt;";

            places.Clear();

            string url = "https://overpass-api.de/api/interpreter?data=" + query;

            for (int attempt = 1; attempt <= 5; attempt++)
            {
                using (UnityWebRequest uwr = UnityWebRequest.Get(url))
                {
                    yield return uwr.SendWebRequest();

                    if (uwr.result == UnityWebRequest.Result.Success)
                    {
                        string jsonResult = uwr.downloadHandler.text;
                        ParseResponse(jsonResult);
                        
                        PlaceSaver.MergeJson(places, loc);
                        PlaceList placeList = PlaceSaver.LoadPlaces();
                        
                        places.Clear();
                        foreach (var place in placeList.places)
                            places.Add(new Place(place));
                        agregator.FillMainMenu(places);
                        
                        yield break;
                    }

                    switch (uwr.responseCode)
                    {
                        case 429:
                            Debug.LogWarning("Слишком много запросов к API. Ждём и пробуем снова. " +
                            "Повторная попытка обратиться: " + attempt + "/5");
                            yield return new WaitForSeconds(10f);
                            continue;
                        case 503:
                            Debug.LogWarning("Сервер недоступен. " +
                            "Повторная попытка обратиться: " + attempt + "/5");
                            yield return new WaitForSeconds(2f);
                            continue;
                        case 504:
                            Debug.LogWarning("Превышено время ожидания ответа от сервера. " +
                            "Повторная попытка обратиться: " + attempt + "/5");
                            yield return new WaitForSeconds(2f);
                            continue;
                        default:
                            Debug.LogWarning($"Ошибка запроса к Overpass: {uwr.error}\n" +
                                $"Код ответа: {uwr.responseCode}\n" +
                                $"Тело ответа:  + {uwr.downloadHandler.text}");
                            yield break;
                    }
                }
            }
            if (places.Count == 0)
                Debug.LogWarning("Отсутствуют сохранения и не удалось" +
                    "ничего найти за отведённое количество попыток");
        }
        finally
        {
            isFinding = false;
        }
    }
    private void ParseResponse(string json)
    {
        places.Clear();
        json = json.Replace("addr:city", "addr_city").
            Replace("addr:postcode", "addr_post_code").
            Replace("addr:housenumber", "addr_housenumber").
            Replace("addr:street", "addr_street").
            Replace("payment:cash", "payment_cash").
            Replace("payment:credit_cards", "payment_credit_cards").
            Replace("contact:phone", "phone");

        try
        {
            OverpassResponse response = JsonUtility.FromJson<OverpassResponse>(json);

            if (response?.elements == null || response.elements.Length == 0)
            {
                return;
            }

            foreach (var element in response.elements)
            {

                if (element?.type != "node")
                    continue;

                Place place = new Place(element);
                places.Add(place);
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Ошибка парсинга JSON: {e.Message}");
        }
    }
}