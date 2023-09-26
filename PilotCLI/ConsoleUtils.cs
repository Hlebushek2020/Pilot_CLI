namespace PilotCLI;

public class ConsoleUtils
{
    public static void WriteLineWarning(string text)
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine(text);
        Console.ResetColor();
    }
}