namespace PilotCLI.Commands.Args;

public class MetadataCommandArgs
{
    public ICollection<string> Args { get; }

    private MetadataCommandArgs(ICollection<string> args) { Args = args; }

    public static MetadataCommandArgs Parse(string args, ISet<string> aviableArgs)
    {
        List<string> argList = new List<string>();
        foreach (string arg in args.Replace("  ", " ").Trim().Split(' '))
        {
            if (aviableArgs.Contains(arg))
                argList.Add(arg);
        }
        return new MetadataCommandArgs(argList);
    }
}