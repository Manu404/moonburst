using CommandLine;
using MoonBurst.Api.Client;

namespace MoonBurst
{
    public class CommandLineStartOptionParser : IStartupOptionParser
    {
        private readonly string[] _args;
        private StartupOptions options = new StartupOptions();
        private bool isParsed = false;

        public class Options
        {
            [Option('l', "layout", Required = false, HelpText = "Layout to load")]
            public string Layout { get; set; }

            [Option('c', "config", Required = false, HelpText = "Configuration to use")]
            public string Config { get; set; }

            [Option('a', "autoconnect", Required = false, HelpText = "Connect automatically to last com and midi devices used with the current config.")]
            public bool Autoconnect { get; set; }

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }
        }

        public CommandLineStartOptionParser(string[] args)
        {
            this._args = args;
        }

        private void Parse()
        {
            Parser.Default.ParseArguments<Options>(_args)
                .WithParsed<Options>(o =>
                {
                    if (!string.IsNullOrEmpty(o.Config))
                    {
                        options.Config = o.Config;
                    }
                    if (!string.IsNullOrEmpty(o.Layout))
                    {
                        options.Layout = o.Layout;
                    }
                    options.Autoconnect = o.Autoconnect;
                });
            isParsed = true;
        }

        public StartupOptions Get()
        {
            if (!isParsed)
                Parse();
            return options;
        }
    }
}
