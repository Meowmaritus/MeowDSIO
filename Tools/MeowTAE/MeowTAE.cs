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
    class MeowTAE
    {
        static Dictionary<string, string> ActionHelp = new Dictionary<string, string>
        {
            ["help"] =

                "\nDescription: Displays help on a specific ACTION. " +
                "\nUsage: MeowTAE help <[input file]> {[output file]}"

            ,["tae2json"] = 

                "\nDescription: Converts the input file from a TAE format file to a JSON format file. " +
                "\n             If the output file name is not specified, then it is assumed to be the " +
                "\n             same file name but with a .JSON extension." +
                "\n" +
                "\nUsage: MeowTAE tae2json <[input file]> {[output file]}"

            ,["json2tae"] =

                "\nDescription: Converts the input file from a JSON format file to a TAE format file. " +
                "\n             If the output file name is not specified, then it is assumed to be the " +
                "\n             same file name but with a .TAE extension." +
                "\n" +
                "\nUsage: MeowTAE json2tae <[input file]> {[output file]}"
        };

        static void PrintUsage()
        {
            Console.WriteLine("Usage: MeowTAE <ACTION> { [input file] { [output file] } }");
            Console.WriteLine("    ");
            Console.WriteLine("Available ACTIONs:");
            foreach (var action in ActionHelp.Keys)
                Console.WriteLine("    " + action);
            Console.WriteLine("    ");
            Console.WriteLine("    ");
            Console.WriteLine("To display information about a specific ACTION:");
            Console.WriteLine("    MeowTAE help <ACTION>\n\n");
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

        static void Main(string[] args)
        {
            args = args.Select(x => x.ToLower()).ToArray();

            try
            {
                var action = args[0];

                if (action == "help")
                {
                    Console.WriteLine(ActionHelp[args[1]] + "\n\n");
                }
                else if (action == "tae2json" || action == "json2tae")
                {
                    var inputFileName = args[1];
                    var outputFileName = "";

                    if (args.Length >= 3)
                    {
                        outputFileName = args[2];
                    }
                    else
                    {
                        outputFileName = ChangeExtension(inputFileName, action == "tae2json" ? ".json" : ".tae");
                    }

                    EnsureDirectory(outputFileName);

                    if (action == "tae2json")
                    {
                        var inputTae = DataFile.LoadFromFile<TAE>(inputFileName);
                        var outputJson = JsonConvert.SerializeObject(inputTae, Formatting.Indented);
                        File.WriteAllText(outputFileName, outputJson);
                    }
                    else //json2tae
                    {
                        var inputJson = File.ReadAllText(inputFileName);
                        var outputTae = JsonConvert.DeserializeObject<TAE>(inputJson);
                        DataFile.SaveToFile(outputTae, outputFileName);
                    }
                }
            }
            catch (IndexOutOfRangeException)
            {
                PrintUsage();
            }
        }
    }
}
