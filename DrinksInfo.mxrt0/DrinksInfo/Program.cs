using DrinksInfo.Controllers;

namespace DrinksInfo;
public class Program
{
    static async Task Main(string[] args)
    {
        var controller = new DrinksController();
        await controller.MainMenu();
    }
}
