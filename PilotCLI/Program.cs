using System.Reflection;
using Ascon.Pilot.DataClasses;
using PilotCLI.Commands;
using PilotCLI.Pilot;

namespace PilotCLI
{
    internal class Program
    {
        public const string TitleTemplate = "Pilot CLI [ {0} ]";

        #region Fields
        private static readonly PilotContext _pilotCtx = new PilotContext();
        private static readonly CommandManager _commandManager = new CommandManager();
        #endregion

        public static string Directory { get; } =
            Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? string.Empty;

        static void Main(string[] args)
        {
            Console.Title = string.Format(TitleTemplate, "none");

            if (!Settings.Availability())
            {
                Console.WriteLine(
                    $"The configuration file (\"{Settings.SettingsPath
                    }\") has been created! Fill it out and restart the program!");
                Console.ReadLine();
                return;
            }

            ISettings settings = Settings.Load();
            RegisterCommands(settings);

            bool isNotExit = true;
            do
            {
                Console.ForegroundColor = settings.CommandColor;
                Console.Write("> ");
                string? commandLine = Console.ReadLine()?.Trim();
                Console.ForegroundColor = settings.OtherTextColor;
                if ("exit".Equals(commandLine?.ToLower()))
                {
                    isNotExit = false;
                }
                else
                {
                    try
                    {
                        _commandManager.Process(commandLine);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            } while (isNotExit);
        }

        private static void RegisterCommands(ISettings settings)
        {
            _commandManager.RegisterCommand(new HelpCommand(settings));
            _commandManager.RegisterCommand(new ObjectCommand(settings, _pilotCtx));
            _commandManager.RegisterCommand(new SetContextCommand(settings, _pilotCtx));
            _commandManager.RegisterCommand(new SettingsPathCommand(settings));
            //_commandManager.RegisterCommand(new StateMachineCommand(settings, _pilotCtx));
            _commandManager.RegisterCommand(new TypeCommand(settings, _pilotCtx));
            _commandManager.RegisterCommand(new MetadataCommand(settings, _pilotCtx));
            _commandManager.RegisterCommand(new UserStateCommand(settings, _pilotCtx));
        }
    }
}