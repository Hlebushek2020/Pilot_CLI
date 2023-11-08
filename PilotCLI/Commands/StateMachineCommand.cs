using Ascon.Pilot.DataClasses;
using PilotCLI.Commands.Args;
using PilotCLI.Commands.Base;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class StateMachineCommand : ICommand
{
    private readonly ISettings _settings;
    private readonly PilotContext _pilotCtx;
    private readonly Dictionary<string, Action<MUserStateMachine>> _selectProcessing;

    public string Name { get; } = "state-machine";
    public string Description { get; } = "Shows information on machine states";

    public StateMachineCommand(ISettings settings, PilotContext pilotCtx)
    {
        _settings = settings;
        _pilotCtx = pilotCtx;

        _selectProcessing = new Dictionary<string, Action<MUserStateMachine>>
        {
            { "title", (mState) => { Console.WriteLine($"Title: {mState.Title}"); } },
            {
                "transitions", (mState) =>
                {
                    ConsoleTable consoleTable = new ConsoleTable("transitions");
                    consoleTable.AddColumn("State Id");
                    consoleTable.AddColumn("State");
                    consoleTable.AddColumn("Transition");
                    consoleTable.AddColumn("To State Id");
                    consoleTable.AddColumn("To State");
                    foreach (KeyValuePair<Guid, MTransition[]> stateTransition in mState.StateTransitions)
                    {
                        if (stateTransition.Value == null)
                            continue;

                        foreach (MTransition transition in stateTransition.Value)
                        {
                            ConsoleTable.Row row = consoleTable.AddRow();
                            row.SetAnyValue(0, stateTransition.Key);
                            Console.WriteLine(stateTransition.Key);

                            string stateName = "-";
                            if (_pilotCtx.Repository.UserStates.TryGetValue(stateTransition.Key,
                                    out MUserState? userState))
                            {
                                stateName = userState.Title;
                            }

                            row.SetAnyValue(1, stateName);
                            Console.WriteLine(stateName);

                            row.SetAnyValue(2, transition.DisplayName);
                            Console.WriteLine(transition.DisplayName);

                            row.SetAnyValue(3, transition.StateTo);
                            Console.WriteLine(transition.StateTo);

                            stateName = "-";
                            if (_pilotCtx.Repository.UserStates.TryGetValue(transition.StateTo, out userState))
                                stateName = userState.Title;

                            row.SetAnyValue(4, stateName);
                            Console.WriteLine(stateName);

                            Console.WriteLine(new string('=', 30));
                        }
                    }
                    consoleTable.Print();
                }
            }
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
            objectCommandArgs.Select.Add("title");

        bool printObjSep = false;
        foreach (Guid usGuid in objectCommandArgs.Objects)
        {
            if (_pilotCtx.Repository.StateMachines.TryGetValue(usGuid, out MUserStateMachine? userState))
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

    public void Help()
    {
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.WriteLine($"{Name} [ GUID ... ] select [ {string.Join(" ", _selectProcessing.Keys)} ] {
            CommandManager.OutputToFile}");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine(Description);
    }
}