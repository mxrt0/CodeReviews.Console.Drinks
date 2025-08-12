using DrinksInfo.Models;
using DrinksInfo.Utils.Enums;

namespace DrinksInfo.Utils
{
    public static class Validator
    {
        private static List<DrinkCategory>? categories;
        private static Dictionary<string, DrinkCategoryOption> normalizedCategoryMap = new Dictionary<string, DrinkCategoryOption>(StringComparer.OrdinalIgnoreCase)
        {
            { "unknown", DrinkCategoryOption.Other },
            { "other/unknown", DrinkCategoryOption.Other },
            { "ordinarydrink", DrinkCategoryOption.Ordinary },
            { "softdrink", DrinkCategoryOption.Soft },
            { "punch", DrinkCategoryOption.PunchParty },
            { "party", DrinkCategoryOption.PunchParty },
            { "punch/party", DrinkCategoryOption.PunchParty },
            { "punch/partydrink", DrinkCategoryOption.PunchParty },
            {"homemade",DrinkCategoryOption.HomemadeLiqueur },
            {"liqueur",DrinkCategoryOption.HomemadeLiqueur },
            { "coffee", DrinkCategoryOption.CoffeeTea },
            { "tea", DrinkCategoryOption.CoffeeTea },
            { "coffee/tea", DrinkCategoryOption.CoffeeTea }
            // add more mappings, normalized (no spaces, lowercase)
        };

        private static string Normalize(string s) => s.Replace(" ", "").ToLowerInvariant();

        private static bool TryGetMappedCategoryEnum(string userInput, out DrinkCategoryOption category)
        {
            return normalizedCategoryMap.TryGetValue(Normalize(userInput), out category);
        }

        public static bool TryValidateCategory(string? userInput, out DrinkCategoryOption category)
        {
            if (string.IsNullOrEmpty(userInput))
            {
                category = default;
                return false;
            }

            if (TryGetMappedCategoryEnum(userInput, out var mappedCategory))
            {
                category = mappedCategory;
                return true;
            }

            if (Enum.TryParse<DrinkCategoryOption>(userInput, true, out var parsedCategory))
            {
                category = parsedCategory;
                return true;
            }

            category = default;
            return false;
        }

    }
}
