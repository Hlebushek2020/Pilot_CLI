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
            { "type", (@object) => { Console.WriteLine($"Type: {@object.TypeId}"); } },
            { "parent", (@object) => { Console.WriteLine($"Parent: {@object.ParentId}"); } },
            {
                "children", (@object) =>
                {
                    Console.WriteLine(new string(CommandConstants.TableSeparator, CommandConstants.SeparatorLength));
                    Console.WriteLine("Type\tObject");
                    foreach (DChild child in @object.Children)
                        Console.WriteLine($"{child.TypeId}\t{child.ObjectId}");
                    Console.WriteLine(new string(CommandConstants.TableSeparator, CommandConstants.SeparatorLength));
                }
            },
            {
                "relations", (@object) =>
                {
                    Console.WriteLine(new string(CommandConstants.TableSeparator, CommandConstants.SeparatorLength));
                    Console.WriteLine("Id\tTarget\tType");
                    foreach (DRelation relation in @object.Relations)
                        Console.WriteLine($"{relation.Id}\t{relation.TargetId}\t{relation.Type}");
                    Console.WriteLine(new string(CommandConstants.TableSeparator, CommandConstants.SeparatorLength));
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

        ObjectCommandArgs objectCommandArgs = ObjectCommandArgs.Parse(
            commandCtx.Args,
            _selectProcessing.Keys.ToHashSet());

        if (objectCommandArgs.Objects.Count == 0)
        {
            Console.WriteLine($"Incorrect command \"{Name}\"");
            return false;
        }

        if (!_pilotCtx.IsInstalled)
        {
            Console.WriteLine($"Command \"{Name}\" cannot be executed because the context is not set");
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
        Console.WriteLine($"{Name} <guid> select [ {string.Join(" | ", _selectProcessing.Keys)} ]");
        Console.ForegroundColor = _settings.OtherTextColor;
        Console.WriteLine(Description);
    }
}