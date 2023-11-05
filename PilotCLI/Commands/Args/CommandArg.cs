namespace PilotCLI.Commands.Args;

public class CommandArg : ICommandArg
{
    public bool AppendToFile { get; }
    public string? FilePath { get; }

    private CommandArg(bool appendToFile, string? filePath)
    {
        AppendToFile = appendToFile;
        FilePath = filePath;
    }

    public static CommandArg Parse(string args, ISet<string> aviableArgs)
    {
        string? filePath = null;
        bool isOverride = false;
        int fileIndex = args.IndexOf('>');
        if (fileIndex != -1)
        {
            filePath = args[fileIndex..];
            if (!string.IsNullOrWhiteSpace(filePath))
            {
                if (filePath.StartsWith(">>"))
                {
                    isOverride = true;
                    filePath = filePath.Remove(0, 2).Trim();
                }
                else
                {
                    filePath = filePath.Remove(0, 1).Trim();
                }
            }
        }
        return new CommandArg(isOverride, filePath);
    }
}