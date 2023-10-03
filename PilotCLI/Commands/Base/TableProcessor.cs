namespace PilotCLI.Commands.Base;

public class TableProcessor
{
    private readonly List<Column> _columns = new List<Column>();
    private readonly string _tableTitle;

    private int fullLenght = 0;

    public TableProcessor(string tableTitle) { _tableTitle = tableTitle; }

    public void AddColumn(string title)
    {
        Column column = new Column
        {
            Title = title,
            MaxLenght = title.Length,
            Values = new List<string>()
        };
        fullLenght += column.MaxLenght;
        _columns.Add(column);
    }

    public void AddValue(int columnIndex, string value)
    {
        Column column = _columns[columnIndex];

        if (value.Length > column.MaxLenght)
        {
            fullLenght += value.Length - column.MaxLenght;
            column.MaxLenght = value.Length;
        }

        column.Values.Add(value);
    }

    public void Print()
    {
        if (_columns.Count > 0)
        {
            int lenghtSep = 3 * (_columns.Count - 1) + fullLenght + 2;
            int titlePartCount = (lenghtSep - _tableTitle.Length) / 2;
            Console.Write(new string('-', titlePartCount));
            Console.Write($" {_tableTitle} ");
            Console.WriteLine(new string('-', lenghtSep - titlePartCount - _tableTitle.Length));
            lenghtSep += 2;
            Column column = new Column();
            for (int numColumn = 0; numColumn < _columns.Count; numColumn++)
            {
                if (numColumn == 0)
                    Console.Write("| ");

                column = _columns[numColumn];

                Console.Write(column.Title);
                Console.Write(new string(' ', column.MaxLenght - column.Title.Length + 1));
                Console.Write("| ");

                if (numColumn == _columns.Count - 1)
                    Console.WriteLine();
            }
            Console.WriteLine(new string('-', lenghtSep));
            int index = 0;
            while (index < column.Values.Count)
            {
                for (int numColumn = 0; numColumn < _columns.Count; numColumn++)
                {
                    if (numColumn == 0)
                        Console.Write("| ");

                    column = _columns[numColumn];

                    string colValue = column.Values[index];

                    Console.Write(colValue);
                    Console.Write(new string(' ', column.MaxLenght - colValue.Length + 1));
                    Console.Write("| ");

                    if (numColumn == _columns.Count - 1)
                        Console.WriteLine();
                }
                index++;
            }
            Console.WriteLine(new string('-', lenghtSep));
        }
    }

    private class Column
    {
        public List<string> Values { get; set; }
        public string Title { get; set; }
        public int MaxLenght { get; set; }
    }
}