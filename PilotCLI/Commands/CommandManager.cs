namespace PilotCLI.Commands;

public class CommandManager
{
    private readonly Dictionary<string, ICommandHandler> _commands = new Dictionary<string, ICommandHandler>();

    public bool RegisterCommand(string command, ICommandHandler commandHandler) =>
        _commands.TryAdd(command.ToLower(), commandHandler);

    public bool Process(string? commandLine)
    {
        if (string.IsNullOrWhiteSpace(commandLine))
            return false;

        commandLine = commandLine.Replace("  ", " ");
        int index = commandLine.IndexOf(' ');
        string command = index != -1 ? commandLine[..index].ToLower() : commandLine.ToLower();

        if (!_commands.TryGetValue(command, out ICommandHandler? commandHandler))
        {
            ConsoleUtils.WriteLineWarning($"Command \"{command}\" not found");
            return false;
        }

        string? commandArgs = index != -1 ? commandLine.Remove(0, index + 1) : null;
        commandHandler.Execute(commandArgs);

        return true;
    }
}