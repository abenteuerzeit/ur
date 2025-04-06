namespace RoyalGameOfUr.Models;

public enum CellType
{
    Disabled = -1, // Not part of the board
    Rosette = 0, // Special rosette cell (star pattern)
    Plain = 1, // Plain cell
    Eye = 2, // Eye pattern
    Dots = 3, // Five dots pattern
    Cross = 4, // Cross pattern
    ZigZag = 5 // Zig-zag pattern
}