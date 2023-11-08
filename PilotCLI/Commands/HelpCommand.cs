using PilotCLI.Commands.Base;

namespace PilotCLI.Commands;

public class HelpCommand : ICommand
{
    private readonly ISettings _settings;

    public string Name { get; } = "help";
    public string Description { get; } = "Show all commands";

    public HelpCommand(ISettings settings)
    {
        _settings = settings;
        Console.WriteLine("Type \"help\" to view a list of commands");
    }

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
                Console.ForegroundColor = _settings.CommandSignatureColor;
                Console.Write(pair.Key);
                Console.ForegroundColor = _settings.OtherTextColor;
                Console.Write(": ");
                Console.WriteLine(pair.Value.Description);
            }

            Console.ForegroundColor = _settings.CommandSignatureColor;
            Console.Write("exit");
            Console.ForegroundColor = _settings.OtherTextColor;
            Console.WriteLine(": Close the console");
        }
        return true;
    }

    public void Help()
    {
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.WriteLine($"{Name} {CommandManager.OutputToFile}");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine(Description);
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.WriteLine($"{Name} COMMAND {CommandManager.OutputToFile}");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine("Shows help on \"COMMAND\"");
    }
}