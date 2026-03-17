using System;
using System.Collections.Generic;
using System.Linq;

[Serializable]
public class PlaceData
{
    public long id;
    public string name;
    public string address;
    public string opening_hours;
    public string website;
    public string phone;
    public List<string> cuisine;
    public string payment_cash;
    public string payment_credit_cards;
    public float location_latitude;
    public float location_longitude;
    public List<string> categories;
    public string subcategory;
    public float rating;
    public List<long> reviews;
    public List<string> photosPaths;
    public PlaceData(Place place)
    {
        id = place.id;
        name = place.name;
        address = place?.address;
        opening_hours = place?.opening_hours;
        website = place?.website;
        phone = place?.phone;
        cuisine = place?.cuisine?.ToList();
        payment_cash = place?.payment_cash;
        payment_credit_cards = place?.payment_credit_cards;
        location_latitude = place.location.latitude;
        location_longitude = place.location.longitude;
        categories = place?.categories?.Select(c => c.ToString()).ToList();
        subcategory = place?.subcategory.ToString();
        rating = place.rating;
    }
}
[Serializable]
public class Fetch
{
    public float latitude;
    public float longitude;
    public string dateTime;
}
[Serializable]
public class PlaceList
{
    public List<Fetch> fetches;
    public List<PlaceData> places;
}