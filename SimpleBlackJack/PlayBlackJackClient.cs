using SimpleBlackJack.Services;
using SimpleBlackJack.Services.Models;
using System.Net.NetworkInformation;
using System.Net.WebSockets;
using System.Text;

namespace SimpleBlackJack
{
    internal class PlayBlackJackClient
    {

        private static IAppVersionService appVersionService = new AppVersionService();
        private static BlackJackMoveResult bmr = new();
        const string DevBy = "Developed by";
        readonly static string SteveSparks = "Steve Sparks";
        readonly static string RepoName = "GitHub";
        readonly static string RepoURL = "https://github.com/Three-Peppers-Gaming";
        const string UNDERLINE = "\x1B[4m";
        const string RESET = "\x1B[0m";
        private static Boolean Monotone = false;
        private static ConsoleColor Monocolor = ConsoleColor.White;

        private static void SetColor(ConsoleColor consoleWordColor)
        {
            Console.BackgroundColor = ConsoleColor.Black;
            Console.ForegroundColor = consoleWordColor;
        }

        private static void Write(ConsoleColor c, string text = "")
        {
            if (!Monotone) SetColor(c);
            else SetColor(Monocolor);
            Console.Write(text);
        }

        private static void Writeln(ConsoleColor c, string text = "")
        {
            Write(c, text);
            Console.WriteLine();
        }

        public static void HowToPlay()
        {

            IAppVersionService app = new AppVersionService();
            var input = "";
            while (input != "enter")
            {
                Console.Clear();
                Writeln(ConsoleColor.Red, "        T h r e e  P e p p e r s  G a m i n g   P r e s e n t s ");
                Writeln(ConsoleColor.Green, "************************* S I M P L E  T E X T *************************");
                Writeln(ConsoleColor.DarkMagenta, "♥♥♥♥♥♥  ♥♥       ♥♥♥♥♥   ♥♥♥♥♥♥ ♥♥   ♥♥      ♥♥  ♥♥♥♥♥   ♥♥♥♥♥♥ ♥♥   ♥♥");
                Writeln(ConsoleColor.DarkMagenta, "♥♥   ♥♥ ♥♥      ♥♥   ♥♥ ♥♥      ♥♥  ♥♥       ♥♥ ♥♥   ♥♥ ♥♥      ♥♥  ♥♥");
                Writeln(ConsoleColor.Magenta, "♥♥♥♥♥♥  ♥♥      ♥♥♥♥♥♥♥ ♥♥      ♥♥♥♥♥        ♥♥ ♥♥♥♥♥♥♥ ♥♥      ♥♥♥♥♥");
                Writeln(ConsoleColor.Magenta, "♥♥   ♥♥ ♥♥      ♥♥   ♥♥ ♥♥      ♥♥  ♥♥  ♥♥   ♥♥ ♥♥   ♥♥ ♥♥      ♥♥  ♥♥");
                Writeln(ConsoleColor.Magenta, "♥♥♥♥♥♥  ♥♥♥♥♥♥♥ ♥♥   ♥♥  ♥♥♥♥♥♥ ♥♥   ♥♥  ♥♥♥♥♥  ♥♥   ♥♥  ♥♥♥♥♥♥ ♥♥   ♥♥");
                Write(ConsoleColor.White, $"  ********** Developed by {SteveSparks}, ");
                Write(ConsoleColor.DarkYellow, $"ver:{app.Version}");
                Writeln(ConsoleColor.White, $" **********");
                Writeln(ConsoleColor.White);
                Writeln(ConsoleColor.Green,  "   Your goal is to beat the dealers hand without going over 21.");
                Writeln(ConsoleColor.Yellow, "    * You will receive 2 cards. Cards have values from 2 through 11.");
                Writeln(ConsoleColor.Yellow, "    * Face Values 2 through 10, K,Q,J are 10 and Ace is 11 or 1 depending on your hand.");
                Writeln(ConsoleColor.Green, "");
                Writeln(ConsoleColor.Green, "   The Dealers cards will show one card facedown and one card face up. ");
                Writeln(ConsoleColor.Yellow, "    * [H]IT if you think you need a higher total.");
                Writeln(ConsoleColor.Yellow, "    * You \"bust\" if you hand is over 21.");
                Writeln(ConsoleColor.Yellow, "    * [S]TAND when you are done taking cards.");
                Writeln(ConsoleColor.Green, "");
                Write(ConsoleColor.Green,    "   Special Betting Options.");
                Writeln(ConsoleColor.Red,    " Be sure to check the [C]asino and [G]ame commands.");
                Writeln(ConsoleColor.Yellow, "    * You will have the option to buy [I]NSurance when the dealer has an ACE showing.");
                Writeln(ConsoleColor.Yellow, "        If the dealer has blackjack you get the insurance and your bet returned.");
                Writeln(ConsoleColor.Yellow, "        Otherwise you lose the insurance bet and play continues.");
                Writeln(ConsoleColor.Yellow, "    * On your first move you can [D]ouble your bet rather than hit or stand.");
                Writeln(ConsoleColor.Yellow, "        You get 1 more card and then the dealers completes the round of play.");
                Writeln(ConsoleColor.Green, "");
                Writeln(ConsoleColor.Green, "   Press [Enter] to return to play.");

                while (!Console.KeyAvailable) { Thread.Sleep(1); }
                input = Console.ReadKey().Key.ToString().ToLower();
            }

        }
        public static void Logo()
        {

            IAppVersionService app = new AppVersionService();
            var input = "";
            while (input != "enter")
            {
                Console.Clear();  
                Writeln(ConsoleColor.Red,         "        T h r e e  P e p p e r s  G a m i n g   P r e s e n t s ");
                Writeln(ConsoleColor.Green,       "************************* S I M P L E  T E X T *************************");
                Writeln(ConsoleColor.DarkMagenta, "♥♥♥♥♥♥  ♥♥       ♥♥♥♥♥   ♥♥♥♥♥♥ ♥♥   ♥♥      ♥♥  ♥♥♥♥♥   ♥♥♥♥♥♥ ♥♥   ♥♥");
                Writeln(ConsoleColor.DarkMagenta, "♥♥   ♥♥ ♥♥      ♥♥   ♥♥ ♥♥      ♥♥  ♥♥       ♥♥ ♥♥   ♥♥ ♥♥      ♥♥  ♥♥");
                Writeln(    ConsoleColor.Magenta, "♥♥♥♥♥♥  ♥♥      ♥♥♥♥♥♥♥ ♥♥      ♥♥♥♥♥        ♥♥ ♥♥♥♥♥♥♥ ♥♥      ♥♥♥♥♥");
                Writeln(ConsoleColor.Magenta,     "♥♥   ♥♥ ♥♥      ♥♥   ♥♥ ♥♥      ♥♥  ♥♥  ♥♥   ♥♥ ♥♥   ♥♥ ♥♥      ♥♥  ♥♥");
                Writeln(ConsoleColor.Magenta,     "♥♥♥♥♥♥  ♥♥♥♥♥♥♥ ♥♥   ♥♥  ♥♥♥♥♥♥ ♥♥   ♥♥  ♥♥♥♥♥  ♥♥   ♥♥  ♥♥♥♥♥♥ ♥♥   ♥♥");
                Write(ConsoleColor.White, $"  ********** Developed by Steve Sparks, ");
                Write(ConsoleColor.DarkYellow, $"ver:{app.Version}");
                Writeln(ConsoleColor.White, $" **********");
                Writeln(ConsoleColor.Green, "");
                Writeln(ConsoleColor.Yellow,      "  Keys to adjust the console experience and get help.");
                Writeln(ConsoleColor.DarkCyan,    "    [G]ame Help    : Details game commands and additional betting help.");
                Writeln(ConsoleColor.DarkCyan,    "    [M]ono/Color   : Toggle colors between all white and default colors.");
                Writeln(ConsoleColor.DarkCyan,    "    [P]laying Help : Toggle colors between all white and default colors.");
                Writeln(ConsoleColor.DarkCyan,    "    [Q]uit         : Quit and close the game.");
                Writeln(ConsoleColor.DarkCyan,    "    [T]ips         : Dispaly this message.");
                Writeln(ConsoleColor.DarkCyan,    "    [C]asino       : Provides \"House\" rules and BlackJack scoring details.");
                Writeln(ConsoleColor.Green, "");
                Writeln(ConsoleColor.Yellow, $"                 Find out more about this game on {RepoName}");
                Writeln(ConsoleColor.Yellow, $"                 {RepoURL}");
                Writeln(ConsoleColor.Green, "");
                Writeln(ConsoleColor.Green,       "  Press [Enter] to Play");
                while (!Console.KeyAvailable) { Thread.Sleep(1); }
                input = Console.ReadKey().Key.ToString().ToLower();
            }
                Console.Clear();
        }

        private static void DrawCardPart(List<Card> PlayerCards, string part)
        {
            ConsoleColor c = ConsoleColor.White;
            var count = PlayerCards.Count;
            string suit = "";
            string cardpart = "";

            for (int x = 0; x < count; x++)
            {
                cardpart = part;

                if (PlayerCards[x].Name.Contains("Hearts")) { c = ConsoleColor.Red; suit = $"{(char)3}"; }
                if (PlayerCards[x].Name.Contains("Diamonds")) { c = ConsoleColor.Red; suit = $"{(char)4}"; }
                if (PlayerCards[x].Name.Contains("Spades")) { c = ConsoleColor.White; suit = $"{(char)5}"; }
                if (PlayerCards[x].Name.Contains("Clubs")) { c = ConsoleColor.White; suit = $"{(char)4}"; }
                if (PlayerCards[x].Name.Contains("Face")) { c = ConsoleColor.Green; suit = $"?"; }


                var cn = PlayerCards[x].Name.Substring(0,1);

                if (PlayerCards[x].Name.Contains("Face"))
                {
                    cardpart = cardpart.Replace("Z", "?"); 
                    cardpart = cardpart.Replace("L", "?"); 
                    cardpart = cardpart.Replace("X", "?"); 
                    cardpart = cardpart.Replace("Y", "?");
                }

                cardpart = cardpart.Replace("@", suit);
                cardpart = cardpart.Replace("Y", cn);
                if (cn == "1") { cardpart = cardpart.Replace("Z", "1"); cardpart = cardpart.Replace("L", "0"); } else { cardpart = cardpart.Replace("Z", " "); cardpart = cardpart.Replace("L", cn); }
                if (cn == "1") { cardpart = cardpart.Replace("X", "0"); } else { cardpart = cardpart.Replace("X", " "); }
   
                Write(c, $"  {cardpart}");
           
            }
            Console.WriteLine();
        }

        private static void DrawHand(List<Card> PlayerCards)
        {
            var cardcount = PlayerCards.Count;
            string CardBase = "+---------+";
            string CardFill = "|         |";
            string CardTopN = "|YX       |";
            string CardBotN = "|       ZL|";
            string CardSuit = "|    @    |";


            DrawCardPart(PlayerCards, CardBase);
            DrawCardPart(PlayerCards, CardTopN);
            // DrawCardPart(PlayerCards, CardFill);
            DrawCardPart(PlayerCards, CardSuit);
            // DrawCardPart(PlayerCards, CardFill);
            DrawCardPart(PlayerCards, CardBotN);
            DrawCardPart(PlayerCards, CardBase); 
            
        }

        private static void GameScreenHeader(BlackJackMoveResult game)
        {
            Console.Clear();
            Console.BackgroundColor = ConsoleColor.Black; Console.ForegroundColor = ConsoleColor.DarkMagenta;
            Writeln(ConsoleColor.White, $"                         B L A C K J A C K  ");
            Writeln(ConsoleColor.DarkCyan, "***********************************************************************");
            Write(ConsoleColor.Green,   $"          House Wins : "); Write(ConsoleColor.Yellow,$"{game.ComputerWins}");
            Write(ConsoleColor.Green,   $"            Player Points : "); Writeln(ConsoleColor.Yellow, $"{game.PlayerPoints}");
            Write(ConsoleColor.Green,   $"         Player Wins : "); Write(ConsoleColor.Yellow, $"{game.PlayerWins}");
            Write(ConsoleColor.Green,   $"               Player Bet : "); Writeln(ConsoleColor.Yellow, $"{game.PlayerCardsBet}");
            Writeln(ConsoleColor.DarkCyan, "***********************************************************************");

        }

        private static void GameScreen(BlackJackMoveResult game)
        {
            GameScreenHeader(game);

            if (game.PlayerCards.Count > 0)
            {
                Write(ConsoleColor.White, " House Cards, ");
                Writeln(ConsoleColor.Yellow, $"Total = {game.ComputerCardTotal}");
                DrawHand(game.ComputerCards);
                Write(ConsoleColor.White, " Player Cards, ");
                Writeln(ConsoleColor.Yellow, $"Total = {game.PlayerCardsTotal}");
                DrawHand(game.PlayerCards);
            }

            Writeln(ConsoleColor.Green, "");
            foreach(string i in game.Message)
            {
                Writeln(ConsoleColor.Yellow,$"  * {i}");
            }
            
            Writeln(ConsoleColor.White, "");
            Writeln(ConsoleColor.Cyan, "Press [bracketed] letter for choice.");
            Write(ConsoleColor.Green,  "  Console : ");
            Writeln(ConsoleColor.DarkCyan,"[M]ONO, [T]IPS, [P]LAY HELP and [Q]UIT.");
            Write(ConsoleColor.Green,  "  Game    : ");
            Writeln(ConsoleColor.DarkYellow, game.CommandStringWithBrackets);

        }


        public static void Play(BlackJackService _client)
        {
            var instance = Guid.NewGuid().ToString();   
            var gameover = false;
            var input = "";
            var betvalue = "100";

            BlackJackMoveResult game;
            GameMove move = new GameMove();

            move.Move = "";
            move.InstanceID = instance;
            game = _client.PlayBlackJack(move);

            Logo();

            while (!gameover) 
            {
                move.InstanceID = instance;
                GameScreen(game);

                while (!Console.KeyAvailable) { Thread.Sleep(1); }
                input = Console.ReadKey().Key.ToString().ToLower();


                switch (input.ToLower())
                {

                    case "m":
                        move.Move = "";
                        if (Monotone == true) { Monotone = false; }
                        else { Monotone = true; }
                        break;
                    case "t":
                        move.Move = "";
                        Logo();
                        break;
                    case "p":
                        move.Move = "";
                        HowToPlay();
                        break;
                    case "q":
                        move.Move = "";
                        gameover = true;
                        break;
                    case "b":
                        move.Move = "BET";
                        betvalue = "100";
                        break;
                    case "d1":
                        move.Move = "BET";
                        betvalue = "1000";
                        break;
                    case "d2":
                        move.Move = "BET";
                        betvalue = "2000";
                        break;
                    case "d3":
                        move.Move = "BET";
                        betvalue = "3000";
                        break;
                    case "d4":
                        move.Move = "BET";
                        betvalue = "4000";
                        break;
                    case "d5":
                        move.Move = "BET";
                        betvalue = "5000";
                        break;
                    case "d6":
                        move.Move = "BET";
                        betvalue = "6000";
                        break;
                    case "d7":
                        move.Move = "BET";
                        betvalue = "7000";
                        break;
                    case "d8":
                        move.Move = "BET";
                        betvalue = "8000";
                        break;
                    case "d9":
                        move.Move = "BET";
                        betvalue = "9000";
                        break;
                    case "d0":
                        move.Move = "BET";
                        betvalue = "10000";
                        break;
                    case "s":
                        move.Move = "STAND";
                        break;
                    case "n":
                        move.Move = "NEW";
                        break;
                    case "h":
                        move.Move = "HIT";
                        break;
                    case "i":
                        move.Move = "INS";
                        break;
                    case "d":
                        move.Move = "DOUBLE";
                        break;
                    case "c":
                        move.Move = "CASINO";
                        input = "";
                        while (input != "enter")
                        {
                            game = _client.PlayBlackJack(move);
                            GameScreenHeader(game);
                            Writeln(ConsoleColor.Green, "");
                            int f1 = 0;
                            foreach (string i in game.Message)
                            {
                                if (f1 ==0) Writeln(ConsoleColor.Yellow, $"    {i}");
                                else if (f1 == 1) Writeln(ConsoleColor.DarkCyan, $"       {i}"); 
                                else if (f1 == 2) Writeln(ConsoleColor.DarkCyan, $"       {i}"); 
                                else Writeln(ConsoleColor.White, $"         * {i}");

                                f1++;
                            }
                            Writeln(ConsoleColor.White, "");
                            Writeln(ConsoleColor.Yellow, "       See the [G]ame help for additional details");
                            Writeln(ConsoleColor.White, "");
                            Writeln(ConsoleColor.Green, "    Press [Enter] to return to the game");
                            game.Message = new(); // clear the messages we want the screen to clear

                       
                                while (!Console.KeyAvailable) { Thread.Sleep(1); }
                                input = Console.ReadKey().Key.ToString().ToLower();
                        }

                        GameScreen(game);
                        move.Move = " ";
                        break;
                    case "g":
                        move.Move = "[g]ame";
                        input = "";
                        while (input != "enter")
                        {
                            game = _client.PlayBlackJack(move);
                            GameScreenHeader(game);
                            Writeln(ConsoleColor.Green, "");
                            int f2 = 0;
                            foreach (string i in game.Message)
                            {
                                if (f2 == 0) Writeln(ConsoleColor.Yellow, $"    {i}");
                                else if (f2 == 1) Writeln(ConsoleColor.DarkCyan, $"      {i}");
                                else Writeln(ConsoleColor.White, $"        * {i}");
                                f2++;

                            }
                            Writeln(ConsoleColor.White, "");
                            Writeln(ConsoleColor.White, "        Possible Keyboard bets are ");
                            Writeln(ConsoleColor.Yellow, "           [B] = 100, [1] = 1000 through 0 = 10000");
                            Writeln(ConsoleColor.White, "");
                            Writeln(ConsoleColor.Green, "Press [Enter] to return to the game");
                            game.Message = new();

                                while (!Console.KeyAvailable) { Thread.Sleep(1); }
                                input = Console.ReadKey().Key.ToString().ToLower();
                        }

                        GameScreen(game);

                        move.Move = " ";
                        break;

                    default:
                        move.Move = "";
                        break;

                }

                if (game.CommandList.Contains(move.Move))
                { 
                    if (move.Move == "BET") { move.Move = $"BET {betvalue}";  }
                    game = _client.PlayBlackJack(move); 
                }

            }

        }
    
    }

}
