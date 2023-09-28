namespace PilotCLI.Commands.Base;

/// <summary>
/// Represents command parameters
/// </summary>
public class CommandContext
{
    /// <summary>
    /// Command arguments
    /// </summary>
    public string? Args { get; }

    /// <summary>
    /// The command manager that called the current command
    /// </summary>
    public CommandManager Manager { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="PilotCLI.Commands.Base.CommandContext" /> class.
    /// </summary>
    /// <param name="args">Command arguments</param>
    /// <param name="manager">Command manager that called the command</param>
    public CommandContext(string? args, CommandManager manager)
    {
        Args = args;
        Manager = manager;
    }
}