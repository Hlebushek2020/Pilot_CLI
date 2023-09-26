using System.Reflection;
using Ascon.Pilot.DataClasses;
using PilotCLI.Commands;
using PilotCLI.Pilot;

namespace PilotCLI
{
    internal class Program
    {
        #region Fields
        private static readonly PilotContext _pilotCtx = new PilotContext();
        private static readonly CommandManager _commandManager = new CommandManager();
        #endregion

        public static string Directory { get; } =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        static void Main(string[] args)
        {
            if (!Settings.Availability())
            {
                ConsoleUtils.WriteLineWarning(
                    "The configuration file has been created! Fill it out and restart the program!");
                Console.ReadLine();
                return;
            }

            RegisterCommands(Settings.Load());

            bool isNotExit = true;
            do
            {
                string? commandLine = Console.ReadLine()?.Trim();
                if ("exit".Equals(commandLine?.ToLower()))
                    isNotExit = false;
                else
                    _commandManager.Process(commandLine);
            } while (isNotExit);
        }

        private static void RegisterCommands(ISettings settings)
        {
            _commandManager.RegisterCommand("set-context", new PilotChangeContextCommand(_pilotCtx, settings));
        }
    }
}