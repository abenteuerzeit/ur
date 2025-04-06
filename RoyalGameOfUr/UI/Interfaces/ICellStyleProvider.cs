using RoyalGameOfUr.Models;

namespace RoyalGameOfUr.UI.Interfaces;

/// <summary>
///     Interface for providing cell styling
/// </summary>
public interface ICellStyleProvider
{
    /// <summary>
    ///     Apply the appropriate style for a cell type
    /// </summary>
    void ApplyCellStyle(CellType cellType);

    /// <summary>
    ///     Get the line content for a cell type
    /// </summary>
    string GetCellLine(CellType cellType, int lineIndex);
}