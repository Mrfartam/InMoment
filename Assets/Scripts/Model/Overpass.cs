using System;

[Serializable]
public class OverpassResponse
{
    public double version;
    public string generator;
    public OverpassElement[] elements;
}

[Serializable]
public class OverpassElement
{
    public string type;
    public long id;
    public float lat;
    public float lon;
    public OverpassTags tags;
}

[Serializable]
public class OverpassTags
{
    public string name;
    public string amenity;
    public string addr_city;
    public string addr_postcode;
    public string addr_housenumber;
    public string addr_street;
    public string cuisine;
    public string opening_hours;
    public string website;
    public string phone;
    public string payment_cash;
    public string payment_credit_cards;
}