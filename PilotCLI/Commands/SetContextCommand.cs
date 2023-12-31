﻿using PilotCLI;
using PilotCLI.Commands;
using PilotCLI.Commands.Base;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class SetContextCommand : ICommand
{
    #region Fields
    private readonly ISettings _settings;
    private readonly PilotContext _pilotCtx;
    #endregion

    public string Name { get; } = "set-context";
    public string Description { get; } = "Change pilot configuration";

    public SetContextCommand(ISettings settings, PilotContext pilotCtx)
    {
        _settings = settings;
        _pilotCtx = pilotCtx;
    }

    public bool Execute(CommandContext commandCtx)
    {
        if (string.IsNullOrWhiteSpace(commandCtx.Args))
        {
            Console.WriteLine(string.Join(", ", _settings.Contexts.Keys));
            return true;
        }

        if ("--current".Equals(commandCtx.Args))
        {
            if (_pilotCtx.IsInstalled)
            {
                Console.WriteLine($"Server Url: {_pilotCtx.ServerUrl}");
                Console.WriteLine($"Database: {_pilotCtx.Database}");
                Console.WriteLine($"Username: {_pilotCtx.Username}");
                Console.WriteLine($"LicenseCode: {_pilotCtx.LicenseCode}");
            }
            else
            {
                Console.WriteLine("NONE");
            }
            return true;
        }

        if (!_settings.Contexts.TryGetValue(commandCtx.Args, out ContextEntry? contextEntry))
        {
            Console.WriteLine($"Context named {commandCtx.Args} not found!");
            return false;
        }

        _pilotCtx.ChangeContext(contextEntry);

        Console.Title = string.Format(
            Program.TitleTemplate,
            $"{_pilotCtx.ServerUrl}; {_pilotCtx.Database}; {_pilotCtx.Username}");

        return true;
    }

    public void Help()
    {
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.WriteLine($"{Name} {CommandManager.OutputToFile}");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine("Show a list of available contexts");
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.WriteLine($"{Name} --current {CommandManager.OutputToFile}");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine("Show the currently set context");
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.WriteLine($"{Name} CONTEXT {CommandManager.OutputToFile}");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine("Sets the specified context");
    }
}