using RoyalGameOfUr.Rendering;
using RoyalGameOfUr.Rendering.Interfaces;
using RoyalGameOfUr.UI;
using RoyalGameOfUr.UI.Interfaces;
using System.Text;

try
{
    Console.Title = "The Royal Game of Ur";
    Console.OutputEncoding = Encoding.UTF8;
    Console.CursorVisible = false;

    IConsoleWrapper console = new ConsoleWrapper();
    ICellStyleProvider cellStyleProvider = new CellStyleProvider(console);
    IBoardRendererFactory rendererFactory = new BoardRendererFactory(console, cellStyleProvider);

    var menu = new MenuDisplay(console, rendererFactory);
    menu.Show();
}
catch (Exception ex)
{
    Console.Clear();
    Console.CursorVisible = true;
    Console.WriteLine($"An error occurred: {ex.Message}");
    Console.WriteLine(ex.StackTrace);
    Console.WriteLine("Press any key to exit...");
    Console.ReadKey();
}
finally
{
    Console.CursorVisible = true;
}