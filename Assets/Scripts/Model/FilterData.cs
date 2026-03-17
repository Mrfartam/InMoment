using System.Collections.Generic;

public enum FilterType
{
    opening_hours,
    contact,
    payment_method,
    cuisine,
    other
}
public enum FilterOption
{
    opened_now,
    round_the_clock,
    with_website,
    with_phone,
    payment_card,
    payment_cash,
    asian_cuisine,
    oriental_cuisine,
    caucasian_cuisine,
    russian_cuisine,
    chinese_cuisine,
    italian_cuisine
    // Ещё другие виды кухонь
}
public static class FilterTypeToOption
{
    public static Dictionary<FilterType, List<FilterOption>> t2o = new()
    {
        { FilterType.opening_hours, new() { FilterOption.opened_now, FilterOption.round_the_clock } },
        { FilterType.contact, new() { FilterOption.with_phone, FilterOption.with_website } },
        { FilterType.payment_method, new() { FilterOption.payment_card, FilterOption.payment_cash } },
        { FilterType.cuisine, new() { FilterOption.asian_cuisine, FilterOption.oriental_cuisine,
            FilterOption.caucasian_cuisine, FilterOption.chinese_cuisine, FilterOption.russian_cuisine,
            FilterOption.italian_cuisine } }
    };
}