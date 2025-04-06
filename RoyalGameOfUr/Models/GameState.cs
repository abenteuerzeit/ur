namespace RoyalGameOfUr.Models;

/// <summary>
///     Represents the state of the game
/// </summary>
public class GameState
{
    /// <summary>
    ///     Creates a new game state
    /// </summary>
    public GameState(GameBoard board)
    {
        Board = board;

        // Define player paths
        var player1Path = new[,]
        {
            { 0, 0, 0, 0, -1, -1, 12, 13 },
            { 5, 6, 7, 8, 9, 10, 11, 14 },
            { 4, 3, 2, 1, -1, -1, 16, 15 }
        };

        var player2Path = new[,]
        {
            { 4, 3, 2, 1, -1, -1, 16, 15 },
            { 5, 6, 7, 8, 9, 10, 11, 14 },
            { 0, 0, 0, 0, -1, -1, 12, 13 }
        };

        Player1 = new Player(1, ConsoleColor.Red, player1Path);
        Player2 = new Player(2, ConsoleColor.Blue, player2Path);

        CurrentPlayer = Player1;
    }

    /// <summary>
    ///     Gets the game board
    /// </summary>
    public GameBoard Board { get; }

    /// <summary>
    ///     Gets the first player
    /// </summary>
    public Player Player1 { get; }

    /// <summary>
    ///     Gets the second player
    /// </summary>
    public Player Player2 { get; }

    /// <summary>
    ///     Gets the current player
    /// </summary>
    public Player CurrentPlayer { get; private set; }

    /// <summary>
    ///     Gets the opponent player
    /// </summary>
    private Player OpponentPlayer => CurrentPlayer == Player1 ? Player2 : Player1;

    /// <summary>
    ///     Gets a game piece at the specified board coordinates, or null if none
    /// </summary>
    public GamePiece? GetPieceAt(int row, int col)
    {
        var player1Position = Player1.Path[row, col];
        var player2Position = Player2.Path[row, col];

        if (player1Position is > 0 and <= 16)
        {
            var piece = Player1.Pieces.FirstOrDefault(p => p.Position == player1Position);
            if (piece != null) return piece;
        }

        if (player2Position is <= 0 or > 16) return null;
        {
            var piece = Player2.Pieces.FirstOrDefault(p => p.Position == player2Position);
            if (piece != null) return piece;
        }

        return null;
    }

    /// <summary>
    ///     Checks if a position is in a safe zone
    /// </summary>
    public static bool IsInSafeZone(int position)
    {
        return position is >= 1 and <= 4;
    }

    /// <summary>
    ///     Checks if a position is a rosette
    /// </summary>
    public bool IsRosette(int position)
    {
        // Get the board coordinates for this position
        var (row, col) = CurrentPlayer.GetBoardCoordinates(position);
        if (row < 0 || col < 0) return false;

        return Board.GetBoardData()[row, col] == CellType.Rosette;
    }

    /// <summary>
    ///     Switches to the other player
    /// </summary>
    public void SwitchPlayer()
    {
        CurrentPlayer = CurrentPlayer == Player1 ? Player2 : Player1;
    }

    /// <summary>
    ///     Adds a new piece to the board at the specified position for the current player
    /// </summary>
    public void AddPiece(int position)
    {
        // Find first piece that's off the board
        var piece = CurrentPlayer.Pieces.FirstOrDefault(p => p.Position == -1);
        if (piece == null) return; // No pieces available to add

        // Place at the specified position
        piece.Position = position;
    }

    /// <summary>
    ///     Moves a piece by the specified number of spaces
    /// </summary>
    public static void MovePiece(GamePiece piece, int spaces)
    {
        if (spaces <= 0) return;

        var newPosition = piece.Position + spaces;

        // Check if this would move the piece off the board
        if (newPosition > 16)
        {
            // Piece can only leave on exact roll
            if (newPosition != 17) return;
            piece.Position = 17; // Mark as exited
            return;
        }

        // Update the position
        piece.Position = newPosition;
    }

    /// <summary>
    ///     Checks and handles potential capture when a piece is moved
    /// </summary>
    public bool CheckCapture(GamePiece piece)
    {
        if (piece is not { IsOnBoard: true }) return false;

        // Get the board coordinates for this piece
        var (row, col) = CurrentPlayer.GetBoardCoordinates(piece.Position);

        // Check if this is a safe zone for the current player
        if (IsInSafeZone(piece.Position)) return false; // Can't capture in safe zones

        // Check if this is a rosette
        if (Board.GetBoardData()[row, col] == CellType.Rosette) return false; // Can't capture on rosettes

        // Check if this is a shared space (middle section)
        var opponentPosition = OpponentPlayer.Path[row, col];
        if (opponentPosition is <= 0 or > 16) return false; // Not a shared space

        // Check if opponent has a piece on this space
        var opponentPiece = OpponentPlayer.Pieces.FirstOrDefault(p => p.Position == opponentPosition);
        if (opponentPiece == null) return false; // No opponent piece to capture

        // Capture the piece (return it to start)
        opponentPiece.Position = -1;
        return true;
    }

    /// <summary>
    ///     Gets the winner if any, or null if the game is still in progress
    /// </summary>
    public Player? GetWinner()
    {
        if (Player1.GetCompletedPiecesCount() >= 7) return Player1;

        return Player2.GetCompletedPiecesCount() >= 7 ? Player2 : null;
    }
}