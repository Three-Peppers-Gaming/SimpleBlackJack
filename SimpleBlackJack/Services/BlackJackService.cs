﻿using Microsoft.Extensions.Caching.Memory;
using SimpleBlackJack.Services.Models;
using System.Reflection.Metadata.Ecma335;

namespace SimpleBlackJack.Services
{
    

    public class BlackJackService : IBlackJack
    {
        // storage for the each blackjack game
        private readonly IMemoryCache _gameCache;

        public BlackJackService(IMemoryCache GameCache)
        {
            _gameCache = GameCache;
        }

        #region CachingMethods

        private void Cache_AddGame(BlackJackGame g)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
              // Keep in cache for this time, reset time if accessed.
              .SetSlidingExpiration(TimeSpan.FromMinutes(8 * 60)); //  8 hours
            _ = _gameCache.Set(g.Id, g, cacheEntryOptions);
        }

        private void Cache_UpdateGame(BlackJackGame g)
        {
            _gameCache.Remove(g.Id);
            Cache_AddGame(g);
        }

        private BlackJackGame Cache_GetGame(string key)
        {
            var cacheEntry = _gameCache.Get<BlackJackGame>(key);

            if (cacheEntry == null)
            {
                var g = new BlackJackGame();
                g.Id = key;
                Cache_AddGame(g);
                cacheEntry = _gameCache.Get<BlackJackGame>(key);
            }

            return cacheEntry;
        }

        private void Cache_RemoveGame(string key)
        {
            _gameCache.Remove(key);

        }

        private BlackJackGame Cache_PlayGame(BlackJackGame game)
        {

            BlackJackGame g = Cache_GetGame(game.Id);

            if (g == null)
            {
                string tempId = Guid.NewGuid().ToString();

                g = new BlackJackGame();
                g.Id = tempId;
                g.PlayerWins = 0;
                g.ComputerWins = 0;
                g.Deck = ShuffleCards();

                Cache_AddGame(g);
            }

            return g;
        }

        #endregion CachingMethiods

        #region Player Help and Rules 

        public List<string> HouseRules()
        {
            List<string> message = new()
            {
                "House Rules:",
                "BlackJack pays 3 x points. (Bet 100 win 300).",
                "BlackJack is an ACE and a FACE Card. A 10 is not a face card.",
                "Deck is 52 cards w/o Jokers.",
                "The single deck will shuffle when the last card of the deck is used.",
                "House draws on 16 or less and stands on 17.",
                "Insurance requires enough points for equal bets. Insurance costs 100%.",
                "Insurance is betting against the dealer having Blackback. ",
                "Double requires enough points remaining to double your bet.",
                //"Splits and Insurance require enough points for equal bets",
                //"Splits are allowed when a player gets on any card of the same value.",
                "The player receives 10000 points at the beginning of the game.",
                "Minimum default Bet is 100 points. Other bets are in multiples of 1000.",
                "Beat the house when the player exceeds 50000 points.",
                "When a player has 0 points, the house wins."
            };

            return message;
        }

        public List<string> CommandList()
        {
            List<string> message = new()
            {
                "Valid Commands:",
                "Some commands are only valid at certain times during the game.",
                "BET #    : Bets 100 or 1000 x the the number specified [1] = 1000 to [0] = 10000.",
                "DOUBLE   : Doubles your bet and then STANDS your hand.",
                "GAME     : Dispalys this list of commands.",
                "HIT      : Deal a card for hand in play.",
                "INS      : Purchases insurance and stand on the current hand.",
                "CASINO   : Provides a list of the betting and playting rules.",
                "NEW      : Restarts the game and resets player and points.",
                "STAND    : Player lets computer finish the game on the main hand."
            };

            return message;
        }

        public List<string> CommandListWithBrackets()
        {
            List<string> message = new()
            {
                "Game Keyboard Commands:",
                "Some keys are only valid at certain times during the game.",
                "[B]ET[#]   : Default bet of 100 or 1000x NumKey [1] 1000 to [0] 10000.",
                "[C]ASINO   : \"House\" Rules for betting and card evaluation rules.",
                "[D]OUBLE   : Doubles your bet and then STANDS your hand",
                "[G]AME     : Displays this list of commands.",
                "[H]IT      : Deal a card for hand in play.",
                "[I]NS      : Purchases insurance and stand on the current hand.",
                "[N]EW      : Restarts the game and resets player and points.",
                "[S]TAND    : Player lets computer finish the game on the main hand.",
            };

            return message;
        }



        #endregion Player Help and Rules 

        public BlackJackMoveResult PlayBlackJack(GameMove move)
        {
            BlackJackGame game = new();

            if (move.InstanceID == null) move.InstanceID = "";
            game.Id = move.InstanceID;
            game = Cache_PlayGame(game); // if the instance does not exist a new game will be started. 
            game.PlayerMove = move.Move.ToLower().Trim();

            game.Messages = new(); // clear out transient lists. 
            game.Command = String.Empty;
            game.Modifier = String.Empty;

            // parse the command and modifier 
            var command = move.Move.ToLower().Trim();
            var modifier = string.Empty;
            if (command.Contains(" "))
            {
                modifier = command.Split(" ")[1];
                command = command.Split(' ')[0];
            }

            game.Command = command;
            game.Modifier = modifier;

            game = ProcessMove(game);
            Cache_UpdateGame(game);
            return SetUpResult(game);

        }

        BlackJackMoveResult SetUpResult(BlackJackGame game)
        {
            BlackJackMoveResult bmr = new();
            bmr.Id = game.Id;
            bmr.PlayerWins = game.PlayerWins;
            bmr.ComputerWins = game.ComputerWins;

            bmr.PlayerPoints = game.PlayerPoints;

            bmr.PlayerCards = game.PlayerCards;
            bmr.PlayerCardsBet = game.PlayerCardsBet;
            bmr.PlayerCardsTotal = CardTotal(game.PlayerCards);
            bmr.PlayerCardsActive = game.PlayerCardsActive;

            bmr.PlayerHasInsurance = game.PlayerHasInsurange;

            bmr.PlayerSplitCards = game.PlayerSplitCards;
            bmr.PlayerSplitBet = game.PlayerSplitBet;
            bmr.PlayerSplotTotal = CardTotal(game.PlayerSplitCards);
            bmr.PlayerSplitActive = game.PlayerSplitActive;

            // Computer Card Total based on if first card is face down
            if (game.ComputerCards.Count > 0)
            {
                if (game.ComputerFaceDownCard == false)
                {
                    bmr.ComputerCardTotal = CardTotal(game.ComputerCards);
                    bmr.ComputerCards = game.ComputerCards;
                }
                else
                {
                    bmr.ComputerCards.Add(new Card { Id = 0, Name = "Face Down", Value = 0 });
                    bmr.ComputerCards.Add(game.ComputerCards[1]);
                    bmr.ComputerCardTotal = game.ComputerCards[1].Value;
                }
            }

            (game.CommandList, game.CommandString, game.CommandStringWithBrackets) = CommandList(game);

            bmr.CommandStringWithBrackets = game.CommandStringWithBrackets;
            bmr.ComamndString = game.CommandString;
            bmr.CommandList = game.CommandList;
            bmr.Message = game.Messages;

            return bmr;
        }


        // Note - When you deal the next card if you run out of cards the deck reset needs to remove the cards in the players and computers hands from the deck
        // Get the new deck and then use linq to remove the card by Id. 

        #region Game Moves

        private BlackJackGame ProcessMove(BlackJackGame game)
        {
          
            if (game.PlayerPoints > 50000)
            {
                game.GameActive = false;
                return game;
            }

            if ((game.GameActive == true))
            {
                switch (game.Command)
                {
                    case "":
                        break;
                    case "hit":
                        {
                            game = GameMoveHit(game);
                            return game;
                        }
                    case "ins":
                        {
                            game = GameMoveInsurance(game);
                            return game;
                        }
                    case "stand":
                        {
                            game = GameMoveStand(game);
                            return game;
                        }
                    case "double":
                        {
                            game = GameMoveDouble(game);
                            return game; 
                        }
                    case "split":
                        {
                            game.Messages.Add("Split not implemented yet.");
                            return game;
                        }
                }

            }

            if ((game.Command == "bet") && game.GameActive == false)
            {
                game = GameMoveBet(game);
                return game;
            }
            else if ((game.Command == "bet")) game.Messages.AddRange(new List<string> { "BET is not valid until the hand is complete." });

            if (game.Command == "new")
            {
                game = GameMoveReset(game);
                return game; 
            }

            if (game.Command == "game")
            {
                foreach (string s in CommandList())
                    game.Messages.Add(s);
            }

            if (game.Command == "[g]ame")
            {
                foreach (string s in CommandListWithBrackets())
                    game.Messages.Add(s);
            }

            if (game.Command == "casino")
            {
                foreach (string s in HouseRules())
                    game.Messages.Add(s);
            }

            if (game.Messages.Count == 0)
            {
                if (game.Command == "" && game.GameActive == false) game.Messages.Add($"BET to start a game.");
                else if (game.Command != "") game.Messages.Add($"{game.Command.ToUpper()} is not a valid command.");
            }

            return game;
        }

        private BlackJackGame GameMoveBet(BlackJackGame game)
        {
            // This starts the hand and will check for BlackJack
            game = StartHand(game);
            return game;
        }

        private BlackJackGame GameMoveDouble(BlackJackGame game)
        {
            var bet = game.PlayerCardsBet;

            if (game.PlayerPoints < game.PlayerCardsBet)
            {
                game.Messages.Add("You don't have enough points to double your bet.");
                game.Messages.Add($"You needed {bet - game.PlayerPoints} points.");
                return game;
            }

            if (game.PlayerCards.Count > 2)
            {
                game.Messages.Add("You cannot double once you have taken any cards.");
                return game;
            }


            game.PlayerCardsBet = game.PlayerCardsBet + bet;
            game.PlayerPoints = game.PlayerPoints - bet;

            game.Messages.Add($"You doubled your bet from {bet} to {game.PlayerCardsBet}.");

            game = GameMoveHit(game);

            if (CardTotal(game.PlayerCards) < 21) game = GameMoveStand(game);

            return game;
        }

        private BlackJackGame GameMoveHit(BlackJackGame game)
        {
            if (game.GameActive == true)
            {
                game.PlayerHasInsurange = false;

                if (game.PlayerCardsActive)
                {
                    (var nextcard, game) = NextCard(game);
                    game.PlayerCards.Add(nextcard);
                    game.PlayerCardsTotal = CardTotal(game.PlayerCards);

                    if (game.PlayerCardsTotal > 21) // player looses
                    {
                        if (!game.PlayerSplitActive) // if player has split take money and play on
                        {
                            game.Messages.Add($"The dealer gives you the {nextcard.Name}.");
                            game.Messages.Add($"You bust with a total of {game.PlayerCardsTotal}.");
                            game = ComputerWins(game);
                            return game;

                        }
                    }

                    if (game.PlayerCardsTotal <= 21) // player is still playing 
                    {
                        game.Messages.Add($"The dealer gives you the {nextcard.Name}.");
                        game.Messages.Add($"Your new total is {game.PlayerCardsTotal}.");
                        if (game.PlayerCardsTotal == 21) game = GameMoveStand(game);
                    }
                    else
                    {
                        if (game.PlayerSplitActive == true)
                        {
                            // swap in the split cards
                            game.PlayerCards = game.PlayerSplitCards;
                            game.PlayerCardsBet = game.PlayerSplitBet;
                            game.PlayerCardsActive = true;
                            game.PlayerSplitActive = false;
                            game.Messages.Add("You can now play the Split Hand.");

                        }
                        else
                        {
                            game.GameActive = false;
                        }

                    }

                }
                else game.Messages.Add("Nothing to Hit");
                return game;
            }
            else game.Messages.AddRange(new List<string> { "You can't HIT right now." });

            return game;
        }

        private BlackJackGame GameMoveStand(BlackJackGame game)
        {
            game.PlayerHasInsurange = false;

            if (CardTotal(game.ComputerCards) <= 16) game.Messages.Add($"House has {CardTotal(game.ComputerCards)}. House must hit till over 16.");

            while (CardTotal(game.ComputerCards) <= 16)
            {
                (Card c, game) = NextCard(game);
                game.ComputerCards.Add(c);
                game.Messages.Add($"House hits and receives {c.Name}.");
            }

            var ctotal = CardTotal(game.ComputerCards);
            var ptotal = CardTotal(game.PlayerCards);

            while (game.GameActive)
            {
                // computer busts
                if (ctotal > 21) // draws over 21
                {
                    game = PlayerWins(game);
                    return game;
                }

                if (ptotal > ctotal) // Player has highr cards than the computer. 
                {
                    game = PlayerWins(game);
                    return game;
                }

                if (ctotal == ptotal)
                {

                    game = PushGame(game);
                    return game;

                }

                if (ctotal > ptotal)
                {
                    game = ComputerWins(game);
                    return game;
                }

            }

            game.Messages.Add($"{game.Command} is not a valid command right now");
            return game;
        }

        private BlackJackGame GameMoveInsurance(BlackJackGame game)
        {
            if (game.PlayerHasInsurange == true)
            {
                game.PlayerHasInsurange = false;
                game.Messages.Add($"Player paid {game.PlayerCardsBet} points for insurance.");

                if (HasBlackJack(game.ComputerCards))
                {
                    game.Messages.Add("House has BlackJack!");
                    game.PlayerPoints = game.PlayerPoints - game.PlayerCardsBet;
                    game = ComputerWins(game);
                    return game;
                }
                else
                {
                    game.Messages.Add("House does not have BlackJack!");
                    game.PlayerPoints = game.PlayerPoints - game.PlayerCardsBet;
                    return game;
                }
            }
            else if (game.PlayerHasInsurange == false) game.Messages.Add("INS is not valid right now.");
            return game;
        }

        public BlackJackGame GameMoveReset(BlackJackGame game)
        {
            var newgame = new BlackJackGame();
            newgame = Cache_PlayGame(newgame);
            Cache_RemoveGame(game.Id);
            game = newgame;
            return game;
        }

        #endregion Game Moves


        private bool HasBlackJack(List<Card> cards)
        {
            var total = CardTotal(cards);

            if (cards.Count < 1) return false; // less than 2 cards is not a blackjack bonus;
            if (cards.Count > 2) return false; // more than 2 cards is not a blackjack bonus;
            if (total != 21) return false; // total needs to be 21
            if (cards[0].Name.Contains("10") || cards[1].Name.Contains("10")) return false; // can't be a 10

            // Player has 2 cards, Totals 21 and one of them is not a 10
            // So we have an ACE and a FACE CARD = BLACkJACK

            return true;
        }

        #region Game State Conditions - Win - Loose - Draw

        private BlackJackGame PushGame(BlackJackGame game)
        {

            game.Messages.Add($"Game is a PUSH, House Hand Total of {CardTotal(game.ComputerCards)} is equal to Player Total of {CardTotal(game.PlayerCards)}.");
            game.Messages.Add($"House returns players bet of {game.PlayerCardsBet}.");
            game.PlayerPoints = game.PlayerPoints + game.PlayerCardsBet;
            

            game.PlayerCardsActive = false;
            game.PlayerSplitActive = false;
            game.PlayerCardsBet = 0;
            game.Command = "";
            game.Modifier = "";
            game.PlayerHasInsurange = false;
            game.ComputerFaceDownCard = false;
            game.GameActive = false;
            return game;
        }

        private BlackJackGame PlayerWins(BlackJackGame game)
        {

            game.PlayerWins = game.PlayerWins + 1;
            if (game.PlayerSplitActive == true)
            {
                game.PlayerWins = game.PlayerWins + 1;
                game.Messages.Add("You get 2 wins againts the house!");
            }

            if (HasBlackJack(game.PlayerCards))
            {
                int winnings = game.PlayerCardsBet * 3;
                game.PlayerPoints = game.PlayerPoints + winnings;
                game.Messages.Add($"BlackJack! {winnings} added to player points!");
                
            }
            else
            {
                game.PlayerPoints = game.PlayerPoints + (game.PlayerCardsBet * 2);
                game.Messages.Add($"Player Wins! Points Payout of {game.PlayerCardsBet * 2} added to player points!");
            }


            game.PlayerCardsActive = false;
            game.PlayerCardsBet = 0;
            game.Command = "";
            game.Modifier = "";
            game.PlayerHasInsurange = false;
            game.ComputerFaceDownCard = false;
            game.GameActive = false;


            return game;
        }

        private BlackJackGame ComputerWins(BlackJackGame game)
        {

            game.Messages.Add($"House Wins with a total of {CardTotal(game.ComputerCards)}");
            game.ComputerWins = game.ComputerWins + 1;

            game.PlayerCardsActive = false;
            game.PlayerHasInsurange = false;
            game.PlayerCardsBet = 0;
            game.Command = "";
            game.Modifier = "";
            game.ComputerFaceDownCard = false;
            game.GameActive = false;

            return game;
        }

        #endregion Game State Conditions - Win - Loose - Draw

        #region Game Processing Methods

        private int ParseBet(string bet)
        {
            int number = 0;
            bool isParsable = Int32.TryParse(bet, out number);

            if (isParsable)
                return number;
            else
                return 100;
        }

        private Tuple<int, BlackJackGame> SetBet(BlackJackGame game)
        {
            int bet = 0;
            int playerbet = ParseBet(game.Modifier);
            bet = playerbet;

            if (playerbet == 0)
            {
                if (game.PlayerPoints < 100) bet = game.PlayerPoints;
                else { bet = 100; }
            }

            if (playerbet > game.PlayerPoints)
            {
                bet = game.PlayerPoints;
            }

            if (playerbet < 100 )
            {
                if (game.PlayerPoints < 100) bet = game.PlayerPoints;
                else { bet = 100; }
            }

            game.PlayerPoints = game.PlayerPoints - bet;
            return new Tuple<int, BlackJackGame>(bet,game);        }

        private BlackJackGame StartHand(BlackJackGame game)
        {
            (var bet, game) = SetBet(game);
            game.PlayerCardsBet = bet;

            // deal cards
            game.PlayerCards.Clear();
            game.PlayerSplitCards.Clear(); // not implemented
            game.ComputerCards.Clear();
            game.ComputerFaceDownCard = true;
            game.PlayerHasInsurange = false;
          
            Card? drawcard;
            (drawcard, game) = NextCard(game);
            game.PlayerCards.Add(drawcard);
            (drawcard, game) = NextCard(game);
            game.ComputerCards.Add(drawcard);
            (drawcard, game) = NextCard(game);
            game.PlayerCards.Add(drawcard);
            (drawcard, game) = NextCard(game);
            game.ComputerCards.Add(drawcard);

            game.PlayerCardsTotal = CardTotal(game.PlayerCards);
            game.ComputerCardsTotal = CardTotal(game.ComputerCards);

            game.PlayerCardsActive = true;
            game.ComputerFaceDownCard = true;

            if (HasBlackJack(game.PlayerCards))
            {
                game = PlayerWins(game);
                return game;
            }

            if (game.ComputerCards[1].Name.Contains("Ace"))
            {
                if (game.PlayerPoints >= game.PlayerCardsBet) game.Messages.Add("Do you want Insurance?. Use the INS command.");
                game.PlayerHasInsurange = true;
            }

            game.GameActive = true;

            return game;

        }

        private static Tuple<List<string>, string, string> CommandList(BlackJackGame game)
        {
            var commandList = new List<string>();
            

          
            commandList.Add("CASINO");
            commandList.Add("GAME");

            if ((game.PlayerPoints <= 0) && (game.GameActive == false))
            {
                commandList.Add("NEW");
                game.Messages.Add("You have 0 points. Start a NEW game.");
                
            }

            if ((game.PlayerPoints >= 50000))
            {
                game.GameActive = false;
                commandList.Add("NEW");
                game.Messages.Add("YOU ARE A BIG WINNER! - The House is bankrupt! ");
                if (game.PlayerPoints > 50000) game.Messages.Add("WOW! You have exceeded 50000 points the House can award!");
                game.Messages.Add("Please start a NEW the game.");
            }

            if (game.GameActive == true)
            {
                commandList.Add("HIT");
                commandList.Add("STAND");
                commandList.Add("NEW");
                 if (game.PlayerCards.Count < 3) commandList.Add("DOUBLE");


                if (game.PlayerHasInsurange == true)    
                {
                        commandList.Add("INS");
                }
            }
            else
            {
                if (game.PlayerPoints > 0)
                {
                    commandList.Add("BET");
                }
            }

            commandList.Sort();

            // Create Regular Command List 
            var commands = string.Empty;
            var x = 0;
            var commandCount = commandList.Count;
            foreach (string s in commandList)
            {
                if (x == 0) commands = s; // add first command
                if (x > 0 && x < commandCount-1) commands = commands + ", " + s; // put commas between commands
                if (x == commandCount-1) commands = commands + ", and " + s +"."; // put command and then and before last command
                x++;
            }

            // Create Command list with [F]irst letter brackets 
            x = 0;
            var commandswithbrackets = string.Empty;
            commandCount = commandList.Count;
            foreach (string s in commandList)
            {
                var fl = s.Substring(0,1); var fc = s.Substring(1,s.Length-1);
                if (x == 0) commandswithbrackets = $"[{fl}]{fc}"; // add first command
                if (x > 0 && x < commandCount - 1) commandswithbrackets = commandswithbrackets + ", " + $"[{fl}]{fc}"; // put commas between commands
                if (x == commandCount - 1) commandswithbrackets = commandswithbrackets + ", and " + $"[{fl}]{fc}" + "."; // put command and then and before last command
                x++;
            }



            return new Tuple<List<string>, string, string>(commandList, commands, commandswithbrackets);
        }

        private static List<Card> ComputerCardsList(BlackJackGame game)
        {
            List<Card> cards = new();    
            if (game.ComputerFaceDownCard == true)
            {
                cards.Add(new Card { Id = 0, Name = "Face Down Card", Value = 0 });
                cards.Add(game.ComputerCards[1]);
            }

            return cards;

        }

        private int CardTotal(List<Card> cards)
        {
            // This returns a total for the cards. If the cards are > 21, and the hand has an ACE the total is adjusted. 

            var total = 0;
            foreach (Card c in cards)
                total = total + c.Value;

            if (total > 21) // Check for Aces and change from 11 to 1 - check for 2 aces as needed. 
            {
                var x = 0;
                while (total > 21 && x < cards.Count) // make x is cards.count 
                {
                    if (cards.Count > x)
                    {
                        if (cards[x].Value == 11)
                        {
                            cards[x].Value = 1;
                            total = total - 10;
                        }
                    }
                    x++;
                }

            }

            return total;
        }

        // Test Helpers
        private static Card GetAce => new Card { Id = 27, Name = "Ace of Spades", Value = 11 };
        private static Card GetJack => new Card { Id = 30, Name = "Jack of Spades", Value = 10 };


        private Tuple<Card, BlackJackGame> NextCard(BlackJackGame game)
        {
  
            if (game.Deck.Count == 0)
            {
                List<Card> NewDeck = new();
                NewDeck = ShuffleCards();

                // clear out the active cards from the new deck
                foreach (Card card in game.PlayerCards)
                    NewDeck.Remove(card);
                foreach (Card card in game.PlayerSplitCards) // Split not fully implemented 
                    NewDeck.Remove(card);
                foreach (Card card in game.ComputerCards)
                    NewDeck.Remove(card);

                game.Deck = NewDeck;
            }

            var c = game.Deck[0];
            game.Deck.Remove(c);

            return new Tuple<Card, BlackJackGame>(c, game);
        }

        #endregion Game Processing Methods


        private List<Card> ShuffleCards()
        {
            List<Card> Deck = new()
            {
                new Card { Id=1, Name = "Ace of Diamonds", Value = 11 },
                new Card { Id=2, Name = "King of Diamonds", Value = 10},
                new Card { Id=3, Name = "Queen of Diamonds", Value = 10},
                new Card { Id=4, Name = "Jack of Diamonds", Value = 10},
                new Card { Id=5, Name = "10 of Diamonds", Value = 10},
                new Card { Id=6, Name = "9 of Diamonds", Value = 9},
                new Card { Id=7, Name = "8 of Diamonds", Value = 8},
                new Card { Id=8, Name = "7 of Diamonds", Value = 7},
                new Card { Id=9, Name = "6 of Diamonds", Value = 6},
                new Card { Id=10, Name = "5 of Diamonds", Value = 5},
                new Card { Id=11, Name = "4 of Diamonds", Value = 4},
                new Card { Id=12, Name = "3 of Diamonds", Value = 3},
                new Card { Id=13, Name = "2 of Diamonds", Value = 2},
                new Card { Id=14, Name = "Ace of Hearts", Value = 11 },
                new Card { Id=15, Name = "King of Hearts", Value = 10},
                new Card { Id=16, Name = "Queen of Hearts", Value = 10},
                new Card { Id=17, Name = "Jack of Hearts", Value = 10},
                new Card { Id=18, Name = "10 of Hearts", Value = 10},
                new Card { Id=19, Name = "9 of Hearts", Value = 9},
                new Card { Id=20, Name = "8 of Hearts", Value = 8},
                new Card { Id=21, Name = "7 of Hearts", Value = 7},
                new Card { Id=22, Name = "6 of Hearts", Value = 6},
                new Card { Id=23, Name = "5 of Hearts", Value = 5},
                new Card { Id=24, Name = "4 of Hearts", Value = 4},
                new Card { Id=25, Name = "3 of Hearts", Value = 3},
                new Card { Id=26, Name = "2 of Hearts", Value = 2},
                new Card { Id=27, Name = "Ace of Spades", Value = 11 },
                new Card { Id=28, Name = "King of Spades", Value = 10},
                new Card { Id=29, Name = "Queen of Spades", Value = 10},
                new Card { Id=30, Name = "Jack of Spades", Value = 10},
                new Card { Id=31, Name = "10 of Spades", Value = 10},
                new Card { Id=32, Name = "9 of Spades", Value = 9},
                new Card { Id=33, Name = "8 of Spades", Value = 8},
                new Card { Id=34, Name = "7 of Spades", Value = 7},
                new Card { Id=35, Name = "6 of Spades", Value = 6},
                new Card { Id=36, Name = "5 of Spades", Value = 5},
                new Card { Id=37, Name = "4 of Spades", Value = 4},
                new Card { Id=38, Name = "3 of Spades", Value = 3},
                new Card { Id=39, Name = "2 of Spades", Value = 2},
                new Card { Id=40, Name = "Ace of Clubs", Value = 11 },
                new Card { Id=41, Name = "King of Clubs", Value = 10},
                new Card { Id=42, Name = "Queen of Clubs", Value = 10},
                new Card { Id=43, Name = "Jack of Clubs", Value = 10},
                new Card { Id=44, Name = "10 of Clubs", Value = 10},
                new Card { Id=45, Name = "9 of Clubs", Value = 9},
                new Card { Id=46, Name = "8 of Clubs", Value = 8},
                new Card { Id=47, Name = "7 of Clubs", Value = 7},
                new Card { Id=48, Name = "6 of Clubs", Value = 6},
                new Card { Id=49, Name = "5 of Clubs", Value = 5},
                new Card { Id=50, Name = "4 of Clubs", Value = 4},
                new Card { Id=51, Name = "3 of Clubs", Value = 3},
                new Card { Id=52, Name = "2 of Clubs", Value = 2}
            };

            List<Card> ShuffeledDeck = new();

            while (Deck.Count > 0)
            {
                var R = new Random();
               
                var CardSelection = R.Next(Deck.Count);
                Card Selection = Deck[CardSelection];
                ShuffeledDeck.Add(Selection);
                Deck.RemoveAt(CardSelection);

            }

            return ShuffeledDeck;

        }

    }
}
