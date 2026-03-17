using UnityEngine;
using System;

public class LocationTransform
{
    public static Vector2 LatLonToXY(GeoLocation loc, int zoomLevel)
    {
        float x = (float)((loc.longitude + 180.0) / 360.0 * (1 << zoomLevel));

        double latRad = loc.latitude * Mathf.Deg2Rad;
        float y = (float)((1.0 - Mathf.Log(Mathf.Tan((float)latRad) + 1f / Mathf.Cos((float)latRad)) / Mathf.PI)
            / 2.0 * (1 << zoomLevel));

        return new Vector2(x, y);
    }
    public static Vector2 LatLonToUnityPos(GeoLocation loc, int zoomLevel, Vector2 mapCenter)
    {
        Vector2 xy = LatLonToXY(loc, zoomLevel);

        return new Vector2(2.56f * (xy.x - (int)mapCenter.x - 0.5f),
            -2.56f * (xy.y - (int)mapCenter.y - 0.5f));
    }
    public static Vector2Int LatLonToTile(GeoLocation loc, int zoomLevel)
    {
        Vector2 xy = LatLonToXY(loc, zoomLevel);

        return new Vector2Int((int)Mathf.Floor(xy.x), (int)Mathf.Floor(xy.y));
    }
    public static GeoLocation XYToLatLon(Vector2 pos, int zoomLevel) {
        float lon = (float)(pos.x * (360.0 / (1 << zoomLevel)) - 180);
        float lat = (float)(Math.Atan(Math.Sinh(Math.PI - (Math.PI * pos.y) / (1 << (zoomLevel - 1)))) * 180.0 / Math.PI);
        return new GeoLocation(lat, lon);
    }
    public static double DistanceBetween(GeoLocation first, GeoLocation second)
    {
        double EarthRadius = 6371.0;
        double dLat = Math.PI * (second.latitude - first.latitude) / 180.0;
        double dLon = Math.PI * (second.longitude - first.longitude) / 180.0;

        double rLat1 = Math.PI * first.latitude / 180.0;
        double rLat2 = Math.PI * second.latitude / 180.0;

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) + 
            Math.Cos(rLat1) * Math.Cos(rLat2) * Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

        return EarthRadius * c;
    }
}
