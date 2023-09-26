using Ascon.Pilot.DataClasses;

namespace PilotCLI.Pilot;

/// <summary>
/// Supports sending callback notifications to the desired location
/// </summary>
public interface IPilotServerCallbackListener
{
    /// <summary>
    /// Notifies when search results are received
    /// </summary>
    /// <param name="result">Object <see cref="Ascon.Pilot.DataClasses.DSearchResult"/> describing the search results</param>
    void Notify(DSearchResult result);
}