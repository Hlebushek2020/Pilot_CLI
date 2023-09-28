namespace PilotCLI.Commands.Base;

/// <summary>
/// Provides a mechanism for creating commands. All command-providing classes must implement this interface.
/// </summary>
public interface ICommand
{
    /// <summary>
    /// Command
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Command description
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Executes the current command with the given parameters
    /// </summary>
    /// <param name="commandCtx">Command parameters</param>
    /// <returns>true if the command is executed otherwise false</returns>
    bool Execute(CommandContext commandCtx);

    /// <summary>
    /// Shows help for the current command
    /// </summary>
    void Help();
}