```
        T h r e e  P e p p e r s  G a m i n g   P r e s e n t s 
************************* S I M P L E  T E X T *************************
♥♥♥♥♥♥  ♥♥       ♥♥♥♥♥   ♥♥♥♥♥♥ ♥♥   ♥♥      ♥♥  ♥♥♥♥♥   ♥♥♥♥♥♥ ♥♥   ♥♥
♥♥   ♥♥ ♥♥      ♥♥   ♥♥ ♥♥      ♥♥  ♥♥       ♥♥ ♥♥   ♥♥ ♥♥      ♥♥  ♥♥
♥♥♥♥♥♥  ♥♥      ♥♥♥♥♥♥♥ ♥♥      ♥♥♥♥♥        ♥♥ ♥♥♥♥♥♥♥ ♥♥      ♥♥♥♥♥
♥♥   ♥♥ ♥♥      ♥♥   ♥♥ ♥♥      ♥♥  ♥♥  ♥♥   ♥♥ ♥♥   ♥♥ ♥♥      ♥♥  ♥♥
♥♥♥♥♥♥  ♥♥♥♥♥♥♥ ♥♥   ♥♥  ♥♥♥♥♥♥ ♥♥   ♥♥  ♥♥♥♥♥  ♥♥   ♥♥  ♥♥♥♥♥♥ ♥♥   ♥♥
```
The game is implemented using my BlackJack Service that was part of this project https://github.com/StevenSSparks/BlackJackServer

### UI
A Single Deck Blackjack Game using a Retro Text Style User Interface
 * [M]ono/Color - Toggle colors between all white and default colors.");
 * [T]ips          : Dispaly this message.");
 * [Q]uit          : Quit and close the game.");
 * [G]ame Help     : Display Game command help messages");
 * [C]asino        : Display \"House\" rules and BlackJack scoring details.");
 
 
 ### Game Commands / Features
 Some commands are only valid at certian times during the game.
* [B]ET[#]   : Default bet of 100 or 1000x NumKey [1] 1000 to [0] 10000.
* [C]ASINO   : \"House\" Rules for betting and card evaluation rules.
* [D]OUBLE   : Doubles your bet and then STANDS your hand.
* [G]AME     : Dispalys this list of commands.
* [H]IT      : Deal a card for hand in play.
* [I]NS      : Purchases insurance and stand on current hand.
* [N]EW      : Restarts the game and resets player and points.
* [S]TAND    : Player lets computer finish the game on main hand.
 
 ### House Rules:
 ##### BlackJack pays 3 x points. (Bet 100 win 300), BlackJack is an ACE and a FACE Card. A 10 is not a face card.
 * Deck is 52 cards w/o Jokers.
 * The deck will shuffle when the last card is used.
 * House draws on 16 or less and stands on 17.
 * Insurance requires enough points for equal bets. Insurance costs 100%.
 * Insurance is betting aginst the dealer having Blackback. (see below)
 * Double requires enough points remaining to double.
 * The player receives 10000 points. Each new play session.
 * Minimum Best is 100 points.
 * Beat the house when the player exceeds 50000 points.
 * When a player has 0 points, the house wins.

 # Unique Features
 While this game is deveoped with a retro look and feel the game 
 * Uses a single deck of cards. Each card is unique and is not reused unless the deck is shuffled. 
 * When the deck is shuffeled, any cards in use are not included in the deck. 
 
