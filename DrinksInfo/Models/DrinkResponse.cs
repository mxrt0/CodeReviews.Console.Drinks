namespace DrinksInfo.Models
{
    public class DrinkResponse<T> where T : class
    {
        public T[] Drinks { get; set; }
    }
}
