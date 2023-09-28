using Newtonsoft.Json;

namespace PilotCLI;

/// <summary>
/// Provides settings for the program
/// </summary>
public interface ISettings
{
    /// <summary>
    /// <see cref="System.Collections.Generic.IReadOnlyDictionary{TKey, TValue}"/> of all available connections. Where
    /// the TKey is the unique name of the connection, the TValue is an object <see cref="PilotCLI.ContextEntry"/>
    /// describing the parameters of this connection.
    /// </summary>
    IReadOnlyDictionary<string, ContextEntry> Contexts { get; }

    /// <summary>
    /// Command signature output color
    /// </summary>
    ConsoleColor CommandSignatureColor { get; }

    /// <summary>
    /// Command input color
    /// </summary>
    ConsoleColor CommandColor { get; }

    /// <summary>
    /// Output color of the rest of the text
    /// </summary>
    ConsoleColor OtherTextColor { get; }
}