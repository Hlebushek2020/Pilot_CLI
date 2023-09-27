namespace PilotCLI.Commands.Args;

public class TypeCommandArgs
{
    public IList<int> Types { get; }
    public IList<string> Select { get; }

    private TypeCommandArgs(IList<int> types, IList<string> select)
    {
        Types = types;
        Select = select;
    }

    public static TypeCommandArgs Parse(string args, ISet<string> aviableSelect)
    {
        string[] parts = args.Split(' ');
        int index = 0;
        string part;

        List<int> guids = new List<int>();
        while (index < parts.Length && !"select".Equals((part = parts[index])))
        {
            if (int.TryParse(part, out int intValue))
                guids.Add(intValue);

            index++;
        }

        index++;
        List<string> select = new List<string>();
        while (index < parts.Length)
        {
            part = parts[index];
            if (aviableSelect.Contains(part))
                select.Add(part);

            index++;
        }

        return new TypeCommandArgs(guids, select);
    }
}