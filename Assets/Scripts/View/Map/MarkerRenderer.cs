using System.Collections.Generic;
using UnityEngine;

public class MarkerRenderer : MonoBehaviour
{
    public GameObject markerPrefab;
    public MapView mapView;
    public void CreateMarkers(List<Vector2> placesLocations)
    {
        foreach (Transform child in transform)
            Destroy(child.gameObject);

        foreach(Vector2 loc in placesLocations)
            Instantiate(markerPrefab, loc,
                Quaternion.Euler(0, 0, -135), transform);
    }
}
