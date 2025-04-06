using RoyalGameOfUr.Models;
using RoyalGameOfUr.Rendering.Interfaces;
using RoyalGameOfUr.Rendering.Renderers;
using RoyalGameOfUr.UI.Interfaces;

namespace RoyalGameOfUr.Rendering
{
    /// <summary>
    /// Factory for creating board renderers
    /// </summary>
    public class BoardRendererFactory(IConsoleWrapper console, ICellStyleProvider cellStyleProvider)
        : IBoardRendererFactory
    {
        private readonly IConsoleWrapper _console = console ?? throw new ArgumentNullException(nameof(console));
        private readonly ICellStyleProvider _cellStyleProvider = cellStyleProvider ?? throw new ArgumentNullException(nameof(cellStyleProvider));

        /// <summary>
        /// Creates a board renderer for the specified orientation
        /// </summary>
        public IBoardRenderer CreateRenderer(BoardOrientation orientation)
        {
            return orientation switch
            {
                BoardOrientation.HorizontalLtr => new HorizontalBoardRenderer(_console, _cellStyleProvider, false),
                BoardOrientation.HorizontalRtl => new HorizontalBoardRenderer(_console, _cellStyleProvider, true),
                BoardOrientation.VerticalTtb => new VerticalBoardRenderer(_console, _cellStyleProvider, false),
                BoardOrientation.VerticalBtt => new VerticalBoardRenderer(_console, _cellStyleProvider, true),
                _ => throw new ArgumentOutOfRangeException(nameof(orientation), orientation, "Unsupported board orientation")
            };
        }
    }
}