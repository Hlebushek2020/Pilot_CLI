﻿using PilotCLI.Commands.Base;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class UserStateCommand : ICommand
{
    private readonly PilotContext _pilotCtx;

    public string Name { get; } = "user-state";
    public string Description { get; }

    public UserStateCommand(PilotContext pilotCtx) { _pilotCtx = pilotCtx; }

    public bool Execute(CommandContext commandCtx) { throw new NotImplementedException(); }
    public void Help() { throw new NotImplementedException(); }
}