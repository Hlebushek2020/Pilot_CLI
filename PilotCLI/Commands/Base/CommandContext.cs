namespace PilotCLI.Commands.Base;

public class CommandContext
{
    public string? Args { get; }
    public CommandManager Manager { get; }

    public CommandContext(string? args, CommandManager manager)
    {
        Args = args;
        Manager = manager;
    }
}