using System;

public class Place
{
    public long id;
    public string name;
    public string address;
    public string opening_hours;
    public string website;
    public string phone;
    public string[] cuisine;
    public string payment_cash;
    public string payment_credit_cards;
    public GeoLocation location;
    public Category[] categories;
    public SubCategory subcategory;

    public float rating;
    public Place(OverpassElement element)
    {
        OverpassTags tags = element.tags;
        id = element.id;
        name = tags?.name;
        address = $"{tags?.addr_city}, {tags?.addr_street}, " +
            $"{tags?.addr_housenumber}, {tags?.addr_postcode}";
        Enum.TryParse(tags?.amenity, out subcategory);
        opening_hours = tags?.opening_hours;
        website = tags?.website;
        phone = tags?.phone;
        cuisine = tags?.cuisine?.Split(';');
        payment_cash = tags?.payment_cash;
        payment_credit_cards = tags?.payment_credit_cards;

        location = new GeoLocation(element.lat, element.lon);
    }
}
