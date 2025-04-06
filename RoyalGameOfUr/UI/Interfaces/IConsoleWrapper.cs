namespace RoyalGameOfUr.UI.Interfaces
{
    /// <summary>
    /// Interface for console operations to enable testability
    /// </summary>
    public interface IConsoleWrapper
    {
        /// <summary>
        /// Writes a string to the console
        /// </summary>
        void Write(string value);

        /// <summary>
        /// Writes a line to the console
        /// </summary>
        void WriteLine(string value = "");

        /// <summary>
        /// Sets the foreground color of the console
        /// </summary>
        void SetForegroundColor(ConsoleColor color);

        /// <summary>
        /// Sets the background color of the console
        /// </summary>
        void SetBackgroundColor(ConsoleColor color);

        /// <summary>
        /// Resets the console colors to default
        /// </summary>
        void ResetColors();

        /// <summary>
        /// Reads a key from the console
        /// </summary>
        ConsoleKeyInfo ReadKey(bool intercept = false);

        /// <summary>
        /// Clears the console
        /// </summary>
        void Clear();
    }
}