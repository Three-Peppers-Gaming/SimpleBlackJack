using Microsoft.Extensions.Caching.Memory;
using SimpleBlackJack.Services;


namespace SimpleBlackJack
{
    internal class Program
    {


       





        static void Main(string[] args)
        {
            Console.Title = "BlackJack";
            Console.SetWindowSize(80, 30);
           



            // Create and instance of the Adventure Framework Service
            var _bjfw = new BlackJackService(new MemoryCache(new MemoryCacheOptions()));
            PlayBlackJackClient.Play(_bjfw);
            
        }
    }
}