using SimpleBlackJack.Services.Models;

namespace SimpleBlackJack.Services
{
    public interface IBlackJack
    {
        public BlackJackMoveResult PlayBlackJack(GameMove move);
        public List<string> HouseRules();
        public List<string> CommandList();

    }
}
