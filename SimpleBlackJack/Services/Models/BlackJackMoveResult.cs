namespace SimpleBlackJack.Services.Models
{
    public class BlackJackMoveResult
    {
        public string Id { get; set; } = string.Empty;
        public int ComputerWins { get; set; } = 0;
        public int PlayerWins { get; set; } = 0;
        public int PlayerPoints { get; set; } = 0;
        public List<Card> ComputerCards { get; set; } = new();
        public int ComputerCardTotal { get; set; } = 0;
        public List<Card> PlayerCards { get; set; } = new();
        public int PlayerCardsTotal { get; set; } = 0;
        public int PlayerCardsBet { get; set; } = 0;
        public List<Card> PlayerSplitCards { get; set; } = new();
        public int PlayerSplitBet { get; set; } = 0;
        public int PlayerSplotTotal { get; set; } = 0;
        public bool PlayerCardsActive { get; set; } = false;
        public bool PlayerSplitActive { get; set; } = false;
        public bool PlayerHasInsurance { get; set; } = false;
        public string ComamndString { get; set; } = string.Empty;
        public string CommandStringWithBrackets { get; set; } = string.Empty;
        public List<string> CommandList { get; set; } = new();
        public List<string> Message { get; set; } = new();
    }
}
