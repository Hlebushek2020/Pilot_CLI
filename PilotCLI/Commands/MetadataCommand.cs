using Ascon.Pilot.DataClasses;
using PilotCLI.Commands.Args;
using PilotCLI.Commands.Base;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class MetadataCommand : ICommand
{
    private readonly PilotContext _pilotCtx;
    private readonly Dictionary<string, Action> _arguments;

    public string Name { get; } = "metadata";
    public string Description { get; } = "Updates or shows metadata (types, user states, machine states)";

    public MetadataCommand(PilotContext pilotCtx)
    {
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

        MetadataCommandArgs commandArgs = MetadataCommandArgs.Parse(commandCtx.Args);

        return true;
    }

    public void Help() { throw new NotImplementedException(); }
}