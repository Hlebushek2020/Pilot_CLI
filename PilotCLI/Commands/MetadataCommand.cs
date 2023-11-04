using Ascon.Pilot.DataClasses;
using PilotCLI.Commands.Args;
using PilotCLI.Commands.Base;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class MetadataCommand : ICommand
{
    private readonly PilotContext _pilotCtx;
    private ISettings _settings;
    private readonly Dictionary<string, Action> _arguments;

    public string Name { get; } = "metadata";
    public string Description { get; } = "Updates or shows metadata (types, user states, machine states)";

    public MetadataCommand(ISettings settings, PilotContext pilotCtx)
    {
        _settings = settings;
        _pilotCtx = pilotCtx;

        _arguments = new Dictionary<string, Action>
        {
            {
                "--refresh", () =>
                {
                    _pilotCtx.Repository.RefreshMetadata();
                    Console.WriteLine("Metadata refreshed!");
                }
            },
            {
                "--types", () =>
                {
                    ConsoleTable consoleTable = new ConsoleTable("types");
                    consoleTable.AddColumn("Id");
                    consoleTable.AddColumn("Name");
                    consoleTable.AddColumn("Is Deleted");
                    consoleTable.AddColumn("Is Mountable");
                    consoleTable.AddColumn("Is Project");
                    consoleTable.AddColumn("Is Service");
                    foreach (MType mType in _pilotCtx.Repository.Types.Values)
                    {
                        ConsoleTable.Row row = consoleTable.AddRow();
                        row.SetAnyValue(0, mType.Id);
                        row.SetAnyValue(1, mType.Name);
                        row.SetAnyValue(2, mType.IsDeleted);
                        row.SetAnyValue(3, mType.IsMountable);
                        row.SetAnyValue(4, mType.IsProject);
                        row.SetAnyValue(5, mType.IsService);
                    }
                    consoleTable.Print();
                }
            },
            {
                "--ustates", () =>
                {
                    ConsoleTable consoleTable = new ConsoleTable("user states");
                    consoleTable.AddColumn("Id");
                    consoleTable.AddColumn("Name");
                    consoleTable.AddColumn("Is Deleted");
                    consoleTable.AddColumn("Is Completion State");
                    consoleTable.AddColumn("Is System State");
                    foreach (MUserState userState in _pilotCtx.Repository.UserStates.Values)
                    {
                        ConsoleTable.Row row = consoleTable.AddRow();
                        row.SetAnyValue(0, userState.Id);
                        row.SetAnyValue(1, userState.Name);
                        row.SetAnyValue(2, userState.IsDeleted);
                        row.SetAnyValue(3, userState.IsCompletionState);
                        row.SetAnyValue(4, userState.IsSystemState);
                    }
                    consoleTable.Print();
                }
            },
            {
                "--mstates", () =>
                {
                    ConsoleTable consoleTable = new ConsoleTable("machine states");
                    consoleTable.AddColumn("Id");
                    consoleTable.AddColumn("Title");
                    foreach (MUserStateMachine stateMachine in _pilotCtx.Repository.StateMachines.Values)
                    {
                        ConsoleTable.Row row = consoleTable.AddRow();
                        row.SetAnyValue(0, stateMachine.Id);
                        row.SetAnyValue(1, stateMachine.Title);
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

        MetadataCommandArgs commandArgs = MetadataCommandArgs.Parse(commandCtx.Args, _arguments.Keys.ToHashSet());

        if (commandArgs.Args.Count == 0)
        {
            Console.WriteLine($"Incorrect command \"{Name}\"");
            return false;
        }

        foreach (string arg in commandArgs.Args)
            _arguments[arg]();

        return true;
    }

    public void Help()
    {
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.WriteLine($"{Name} [ <ARG> ... ]");
        Console.WriteLine("   ARG is:");
        Console.Write("      --refresh");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine(" - updates metadata");
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.Write("      --types");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine(" - show information for all types");
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.Write("      --ustates");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine(" - show information for all user states");
        Console.ForegroundColor = _settings.CommandSignatureColor;
        Console.Write("      --mstates");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine(" - show information for all machine states");
    }
}