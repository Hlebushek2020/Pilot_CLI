using Ascon.Pilot.DataClasses;
using PilotCLI.Commands.Args;
using PilotCLI.Commands.Base;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class TypeCommand : ICommand
{
    private readonly PilotContext _pilotCtx;
    private readonly Dictionary<string, Action<MType>> _selectProcessing;

    public string Name { get; } = "type";
    public string Description { get; } = "Shows type information";

    public TypeCommand(PilotContext pilotCtx)
    {
        _pilotCtx = pilotCtx;
        _selectProcessing = new Dictionary<string, Action<MType>>
        {
            { "name", (type) => { Console.WriteLine($"Name: {type.Name}"); } },
            { "title", (type) => { Console.WriteLine($"Title: {type.Title}"); } }
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

        if ("--refresh".Equals(commandCtx.Args))
        {
            _pilotCtx.Repository.RefreshMetadata();
            return true;
        }

        TypeCommandArgs typeCommandArgs = TypeCommandArgs.Parse(
            commandCtx.Args,
            _selectProcessing.Keys.ToHashSet());

        if (typeCommandArgs.Types.Count == 0)
        {
            Console.WriteLine($"Incorrect command \"{Name}\"");
            return false;
        }

        if (typeCommandArgs.Select.Count == 0)
            typeCommandArgs.Select.Add("name");

        bool isFirstPrint = false;
        foreach (int idType in typeCommandArgs.Types)
        {
            if (_pilotCtx.Repository.Types.TryGetValue(idType, out MType mType))
            {
                if (isFirstPrint)
                    Console.WriteLine(new string('=', 30));

                Console.WriteLine($"$Id: {idType}");
                foreach (string select in typeCommandArgs.Select)
                    _selectProcessing[select].Invoke(mType);

                isFirstPrint = true;
            }
        }

        return true;
    }

    public void Help()
    {
        Console.ForegroundColor = CommandConstants.CommandColor;
        Console.WriteLine($"{Name} --refresh");
        Console.ResetColor();
        Console.WriteLine("Updates all metadata (types and user states)");
        Console.ForegroundColor = CommandConstants.CommandColor;
        Console.WriteLine($"{Name} <guid> select [ {string.Join(" | ", _selectProcessing.Keys)} ]");
        Console.ResetColor();
        Console.WriteLine(Description);
    }
}