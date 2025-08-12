using DrinksInfo.Models;
using DrinksInfo.Utils;
using DrinksInfo.Utils.Enums;
using Newtonsoft.Json;

namespace DrinksInfo.Controllers
{
    public class DrinksController
    {
        Dictionary<DrinkCategoryOption, List<DrinkInfo>> drinksByCategory;
        public DrinksController()
        {
            drinksByCategory = new();
        }

        public async Task MainMenu()
        {
            await UIHelper.DisplayCategories();

            Console.Write("Your input: ");
            string? userInput = Console.ReadLine();

            DrinkCategoryOption category;
            while (!Validator.TryValidateCategory(userInput!, out category))
            {
                Console.WriteLine(Messages.InvalidInputMessage);
                userInput = Console.ReadLine();
            }

            await HandleUserInput(category);
        }

        public async Task HandleUserInput(DrinkCategoryOption category)
        {
            Console.Clear();
            Console.WriteLine(Messages.CategoryInfoMessage);
            await Task.Delay(1000);

            await ShowDrinks(category);

            string userInput = await GetUserInput();

            var drinksInCategory = await APIHelper.FetchDrinksByCategory(category);

            var drinkToShow = drinksInCategory.FirstOrDefault(drink => string.Equals(userInput!.Replace(" ", "").ToLowerInvariant(),
                drink.StrDrink.Replace(" ", "").ToLowerInvariant()));

            while (drinkToShow is null)
            {
                Console.WriteLine(Messages.DrinkNotInCategoryMessage);
                userInput = await GetUserInput();
            }
            var drinkToShowInfo = await APIHelper.FetchDrinkById(int.Parse(drinkToShow.IdDrink));
            if (drinkToShowInfo is not null)
            {
                ShowDrinkInfo(drinkToShowInfo);
            }

            Console.WriteLine(Messages.PressAnyKeyToContinueMessage);
            Console.ReadKey();
            await MainMenu();
        }
        public void ShowDrinkInfo(DrinkInfo drink)
        {
            var drinkInfo = typeof(DrinkInfo).GetProperties().Where(prop => prop.GetValue(drink) is not null);

            Console.WriteLine();
            Console.WriteLine(string.Join($"{Environment.NewLine}- - - - - - - - - - -{Environment.NewLine}",
                drinkInfo.Select(prop => $"{prop.Name}: {prop.GetValue(drink)}")));
        }
        public async Task ShowDrinks(DrinkCategoryOption category)
        {
            if (!drinksByCategory.TryGetValue(category, out var currentCategoryDrinks))
            {
                try
                {
                    currentCategoryDrinks = await APIHelper.FetchDrinksByCategory(category);
                    drinksByCategory[category] = currentCategoryDrinks;
                }
                catch (HttpRequestException ex)
                {
                    Console.WriteLine($"Network or HTTP error: {ex.Message}");
                }
                catch (JsonException ex)
                {
                    Console.WriteLine($"Failed to parse response: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Unexpected error: {ex.Message}");
                }
            }

            UIHelper.PrintDrinksPaginated(currentCategoryDrinks!);
        }

        public async Task CheckReturnToMainMenu(string? input)
        {
            if (input == "0")
            {
                await MainMenu();
            }
        }

        public async Task<string> GetUserInput()
        {
            Console.WriteLine(Messages.ChooseDrinkMessage);
            Console.WriteLine(Messages.ReturnToMainMenuMessage);
            string? userInput = Console.ReadLine();
            while (string.IsNullOrEmpty(userInput))
            {
                Console.WriteLine(Messages.EmptyInputMessage);
            }
            await CheckReturnToMainMenu(userInput);
            return userInput;
        }
    }
}
