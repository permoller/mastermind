# Mastermind
Implementation of the code breaker game Mastermind.

<https://en.wikipedia.org/wiki/Mastermind_(board_game)>

__Directories:__
- __[Mastermind.GameLogic](https://github.com/permoller/mastermind/tree/master/Mastermind.GameLogic):__ Game models and line comparing algorithm for the game.
  - Pegs are represented as integers. There is no special type for it.
  - Lines are represented as arrays of integers. There is no special type for it.
  - [Game](https://github.com/permoller/mastermind/blob/master/Mastermind.GameLogic/Game.cs): A class that is initialized with the number of different pegs, the number of pegs per line, the allowed number of guesses and the secret line that a player should guess. It also contains a Play-method that takes an implementation of `IPlayer` that it calls to let the `IPlayer` guess the secret.
  - [IPlayer](https://github.com/permoller/mastermind/blob/master/Mastermind.GameLogic/IPlayer.cs): The interface that needs to be implemented by algorithms that tries to guess the secret.
  - [LineComparer](https://github.com/permoller/mastermind/blob/master/Mastermind.GameLogic/LineComparer.cs): The algorithm for comparing a guess to a secret and getting the number of pegs at the correct and wrong position in a [`Result`](https://github.com/permoller/mastermind/blob/master/Mastermind.GameLogic/Result.cs) object. Public so it can be reused by imeplementations of `IPlayer`.
- __[Mastermind.HumanPlayer](https://github.com/permoller/mastermind/tree/master/Mastermind.HumanPlayer):__ Console application for playing the game manually.
- __[Mastermind.PerformanceTestRunner](https://github.com/permoller/mastermind/tree/master/Mastermind.PerformanceTestRunner):__ Console application for running performance tests using the different algorithms implementing `IPlayer`.
- __Mastermind.Algorithms.\*:__ Different algorithms implementing `IPlayer`.
