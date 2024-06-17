using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommandLine;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Plugins;

//Sort of copied from Mutagen/Spriggit

namespace ESPLinter
{
    [Verb("lint", HelpText = "Parses given plugin file for bad juju")]
    public class LintCommand
    {
        [Option('i', "InputPath", HelpText = "Path to the Bethesda plugin file", Required = true)]
        public string InputPath { get; set; } = string.Empty;

        [Option('o', "OutputPath", HelpText = "Path to export the linted plugin. Leave blank to overwrite input", Required = false)]
        public string OutputPath { get; set; } = string.Empty;

        [Option('r', "GameRelease", HelpText = "The game release to target. Required.", Required = true)]
        public GameRelease Release { get; set; }

        [Option('v', "Verbose", HelpText = "Display each processed record. Off by default", Required = false)]
        public bool Verbose { get; set; } = false;

        [Option("DryRun", HelpText = "Setting to true prevents this program from modifying the plugin. false by default", Required = false)]
        public bool DryRun { get; set; } = false;

        [Option('p', "Pedantic", HelpText = "Fail on warnings. false by default", Required = false)]
        public bool Pedantic { get; set; } = false;
    }
}
