using PilotCLI.Commands.Base;

namespace PilotCLI.Commands;

public class CommandManager
{
    private readonly Dictionary<string, ICommand> _commands = new Dictionary<string, ICommand>();

    public IReadOnlyDictionary<string, ICommand> Commands => _commands;

    public bool RegisterCommand(ICommand command) => _commands.TryAdd(command.Name, command);

    public bool Process(string? commandLine)
    {
        if (string.IsNullOrWhiteSpace(commandLine))
            return false;

        commandLine = commandLine.Replace("  ", " ");
        int index = commandLine.IndexOf(' ');
        string command = index != -1 ? commandLine[..index].ToLower() : commandLine.ToLower();

        if (!_commands.TryGetValue(command, out ICommand? commandHandler))
        {
            Console.WriteLine($"Command \"{command}\" not found!");
            return false;
        }

        string? commandArgs = index != -1 ? commandLine.Remove(0, index + 1) : null;
        commandHandler.Execute(new CommandContext(commandArgs, this));

        return true;
    }
}