using PilotCLI.Commands.Base;

namespace PilotCLI.Commands;

public class SettingsPathCommand : ICommand
{
    private readonly ISettings _settings;

    public string Name { get; } = "settings";
    public string Description { get; } = "Show path to current settings file";

    public SettingsPathCommand(ISettings settings) { _settings = settings; }

    public bool Execute(CommandContext commandCtx)
    {
        Console.WriteLine(Settings.SettingsPath);
        return true;
    }

    public void Help()
    {
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.WriteLine(Name);
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine(Description);
    }
}