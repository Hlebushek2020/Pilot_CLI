using Newtonsoft.Json;

namespace PilotCLI;

public interface ISettings
{
    IReadOnlyDictionary<string, ContextEntry> Contexts { get; }
    ConsoleColor CommandSignatureColor { get; }
    ConsoleColor CommandColor { get; }
    ConsoleColor OtherTextColor { get; }
}