using DrinksInfo.Models;
using DrinksInfo.Utils.Enums;
using Newtonsoft.Json;

namespace DrinksInfo.Utils;
public static class APIHelper
{
    private static readonly string baseApiFilterUrl = @"https://www.thecocktaildb.com/api/json/v1/1/filter.php?c={0}";
    private static readonly string baseApiIdLookupUrl = @"https://www.thecocktaildb.com/api/json/v1/1/lookup.php?i={0}";
    private static readonly string categoriesUrl = @"https://www.thecocktaildb.com/api/json/v1/1/list.php?c=list";
    private static readonly HttpClient client = new HttpClient();

    private static readonly Dictionary<DrinkCategoryOption, string> searchTermByCategory = new()
    {
        { DrinkCategoryOption.Ordinary, "Ordinary_Drink" },
        { DrinkCategoryOption.PunchParty, "Punch_Party_Drink" },
        { DrinkCategoryOption.Soft, "Soft_Drink" },
        { DrinkCategoryOption.CoffeeTea, "Coffee_/_Tea" },
        { DrinkCategoryOption.Other, "Other_/_Unknown" },
        { DrinkCategoryOption.HomemadeLiqueur, "Homemade_Liqueur" }
    };
    public static async Task<DrinkInfo>? FetchDrinkById(int drinkId)
    {
        string lookupUrl = string.Format(baseApiIdLookupUrl, drinkId);

        var response = await client.GetAsync(lookupUrl);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var drinkResponse = JsonConvert.DeserializeObject<DrinkResponse<DrinkInfo>>(jsonResponse);

        return drinkResponse.Drinks.FirstOrDefault();
    }
    public static async Task<List<DrinkInfo>> FetchDrinksByCategory(DrinkCategoryOption category)
    {
        List<DrinkInfo> drinks = new();

        string searchTerm = searchTermByCategory.TryGetValue(category, out var term) ? term : category.ToString();

        string apiUrl = string.Format(baseApiFilterUrl, searchTerm);

        var response = await client.GetAsync(apiUrl);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var drinksResponse = JsonConvert.DeserializeObject<DrinkResponse<DrinkInfo>>(jsonResponse);
        if (drinksResponse?.Drinks is not null)
        {
            drinks.AddRange(drinksResponse.Drinks);
        }
        return drinks;
    }

    public static async Task<List<DrinkCategory>> FetchDrinkCategories()
    {
        List<DrinkCategory> drinkCategories = new List<DrinkCategory>();

        var response = await client.GetAsync(categoriesUrl);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();

        var drinkResponse = JsonConvert.DeserializeObject<DrinkResponse<DrinkCategory>>(jsonResponse);

        if (drinkResponse?.Drinks is not null)
        {
            drinkCategories.AddRange(drinkResponse.Drinks);
        }
        return drinkCategories;
    }
}
