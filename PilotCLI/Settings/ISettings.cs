using Newtonsoft.Json;

namespace PilotCLI;

public interface ISettings
{
    IReadOnlyDictionary<string, ContextEntry> Contexts { get; }
}