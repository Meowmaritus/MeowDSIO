using MeowDSIO;
using MeowDSIO.DataFiles;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeowTAE
{
    public class MeowTAE
    {
        enum ACTION
        {
            HELP,
            TOJSON,
            TOTAE
        }

        enum ConversionType
        {
            TaeToJson,
            JsonToTae
        }

        static Dictionary<ConversionType, string> ConversionDefaultOutputFileExtension = new Dictionary<ConversionType, string>
        {
            [ConversionType.JsonToTae] = ".tae",
            [ConversionType.TaeToJson] = ".json",
        };

        static Dictionary<ACTION, string> ActionUsage = new Dictionary<ACTION, string>
        {
            [ACTION.HELP] = "MeowTAE Help <[ACTION]>",
            [ACTION.TOJSON] = "MeowTAE ToJSON <[input file]> {[output file]}",
            [ACTION.TOTAE] = "MeowTAE ToTAE <[input file]> {[output file]}",
        };

        static Dictionary<ACTION, string> ActionDescription = new Dictionary<ACTION, string>
        {
            [ACTION.HELP] = "Displays help on a specific ACTION.",

            [ACTION.TOJSON] = "Converts the input file from a TAE format file to a JSON format file. " +
                "If the output file name is not specified, then it is assumed to be the " +
                "same file name but with a .JSON extension.",

            [ACTION.TOTAE] = "Converts the input file from a JSON format file to a TAE format file. " +
                "If the output file name is not specified, then it is assumed to be the " +
                "same file name but with a .TAE extension.",
        };

        static void PrintUsage()
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            {
                Console.WriteLine("To convert with specific settings:");

                Console.ForegroundColor = ConsoleColor.Yellow;
                {
                    Console.WriteLine("    MeowTAE <[ACTION]> <[input file]> {[output file]}");
                }
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("    ");
                Console.WriteLine("Available ACTIONs:");
                foreach (var action in (ACTION[])Enum.GetValues(typeof(ACTION)))
                    Console.WriteLine("    " + action);
                Console.WriteLine("    ");
                Console.WriteLine("To display information about a specific ACTION:");

                Console.ForegroundColor = ConsoleColor.Yellow;
                {
                    Console.WriteLine("    MeowTAE HELP <ACTION>");
                }
                Console.ForegroundColor = ConsoleColor.Cyan;

                Console.WriteLine("    ");
                Console.WriteLine("To guess the input/output file type and convert:");

                Console.ForegroundColor = ConsoleColor.Yellow;
                {
                    Console.WriteLine("    MeowTAE <[input file]>");
                    Console.WriteLine("    ");
                    Console.WriteLine("    OR");
                    Console.WriteLine("    ");
                    Console.WriteLine("    *Drag and drop file onto MeowTAE.exe*");
                }
                Console.ForegroundColor = ConsoleColor.Cyan;

            }
            Console.ForegroundColor = ConsoleColor.White;
        }

        static string ChangeExtension(string fileName, string newExtension)
        {
            return fileName.Substring(0, fileName.Length - Path.GetExtension(fileName).Length) + newExtension;
        }

        static void EnsureDirectory(string filePath)
        {
            string dir = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(dir))
            {
                Directory.CreateDirectory(dir);
            }
        }

        static void PrintErrorLine(string errTxt)
        {
            var curColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Red;
            Console.Error.WriteLine(errTxt);
            Console.ForegroundColor = curColor;
        }

        static bool CheckAction(string txt, out ACTION act)
        {
            act = ACTION.HELP;

            if (txt == null)
                return false;

            var actionTxtUpper = txt.ToUpper();
            if (Enum.TryParse(actionTxtUpper, out ACTION helpAction))
            {
                act = helpAction;
                return true;
            }
            else
            {
                return false;
            }
        }

        static bool CheckGetFullFileName(string cmdArg, out string fullFileName)
        {
            try
            {
                fullFileName = new FileInfo(cmdArg).FullName;
                return true;
            }
            catch
            {
                fullFileName = null;
                return false;
            }
        }

        static void PrintActionHelp(ACTION action)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.Write("Usage: ");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine(ActionUsage[action]);
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(ActionDescription[action]);
        }

        static void DoConversion(string inputFile, string outputFile, ConversionType conversionType)
        {
            if (inputFile != null && CheckGetFullFileName(inputFile, out string inputFileName))
            {
                var outputFileName = "";

                if (outputFile != null /*output is specified*/)
                {
                    if (CheckGetFullFileName(outputFile, out string userSpecifiedOutputFileName))
                        outputFileName = userSpecifiedOutputFileName;
                    else
                        PrintErrorLine($"Invalid output file name specified: '{inputFile}'\n\n" +
                            "If you would like to output to the same file name, but with the " +
                            "extension changed, simply omit the output file name argument.");
                }
                else //output not specified; use default.
                {
                    outputFileName = ChangeExtension(inputFileName, ConversionDefaultOutputFileExtension[conversionType]);
                }

                EnsureDirectory(outputFileName);

                Console.Write("Converting...");

                try
                {
                    switch (conversionType)
                    {
                        case ConversionType.JsonToTae:
                            var inputJson = File.ReadAllText(inputFileName);
                            var outputTae = JsonConvert.DeserializeObject<TAE>(inputJson);
                            DataFile.SaveToFile(outputTae, outputFileName);
                            break;
                        case ConversionType.TaeToJson:
                            var inputTae = DataFile.LoadFromFile<TAE>(inputFileName);
                            var outputJson = JsonConvert.SerializeObject(inputTae, Formatting.Indented);
                            File.WriteAllText(outputFileName, outputJson);
                            break;
                    }
                }
                catch (Exception e)
                {
                    PrintErrorLine("Failed."); //"Converting...Failed."
                    PrintErrorLine("An error occurred during conversion:");
                    PrintErrorLine(e.Message);
                }

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Done."); //"Converting...Done."
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                PrintErrorLine($"Invalid input file name specified: '{inputFile}'");
            }
        }

        const int ARG_COUNT = 4;

        public static void Main(string[] args)
        {
            if (args.Length == 0) //No args specified. Print usage and exit.
            {
                PrintUsage();
                return;
            }

            //Going to make args the user didn't enter as NULL strings rather than a shorter array.
            //That way we just pass a NULL if the user didn't enter enough parameters.
            //The null checks will spit out the right error message or whatever.
            var newArgs = new string[ARG_COUNT];
            for (int i = 0; i < ARG_COUNT; i++)
            {
                if (i < args.Length)
                    newArgs[i] = args[i].ToUpper();
                else
                    newArgs[i] = null;
            }
            args = newArgs;

            if (CheckAction(args[0], out ACTION action))
            {
                switch (action)
                {
                    case ACTION.HELP:

                        if (args[1] == null)
                        {
                            PrintUsage();
                        }
                        else
                        {
                            if (CheckAction(args[1], out ACTION helpAction))
                            {
                                PrintActionHelp(helpAction);
                            }
                            else
                            {
                                PrintErrorLine($"Invalid ACTION: '{args[1].ToUpper()}'. Run 'MeowTAE HELP' for a list of ACTIONs.");
                            }
                        }
                        return;
                    case ACTION.TOJSON:
                        DoConversion(args[1], args[2], ConversionType.TaeToJson);
                        return;
                    case ACTION.TOTAE:
                        DoConversion(args[1], args[2], ConversionType.JsonToTae);
                        return;
                }
            }
            else if (CheckGetFullFileName(args[0], out string inferredInputFileName))
            {
                var dotIndex = inferredInputFileName.LastIndexOf('.');
                string extension = dotIndex >= 0 ? inferredInputFileName.ToUpper().Substring(dotIndex) : null;

                if (extension == ".TAE")
                {
                    Console.WriteLine("Input file is likely a TAE file. Doing TAE -> JSON conversion...");
                    DoConversion(inferredInputFileName, args[1], ConversionType.TaeToJson);
                }
                else if (extension == ".JSON")
                {
                    Console.WriteLine("Input file is likely a JSON file. Doing JSON -> TAE conversion...");
                    DoConversion(inferredInputFileName, args[1], ConversionType.JsonToTae);
                }
                else
                {
                    PrintErrorLine("Could not infer input file type from the file's extension; ");
                    PrintErrorLine("files must be *.TAE or *.JSON for their type to be inferred.");
                    PrintErrorLine("");
                    PrintErrorLine("Run MeowTAE with no args to see how to specify what kind of ");
                    PrintErrorLine("conversion you wish to perform.");
                    PrintErrorLine("");
                    return;
                }
            }
            else //Not a recognized ACTION and not a valid file name.
            {
                PrintErrorLine($"Invalid ACTION: '{args[0].ToUpper()}'. Run 'MeowTAE HELP' for a list of ACTIONs.");
            }

        }
    }
}
