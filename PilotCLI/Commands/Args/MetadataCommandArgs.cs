namespace PilotCLI.Commands.Args;

public class MetadataCommandArgs
{
    public ICollection<string> Args { get; }

    private MetadataCommandArgs(bool appendToFile, string? filePath, ICollection<string> args) { Args = args; }

    public static MetadataCommandArgs Parse(string args, ISet<string> aviableArgs)
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
            args = args.Remove(fileIndex);
        }

        List<string> argList = new List<string>();
        foreach (string arg in args.Replace("  ", " ").Trim().Split(' '))
        {
            if (aviableArgs.Contains(arg))
                argList.Add(arg);
        }
        return new MetadataCommandArgs(isOverride, filePath, argList);
    }
}