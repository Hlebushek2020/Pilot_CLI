namespace PilotCLI.Commands;

public interface ICommandHandler
{
    void Execute(string? args);
}