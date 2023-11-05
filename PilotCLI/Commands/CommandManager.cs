using PilotCLI.Commands.Base;

namespace PilotCLI.Commands;

public class CommandManager
{
    public const string OutputToFile = "[ >> > ] FILE";

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

        OutToFile? outToFile = null;

        bool isOverride = false;
        string commandArgs = commandLine.Remove(0, index + 1);
        int fileIndex = commandArgs.IndexOf('>');
        if (fileIndex != -1)
        {
            string filePath = commandArgs[fileIndex..];
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
            commandArgs = commandArgs.Remove(fileIndex);
            outToFile = new OutToFile(Console.Out, filePath, isOverride);
            Console.SetOut(outToFile);
        }

        commandHandler.Execute(new CommandContext(commandArgs, this));

        outToFile?.CloseFile();

        return true;
    }
}