using RoyalGameOfUr.UI.Interfaces;

namespace RoyalGameOfUr.UI;

/// <summary>
///     Wrapper for System.Console to enable testability
/// </summary>
public class ConsoleWrapper : IConsoleWrapper
{
    /// <summary>
    ///     Writes a string to the console
    /// </summary>
    public void Write(string value)
    {
        Console.Write(value);
    }

    /// <summary>
    ///     Writes a line to the console
    /// </summary>
    public void WriteLine(string value = "")
    {
        Console.WriteLine(value);
    }

    /// <summary>
    ///     Gets the current foreground color
    /// </summary>
    public ConsoleColor ForegroundColor => Console.ForegroundColor;

    /// <summary>
    ///     Gets the current background color
    /// </summary>
    public ConsoleColor BackgroundColor => Console.BackgroundColor;

    /// <summary>
    ///     Sets the foreground color of the console
    /// </summary>
    public void SetForegroundColor(ConsoleColor color)
    {
        Console.ForegroundColor = color;
    }

    /// <summary>
    ///     Sets the background color of the console
    /// </summary>
    public void SetBackgroundColor(ConsoleColor color)
    {
        Console.BackgroundColor = color;
    }

    /// <summary>
    ///     Resets the console colors to default
    /// </summary>
    public void ResetColors()
    {
        Console.ResetColor();
    }

    /// <summary>
    ///     Reads a key from the console
    /// </summary>
    public ConsoleKeyInfo ReadKey(bool intercept = false)
    {
        return Console.ReadKey(intercept);
    }

    /// <summary>
    ///     Clears the console
    /// </summary>
    public void Clear()
    {
        Console.Clear();
    }

    /// <summary>
    ///     Gets the current cursor left position
    /// </summary>
    public int CursorLeft => Console.CursorLeft;

    /// <summary>
    ///     Gets the current cursor top position
    /// </summary>
    public int CursorTop => Console.CursorTop;

    /// <summary>
    ///     Sets the cursor position
    /// </summary>
    public void SetCursorPosition(int left, int top)
    {
        Console.SetCursorPosition(left, top);
    }
}