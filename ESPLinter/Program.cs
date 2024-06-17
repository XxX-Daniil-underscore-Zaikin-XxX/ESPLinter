// See https://aka.ms/new-console-template for more information
using CommandLine;
using ESPLinter;
using Serilog;
using Mutagen.Bethesda;
using Mutagen.Bethesda.Environments;
using Mutagen.Bethesda.Plugins;
using Mutagen.Bethesda.Plugins.Aspects;
using Mutagen.Bethesda.Plugins.Cache;
using Mutagen.Bethesda.Plugins.Records;
using Mutagen.Bethesda.Plugins.Records.Internals;
using Mutagen.Bethesda.Skyrim;
using Mutagen.Bethesda.Binary;
using Mutagen.Bethesda.Plugins.Binary.Parameters;
using CommandLine.Text;

class Program
{
    static class LoggerSetup
    {
        public static ILogger Logger { get; }

        static LoggerSetup()
        {
            Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate:
                    "[{Timestamp:HH:mm:ss.fff} {Level:u3}] {Message:lj}{NewLine}{Exception}")
                .CreateLogger();
        }
    }

    static void Main(string[] args)
    {
        CommandLine.Parser.Default.ParseArguments<LintCommand>(args)
            .WithParsed(RunLint)
            .WithNotParsed(HandleParseError);
    }


    static void RunLint(LintCommand command)
    {
        var mod = SkyrimMod.CreateFromBinary(
            ModPath.FromPath(command.InputPath),
            command.Release.ToSkyrimRelease());
        
        ILinkCache scriptLinkCache = mod.ToMutableLinkCache();

        foreach (var scripted in mod.EnumerateMajorRecords<IScripted>())
        {
            var scripts = scripted.VirtualMachineAdapter?.Scripts;
            if (scripts == null || !scripts.Any())
            {
                continue;
            }
            foreach (var script in scripts)
            {
                if (script == null || !script.Properties.Any())
                {
                    continue;
                }
                script.Properties.Sort((prop1, prop2) => string.Compare(prop1.Name.ToLower(), prop2.Name.ToLower()));
            }
        }

        /*foreach (var keyworded in mod.EnumerateMajorRecords<IKeyworded<IKeyword>>())
        {
            var keywords = keyworded.Keywords;
            if (keywords == null || !keywords.Any())
            {
                continue;
            }
            keywords.Sort();
        }*/
        var writeParams = new BinaryWriteParameters()
        {
            ModKey = ModKeyOption.CorrectToPath,
            

        };
        mod.WriteToBinary(
            command.OutputPath,
            writeParams);
    }

/*    static void LintMod<TMod>(GameRelease gameRelease, ModPath inputPath)
        where TMod : IModDisposeGetter
    {
        using TMod importedMod = ModInstantiator<TMod>.Importer(inputPath, gameRelease);
        //using var readOnlyInputMod = SkyrimMod.CreateFromBinaryOverlay(inputPath, SkyrimRelease.SkyrimSE);
        foreach (var gamer in importedMod.EnumerateMajorRecords<IScripted>)
    }
*/

    static void HandleParseError(IEnumerable<Error> errors)
    {
        /*var logger = LoggerSetup.Logger;
        foreach (var error in errors)
        {
            logger.Error(error);
            Console.Error.WriteLine(error);
        }*/
        var sentenceBuilder = SentenceBuilder.Create();
        var logger = LoggerSetup.Logger;
        foreach (var error in errors)
        {
            logger.Error(sentenceBuilder.FormatError(error));
        }
    }
}

class Linter
{
    private LintCommand _options;

    public Linter(LintCommand options)
    {
        _options = options;
    }

    public HashSet<RecordType> getApplicableRecordTypes()
    {
        return new HashSet<RecordType>();
    }

    public bool filterRecord(IMajorRecordGetter record)
    {
        return false;
    }

    public int checkProblem(IMajorRecordGetter record)
    {
        return 0;
    }

    public bool fixProblem(IMajorRecordGetter record)
    {
        return true;
    }
}