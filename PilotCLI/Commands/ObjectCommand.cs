using Ascon.Pilot.DataClasses;
using PilotCLI.Commands.Args;
using PilotCLI.Commands.Base;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class ObjectCommand : ICommand
{
    private readonly ISettings _settings;
    private readonly PilotContext _pilotCtx;
    private readonly Dictionary<string, Action<DObject>> _selectProcessing;

    public string Name { get; } = "object";
    public string Description { get; } = "Displays certain information about the specified object";

    public ObjectCommand(ISettings settings, PilotContext pilotCtx)
    {
        _settings = settings;
        _pilotCtx = pilotCtx;
        _selectProcessing = new Dictionary<string, Action<DObject>>
        {
            {
                "type", (@object) =>
                {
                    string typeName = "-";
                    if (_pilotCtx.Repository.Types.TryGetValue(@object.TypeId, out MType? type))
                        typeName = type.Name;
                    Console.WriteLine($"Type: {typeName} (Id: {@object.TypeId})");
                }
            },
            { "parent", (@object) => { Console.WriteLine($"Parent: {@object.ParentId}"); } },
            {
                "children", (@object) =>
                {
                    ConsoleTable consoleTable = new ConsoleTable("children");
                    consoleTable.AddColumn("Type");
                    consoleTable.AddColumn("Type Id");
                    consoleTable.AddColumn("Object");
                    foreach (DChild child in @object.Children)
                    {
                        ConsoleTable.Row row = consoleTable.AddRow();

                        string typeName = "-";
                        if (_pilotCtx.Repository.Types.TryGetValue(child.TypeId, out MType? type))
                            typeName = type.Name;

                        row.SetAnyValue(0, typeName);
                        row.SetAnyValue(1, child.TypeId);
                        row.SetAnyValue(2, child.ObjectId);
                    }
                    consoleTable.Print();
                }
            },
            {
                "relations", (@object) =>
                {
                    ConsoleTable consoleTable = new ConsoleTable("relations");
                    consoleTable.AddColumn("Id");
                    consoleTable.AddColumn("Target");
                    consoleTable.AddColumn("Type");
                    foreach (DRelation relation in @object.Relations)
                    {
                        ConsoleTable.Row row = consoleTable.AddRow();
                        row.SetAnyValue(0, relation.Id);
                        row.SetAnyValue(1, relation.TargetId);
                        row.SetAnyValue(2, relation.Type);
                    }
                    consoleTable.Print();
                }
            },
            {
                "attributes", (@object) =>
                {
                    foreach (KeyValuePair<string, DValue> attribute in @object.Attributes)
                    {
                        object value = attribute.Value.Value;
                        if (attribute.Value.ArrayValue != null)
                        {
                            value = string.Join("; ", attribute.Value.ArrayValue);
                        }
                        else if (attribute.Value.ArrayIntValue != null)
                        {
                            value = string.Join("; ", attribute.Value.ArrayIntValue.Select(iv => iv.ToString()));
                        }
                        Console.WriteLine($"{attribute.Key}: {value}");
                    }
                }
            },
            {
                "context", (@object) =>
                {
                    IReadOnlyDictionary<Guid, DObject> objectById = _pilotCtx.Repository
                        .GetObjects(@object.Context)
                        .ToDictionary(ks => ks.Id);

                    ConsoleTable consoleTable = new ConsoleTable("context");
                    consoleTable.AddColumn("Id");
                    consoleTable.AddColumn("Type");
                    foreach (Guid guid in @object.Context)
                    {
                        ConsoleTable.Row row = consoleTable.AddRow();
                        row.SetAnyValue(0, guid);

                        string typeName = "-";
                        if (objectById.TryGetValue(guid, out DObject? ctxObj) &&
                            _pilotCtx.Repository.Types.TryGetValue(ctxObj.TypeId, out MType? ctxObjType))
                        {
                            typeName = ctxObjType.Name;
                        }
                        row.SetAnyValue(1, typeName);
                    }
                    consoleTable.Print();
                }
            },
            {
                "files", (@object) =>
                {
                    IReadOnlyDictionary<Guid, DObject> objectById = _pilotCtx.Repository
                        .GetObjects(@object.Context)
                        .ToDictionary(ks => ks.Id);

                    ConsoleTable consoleTable = new ConsoleTable("files");
                    consoleTable.AddColumn("Name");
                    consoleTable.AddColumn("Id");
                    consoleTable.AddColumn("Created");
                    consoleTable.AddColumn("Modified");
                    consoleTable.AddColumn("Size");
                    foreach (DFile file in @object.ActualFileSnapshot.Files)
                    {
                        ConsoleTable.Row row = consoleTable.AddRow();
                        row.SetAnyValue(0, file.Name);
                        row.SetAnyValue(1, "-");
                        row.SetAnyValue(2, "-");
                        row.SetAnyValue(3, "-");
                        row.SetAnyValue(4, "-");

                        if (file.Body != null)
                        {
                            row.SetAnyValue(1, file.Body.Id);
                            row.SetAnyValue(2, file.Body.Created);
                            row.SetAnyValue(3, file.Body.Modified);

                            double size = double.Round(file.Body.Size / 1024.0, 2);

                            row.SetAnyValue(4, $"{size} Kb");
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
        {
            objectCommandArgs.Select.Add("type");
            objectCommandArgs.Select.Add("children");
        }

        IReadOnlyList<DObject> objects = _pilotCtx.Repository.GetObjects(objectCommandArgs.Objects);
        for (int numObj = 0; numObj < objects.Count; numObj++)
        {
            DObject dObject = objects[numObj];
            Console.WriteLine($"Object: {dObject.Id}");
            foreach (string select in objectCommandArgs.Select)
                _selectProcessing[select].Invoke(dObject);
            if (numObj < objects.Count - 1)
                Console.WriteLine(new string(CommandConstants.ObjectSeparator, CommandConstants.SeparatorLength));
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