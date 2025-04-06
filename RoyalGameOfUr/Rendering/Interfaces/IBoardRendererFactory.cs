using RoyalGameOfUr.Models;

namespace RoyalGameOfUr.Rendering.Interfaces;

/// <summary>
///     Interface for board renderer factory
/// </summary>
public interface IBoardRendererFactory
{
    /// <summary>
    ///     Creates a board renderer for the specified orientation
    /// </summary>
    IBoardRenderer CreateRenderer(BoardOrientation orientation);
}