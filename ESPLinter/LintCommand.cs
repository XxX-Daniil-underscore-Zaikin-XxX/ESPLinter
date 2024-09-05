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

        [Option('r', "GameRelease", HelpText = "The game release to target. Required.", Required = false)]
        public GameRelease Release { get; set; }

        public LintCommand(string inputFile, string outputFile, GameRelease? release)
        {
            this.OutputPath = outputFile;
            this.InputPath = inputFile;
            if (release != null)
            {
                this.Release = GameRelease.SkyrimSE;
            }
        }

        public LintCommand()
        {
            this.InputPath = string.Empty;
            this.OutputPath= string.Empty;
            this.Release = GameRelease.SkyrimSE;
        }
    }
}
