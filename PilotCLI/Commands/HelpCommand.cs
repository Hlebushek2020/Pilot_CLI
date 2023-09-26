using PilotCLI.Commands.Base;

namespace PilotCLI.Commands;

public class HelpCommand : ICommand
{
    public string Name { get; } = "help";
    public string Description { get; } = "Show all commands";

    public HelpCommand() { Console.WriteLine("Type \"help\" to view a list of commands"); }

    public bool Execute(CommandContext commandCtx)
    {
        if (!string.IsNullOrWhiteSpace(commandCtx.Args) &&
            commandCtx.Manager.Commands.TryGetValue(commandCtx.Args.ToLower(), out ICommand? command))
        {
            command.Help();
        }
        else
        {
            foreach (KeyValuePair<string, ICommand> pair in commandCtx.Manager.Commands)
            {
                Console.ForegroundColor = CommandConstants.CommandColor;
                Console.Write(pair.Key);
                Console.Write(": ");
                Console.ResetColor();
                Console.WriteLine(pair.Value.Description);
            }
        }
        return true;
    }

    public void Help()
    {
        Console.ForegroundColor = CommandConstants.CommandColor;
        Console.WriteLine(Name);
        Console.ResetColor();
        Console.WriteLine(Description);
        Console.ForegroundColor = CommandConstants.CommandColor;
        Console.WriteLine($"{Name} <command>");
        Console.ResetColor();
        Console.WriteLine("Shows help on \"command\"");
    }
}