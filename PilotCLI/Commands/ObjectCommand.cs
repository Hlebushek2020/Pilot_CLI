﻿using Ascon.Pilot.DataClasses;
using PilotCLI.Commands.Args;
using PilotCLI.Commands.Base;
using PilotCLI.Pilot;

namespace PilotCLI.Commands;

public class ObjectCommand : ICommand
{
    private readonly PilotContext _pilotCtx;
    private readonly Dictionary<string, Action<DObject>> _selectProcessing;

    public string Name { get; } = "object";
    public string Description { get; } = "Displays certain information about the specified object";

    public ObjectCommand(PilotContext pilotCtx)
    {
        _pilotCtx = pilotCtx;
        _selectProcessing = new Dictionary<string, Action<DObject>>
        {
            { "type", (@object) => { Console.WriteLine($"Type: {@object.TypeId}"); } },
            { "parent", (@object) => { Console.WriteLine($"Parent: {@object.ParentId}"); } },
            {
                "children", (@object) =>
                {
                    Console.WriteLine("Type\tObject");
                    foreach (DChild child in @object.Children)
                        Console.WriteLine($"{child.TypeId}\t{child.ObjectId}");
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
            foreach (string select in objectCommandArgs.Select)
                _selectProcessing[select].Invoke(dObject);
            if (numObj < objects.Count - 1)
                Console.WriteLine(new string('=', 30));
        }

        return true;
    }

    public void Help()
    {
        Console.ForegroundColor = CommandConstants.CommandColor;
        Console.WriteLine($"{Name} <guid> select [ {string.Join(" | ", _selectProcessing.Keys)} ]");
        Console.ResetColor();
        Console.WriteLine(Description);
    }
}