using DrinksInfo.Models;

namespace DrinksInfo.Utils
{
    public static class UIHelper
    {
        private static List<DrinkCategory> drinkCategories;
        public static async Task DisplayCategories()
        {
            Console.Clear();

            Console.WriteLine(Messages.MainMenuMessage);
            if (drinkCategories is null)
            {
                drinkCategories = await APIHelper.FetchDrinkCategories();
            }

            var sortedCategories = drinkCategories
                .OrderBy(c => c.StrCategory == "Other / Unknown" ? 1 : 0)
                .ThenBy(c => c.StrCategory)
                .ToList();

            Console.WriteLine(string.Join($"{Environment.NewLine}- - - - - - - - - - -{Environment.NewLine}", sortedCategories.Select(c => c.StrCategory)));
            Console.WriteLine();
        }

        public static void PrintDrinksPaginated(List<DrinkInfo> drinks, int pageSize = 10)
        {
            int total = drinks.Count;
            int pages = (total + pageSize - 1) / pageSize;

            for (int page = 0; page < pages; page++)
            {
                var pageDrinks = drinks.Skip(page * pageSize).Take(pageSize);
                Console.WriteLine(string.Join($"{Environment.NewLine}- - - - - - - - - - -{Environment.NewLine}",
                    pageDrinks.Select(d => d.StrDrink)));
                if (page < pages - 1)
                {
                    Console.WriteLine("\n(Press 'N' to see more or any other key to move on to drink input...)\n");
                    var keyInfo = Console.ReadKey(true);
                    if (keyInfo.Key == ConsoleKey.N)
                    {
                        continue;
                    }
                    break;
                }
            }
        }
    }
}
