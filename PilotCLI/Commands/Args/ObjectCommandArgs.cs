namespace PilotCLI.Commands.Args;

public class ObjectCommandArgs
{
    public ICollection<Guid> Objects { get; }
    public ICollection<string> Select { get; }

    private ObjectCommandArgs(ICollection<Guid> objects, ICollection<string> select)
    {
        Objects = objects;
        Select = select;
    }

    public static ObjectCommandArgs Parse(string args, ISet<string> aviableSelect)
    {
        string[] parts = args.Split(' ');
        int index = 0;
        string part;

        List<Guid> guids = new List<Guid>();
        while (index < parts.Length && !"select".Equals((part = parts[index])))
        {
            guids.Add(Guid.Parse(part));
            index++;
        }

        index++;
        List<string> select = new List<string>();
        while (index < parts.Length)
        {
            part = parts[index];
            if (aviableSelect.Contains(part))
                select.Add(part);
        }

        return new ObjectCommandArgs(guids, select);
    }
}