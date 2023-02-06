using Microsoft.Extensions.Caching.Memory;
using SimpleBlackJack.Services;


namespace SimpleBlackJack
{
    internal class Program
    {

        static void Main(string[] args)
        {
            Console.Title = "BlackJack";
            // Create and instance of the BlackJack
            var _bjfw = new BlackJackService(new MemoryCache(new MemoryCacheOptions()));
            PlayBlackJackClient.Play(_bjfw);
            
        }
    }
}