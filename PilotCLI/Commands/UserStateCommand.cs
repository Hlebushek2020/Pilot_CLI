using Ascon.Pilot.DataClasses;
using PilotCLI.Commands.Args;
using PilotCLI.Commands.Base;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class UserStateCommand : ICommand
{
    private readonly ISettings _settings;
    private readonly PilotContext _pilotCtx;
    private readonly Dictionary<string, Action<MUserState>> _selectProcessing;

    public string Name { get; } = "user-state";
    public string Description { get; } = "Shows information on user states";

    public UserStateCommand(ISettings settings, PilotContext pilotCtx)
    {
        _settings = settings;
        _pilotCtx = pilotCtx;
        _selectProcessing = new Dictionary<string, Action<MUserState>>
        {
            { "name", (userState) => { } },
            { "title", (userState) => { } },
            { "flags", (userState) => { } }
        };
    }

    public bool Execute(CommandContext commandCtx)
    {
        if (string.IsNullOrWhiteSpace(commandCtx.Args))
        {
            Console.WriteLine($"Incorrect command \"{Name}\"");
            return false;
        }

        if (!_pilotCtx.IsInstalled)
        {
            Console.WriteLine($"Command \"{Name}\" cannot be executed because the context is not set");
            return false;
        }

        ObjectCommandArgs objectCommandArgs = ObjectCommandArgs.Parse(
            commandCtx.Args,
            _selectProcessing.Keys.ToHashSet());

        if (objectCommandArgs.Objects.Count == 0)
        {
            Console.WriteLine($"Incorrect command \"{Name}\"");
            return false;
        }

        if (objectCommandArgs.Select.Count == 0)
            objectCommandArgs.Select.Add("name");

        bool printObjSep = false;
        foreach (Guid usGuid in objectCommandArgs.Objects)
        {
            if (_pilotCtx.Repository.UserStates.TryGetValue(usGuid, out MUserState? userState))
            {
                if (printObjSep)
                    Console.WriteLine(new string(CommandConstants.ObjectSeparator, CommandConstants.SeparatorLength));
                Console.WriteLine($"User state: {usGuid}");
                foreach (string select in objectCommandArgs.Select)
                    _selectProcessing[select].Invoke(userState);
                printObjSep = true;
            }
        }

        return true;
    }

    public void Help() { throw new NotImplementedException(); }
}