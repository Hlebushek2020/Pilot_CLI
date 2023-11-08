namespace PilotCLI.Commands.Base;

public class ConsoleTable
{
    private readonly List<Column> _columns = new List<Column>();
    private readonly List<Row> _rows = new List<Row>();
    private readonly string _tableTitle;

    private int _fullLenght = 0;

    public ConsoleTable(string tableTitle) { _tableTitle = tableTitle; }

    public void AddColumn(string title)
    {
        Column column = new Column(title);
        _fullLenght += column.MaxLenght;
        _columns.Add(column);
    }

    public Row AddRow()
    {
        Row row = new Row(this);
        _rows.Add(row);
        return row;
    }

    public void Print()
    {
        if (_columns.Count > 0)
        {
            int lenghtSep = 3 * (_columns.Count - 1) + _fullLenght + 2;
            int titlePartCount = (lenghtSep - _tableTitle.Length) / 2;
            Console.Write(new string('-', titlePartCount));
            Console.Write($" {_tableTitle} ");
            Console.WriteLine(new string('-', lenghtSep - titlePartCount - _tableTitle.Length));
            lenghtSep += 2;
            for (int numColumn = 0; numColumn < _columns.Count; numColumn++)
            {
                if (numColumn == 0)
                    Console.Write("| ");

                Column column = _columns[numColumn];

                Console.Write(column.Title);
                Console.Write(new string(' ', column.MaxLenght - column.Title.Length + 1));
                Console.Write("| ");

                if (numColumn == _columns.Count - 1)
                    Console.WriteLine();
            }
            Console.WriteLine(new string('-', lenghtSep));
            foreach (Row row in _rows)
            {
                for (int numColumn = 0; numColumn < _columns.Count; numColumn++)
                {
                    if (numColumn == 0)
                        Console.Write("| ");

                    string cellValue = row[numColumn];
                    Console.Write(cellValue);
                    Console.Write(new string(' ', _columns[numColumn].MaxLenght - cellValue.Length + 1));
                    Console.Write("| ");

                    if (numColumn == _columns.Count - 1)
                        Console.WriteLine();
                }
            }
            Console.WriteLine(new string('-', lenghtSep));
        }
    }

    private class Column
    {
        public string Title { get; }
        public int MaxLenght { get; set; }

        public Column(string title)
        {
            Title = title;
            MaxLenght = title.Length;
        }
    }

    public class Row
    {
        private readonly ConsoleTable _consoleTable;
        private readonly string[] _cells;

        public string this[int index]
        {
            get => _cells[index];
            set
            {
                _cells[index] = value;
                Column column = _consoleTable._columns[index];

                if (value.Length > column.MaxLenght)
                {
                    _consoleTable._fullLenght += value.Length - column.MaxLenght;
                    column.MaxLenght = value.Length;
                }
            }
        }

        public void SetAnyValue(int index, object? value) => this[index] = value?.ToString() ?? string.Empty;

        internal Row(ConsoleTable consoleTable)
        {
            _consoleTable = consoleTable;
            _cells = new string[consoleTable._columns.Count];
        }
    }
}