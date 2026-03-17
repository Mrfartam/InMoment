using System.Collections.Generic;

public enum Category
{
    food,
    drink,
    entertainment,
    children
}
public enum SubCategory
{
    restaurant,
    cafe,
    bar,
    fast_food,
    food_court,
    cinema,
    night_club,
    internet_cafe
}
public static class CategorySubcategory
{
    public static readonly Dictionary<Category, List<SubCategory>> subcategories = new()
    {
        { Category.food, new() { SubCategory.restaurant, SubCategory.fast_food, SubCategory.food_court, SubCategory.cafe } },
        { Category.drink, new() { SubCategory.restaurant, SubCategory.bar, SubCategory.food_court  } },
        { Category.entertainment, new() { SubCategory.cinema, SubCategory.night_club, SubCategory.internet_cafe } },
        { Category.children, new() { SubCategory.cinema, SubCategory.restaurant, SubCategory.cafe } }
    };
}