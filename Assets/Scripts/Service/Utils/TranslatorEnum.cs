using System.Collections.Generic;

public static class TranslatorEnum
{
    public static readonly Dictionary<FilterType, string> filtercontainer = new()
    {
        { FilterType.opening_hours, "Время работы" },
        { FilterType.contact, "Способ связи" },
        { FilterType.payment_method, "Способ оплаты" },
        { FilterType.cuisine, "Кухня" },
        { FilterType.other, "Дополнительно" }
    };
    public static readonly Dictionary<FilterOption, string> filter = new()
    {
        { FilterOption.opened_now, "Открыто сейчас" },
        { FilterOption.round_the_clock, "Круглосуточно" },
        { FilterOption.with_website, "С сайтом" },
        { FilterOption.with_phone, "С телефоном" },
        { FilterOption.payment_card, "Картой" },
        { FilterOption.payment_cash, "Наличными" },
        { FilterOption.asian_cuisine, "Азиатская" },
        { FilterOption.oriental_cuisine, "Восточноазиатская" },
        { FilterOption.chinese_cuisine, "Китайская" },
        { FilterOption.russian_cuisine, "Русская" },
        { FilterOption.caucasian_cuisine, "Кавказская" }
    };
    public static readonly Dictionary<Category, string> category = new()
    {
        { Category.food, "Поесть" },
        { Category.drink, "Попить" },
        { Category.entertainment, "Развлечься" },
        { Category.children, "С детьми" }
    };
    public static readonly Dictionary<SubCategory, string> subcategory = new()
    {
        { SubCategory.restaurant, "Ресторан" },
        { SubCategory.cafe, "Кафе" },
        { SubCategory.cinema, "Кинотеатр" },
        { SubCategory.bar, "Бар" },
        { SubCategory.fast_food, "Фастфуд" },
        { SubCategory.food_court, "Фудкорт" },
        { SubCategory.internet_cafe, "Интернет-кафе" },
        { SubCategory.night_club, "Ночной клуб" }
    };
}