using RoyalGameOfUr.Models;
using RoyalGameOfUr.Rendering.Interfaces;
using RoyalGameOfUr.UI.Interfaces;
using System.Text;

namespace RoyalGameOfUr.UI
{
    /// <summary>
    /// Handles menu display and interaction
    /// </summary>
    public class MenuDisplay(IConsoleWrapper console, IBoardRendererFactory rendererFactory)
    {
        private readonly IConsoleWrapper _console = console ?? throw new ArgumentNullException(nameof(console));
        private readonly IBoardRendererFactory _rendererFactory = rendererFactory ?? throw new ArgumentNullException(nameof(rendererFactory));

        private readonly string[] _options = ["Tall", "Wide"];

        // Board orientation rotation order
        private readonly BoardOrientation[] _rotationOrder =
        [
            BoardOrientation.VerticalBtt,    // Tall (Bottom to Top)
            BoardOrientation.HorizontalLtr,   // Wide (Left to Right)
            BoardOrientation.VerticalTtb,    // Tall (Top to Bottom)
            BoardOrientation.HorizontalRtl    // Wide (Right to Left)
        ];

        /// <summary>
        /// Shows the menu and handles user interaction
        /// </summary>
        public void Show()
        {
            var exitProgram = false;

            while (!exitProgram)
            {
                _console.Clear();

                var selectedIndex = 0;

                var screenBuffer = RenderMenuToBuffer(_options, selectedIndex);
                DisplayBuffer(screenBuffer);

                var selectionMade = false;

                while (!selectionMade && !exitProgram)
                {
                    var keyInfo = _console.ReadKey(true);
                    var keyPressed = keyInfo.Key;

                    switch (keyPressed)
                    {
                        case ConsoleKey.Escape:
                            exitProgram = true;
                            _console.Clear();
                            _console.WriteLine("Program exited.");
                            break;
                        case ConsoleKey.Enter:
                            selectionMade = true;
                            _console.Clear();
                            HandleSelection(selectedIndex);
                            break;
                        case ConsoleKey.UpArrow when selectedIndex > 0:
                            selectedIndex--;
                            break;
                        case ConsoleKey.DownArrow when selectedIndex < _options.Length - 1:
                            selectedIndex++;
                            break;
                        default:
                            continue;
                    }

                    if (exitProgram || selectionMade) continue;

                    screenBuffer = RenderMenuToBuffer(_options, selectedIndex);
                    DisplayBuffer(screenBuffer);
                }
            }

            if (exitProgram)
            {
                Environment.Exit(0);
            }
        }

        /// <summary>
        /// Renders the menu to a buffer
        /// </summary>
        private static string[] RenderMenuToBuffer(string[] options, int selectedIndex)
        {
            var buffer = new List<string>
            {
                "",
                "=====================",
                "THE ROYAL GAME OF UR",
                "=====================",
                ""
            };
            buffer.AddRange(options.Select((t, i) => i == selectedIndex ? $"> {i + 1}. {t} <" : $"  {i + 1}. {t}  "));

            buffer.Add("");
            buffer.Add("Use arrow keys to navigate, Enter to select, or Esc to quit.");

            return buffer.ToArray();
        }

        /// <summary>
        /// Displays the buffer
        /// </summary>
        private void DisplayBuffer(string[] buffer)
        {
            Console.SetCursorPosition(0, 0);

            var sb = new StringBuilder();

            foreach (var line in buffer)
            {
                if (line.StartsWith("> ") && line.EndsWith(" <"))
                {
                    sb.Append("\e[47m\e[30m"); // ANSI escape code for gray background, black text
                    sb.Append(line);
                    sb.Append("\e[0m"); // Reset formatting
                }
                else
                {
                    sb.Append(line);
                }

                sb.Append(new string(' ', Math.Max(0, Console.WindowWidth - line.Length)));
                sb.Append('\n');
            }

            var remainingLines = Math.Max(0, Console.WindowHeight - buffer.Length - 1);
            for (var i = 0; i < remainingLines; i++)
            {
                sb.Append(new string(' ', Console.WindowWidth));
                sb.Append('\n');
            }

            _console.Write(sb.ToString());
        }

        /// <summary>
        /// Handles the user's selection
        /// </summary>
        private void HandleSelection(int selectedIndex)
        {
            var orientation = selectedIndex == 0
                ? BoardOrientation.VerticalBtt
                : BoardOrientation.HorizontalLtr;

            DisplayBoard(orientation);
        }

        /// <summary>
        /// Gets the orientation name to display
        /// </summary>
        private static string GetOrientationName(BoardOrientation orientation)
        {
            return orientation switch
            {
                BoardOrientation.HorizontalLtr => "Wide (Left to Right)",
                BoardOrientation.HorizontalRtl => "Wide (Right to Left)",
                BoardOrientation.VerticalTtb => "Tall (Top to Bottom)",
                BoardOrientation.VerticalBtt => "Tall (Bottom to Top)",
                _ => "Unknown"
            };
        }

        /// <summary>
        /// Gets the next orientation in rotation
        /// </summary>
        private BoardOrientation GetNextOrientation(BoardOrientation current)
        {
            var index = Array.IndexOf(_rotationOrder, current);
            if (index == -1) return BoardOrientation.HorizontalLtr;

            var nextIndex = (index + 1) % _rotationOrder.Length;
            return _rotationOrder[nextIndex];
        }

        /// <summary>
        /// Displays the board with the specified orientation and handles keyboard input
        /// </summary>
        private void DisplayBoard(BoardOrientation orientation)
        {
            while (true)
            {
                _console.Clear();

                var gameBoard = GameBoard.CreateDefault();
                var orientationName = GetOrientationName(orientation);
                var renderer = _rendererFactory.CreateRenderer(orientation);

                renderer.RenderBoard(gameBoard.GetBoardData());

                _console.WriteLine($"Current view: {orientationName}");
                _console.WriteLine("\nControls:");
                _console.WriteLine("  Space: Rotate view 90° clockwise");
                _console.WriteLine("  Escape: Exit program");
                _console.WriteLine("  Any other key: Return to menu");

                var keyInfo = _console.ReadKey(true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Escape:
                        _console.Clear();
                        Environment.Exit(0);
                        break;
                    case ConsoleKey.Spacebar:
                        orientation = GetNextOrientation(orientation);
                        break;
                    default:
                        return;
                }
            }
        }
    }
}