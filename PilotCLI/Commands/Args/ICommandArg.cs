namespace PilotCLI.Commands.Args;

public interface ICommandArg
{
    bool AppendToFile { get; }
    string? FilePath { get; }
}