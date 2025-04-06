using RoyalGameOfUr.Models;

namespace RoyalGameOfUr.Game;

public static class GameConfig
{
    public const string PlayerOneColor = "Red";
    public const string PlayerTwoColor = "Blue";

    public const int DiceCount = 4;
    public const int DiceSides = 2;
    public const int MaxPosition = 16;
    public const int ExitPosition = 17;

    public static readonly BoardOrientation[] RotationOrder =
    [
        BoardOrientation.HorizontalLtr, // Wide (Left to Right)
        BoardOrientation.HorizontalRtl, // Wide (Right to Left)
        BoardOrientation.VerticalTtb, // Tall (Top to Bottom)
        BoardOrientation.VerticalBtt // Tall (Bottom to Top)
    ];

    public static class Prompts
    {
        public const string RollPrompt =
            "\nPress any key to roll the dice... (Press 'R' for rotation menu, 'B' to quickly toggle horizontal view, 'V' to toggle vertical view, Esc to exit)";

        public const string NoMovesZeroRoll =
            "No moves possible with a roll of 0. Press any key to continue...";

        public const string NoValidMoves =
            "No valid moves available. Press any key to continue...";

        public const string CaptureMessage =
            "\nYou captured an opponent's piece! Press any key to continue...";

        public const string RosetteMessage =
            "\nLanded on a rosette! You get another turn. Press any key to continue...";

        public const string ExitMessage =
            "\nYour piece has completed the path! Press any key to continue...";

        public const string WinnerMessage =
            "\nPlayer {0} ({1}) has won the game!";

        public const string WinnerPrompt =
            "\nPress any key to return to the menu...";

        public const string CurrentPlayer =
            "\nCurrent Player: Player {0} ({1})";

        public const string RollResult =
            "You rolled a {0}";
    }
}