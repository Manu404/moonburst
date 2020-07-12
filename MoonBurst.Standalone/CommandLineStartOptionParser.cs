using CommandLine;
using MoonBurst.Api.Client;

namespace MoonBurst
{
    public class CommandLineStartOptionParser : IStartupOptionParser
    {
        private readonly string[] _args;

        public class Options
        {
            [Option('l', "layout", Required = false, HelpText = "Layout to load.")]
            public string Layout { get; set; }

            [Option('c', "config", Required = false, HelpText = "Config to use.")]
            public string Config { get; set; }

            [Option('v', "verbose", Required = false, HelpText = "Set output to verbose messages.")]
            public bool Verbose { get; set; }
        }

        public CommandLineStartOptionParser(string[] args)
        {
            this._args = args;
        }

        public StartupOptions Get()
        {
            var options = new StartupOptions();
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
                });
            return options;
        }
    }
}
