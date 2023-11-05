using System.Text;

namespace PilotCLI.Commands.Base;

public class OutToFile : TextWriter
{
    private readonly StreamWriter _streamWriter;
    private readonly TextWriter _console;

    public override Encoding Encoding
    {
        get { return Encoding.UTF8; }
    }

    public OutToFile(TextWriter console, string path, bool append)
    {
        _console = console;
        _streamWriter = new StreamWriter(path, append, Encoding);
    }

    public override void Write(char value)
    {
        _console.Write(value);
        _streamWriter.Write(value);
    }

    public void CloseFile()
    {
        Console.SetOut(_console);
        _streamWriter.Flush();
        _streamWriter.Close();
    }
}