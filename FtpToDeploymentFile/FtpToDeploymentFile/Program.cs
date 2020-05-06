using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Xml;

namespace FtpToDeploymentFile
{
    class Program
    {
        static void Main(string[] args)
        {
            //check if we have source and destination file names
            //in arguments
            //todo  named parameters
            if (args.Length < 2)
            {
                Exit("Please specify source and destination file names ");
            }

            var sourceServer = args[0];
            var sourceLines = Files.ReadFile(sourceServer);
            if (sourceLines == null)
                Exit("Unable to read source file");
            Console.WriteLine($"sourceLines: {sourceLines.Count()} items");
            var sourceFiles = sourceLines.Where(Validation.ValidateSource).Select(SourceFiles);
            Console.WriteLine($"sourceFiles: {sourceFiles.Count()} items");
            if (!sourceFiles.Any())
                Exit("No valid data in source file");

            //todo remove duplicates by RecordNum
            //todo change target name if it comes from different sources (e.g. Name -> Name(1))

            var destinationFiles = new List<DestinationFile>();
            //grouping by SourceFile and sourceFolder to write
            var sourceGroups = sourceFiles.GroupBy(f => new { path = Path.GetFullPath(f.SourceFullNamePath), name = Path.GetFileName(f.SourceFullNamePath) });
            //test
            //sourceGroups = sourceGroups.Take(1);
            foreach (var sourceGroup in sourceGroups)
            {
                var destinationFile = new DestinationFile
                {
                    SourceFile = sourceGroup.Key.name,
                    SourceFolder = sourceGroup.Key.path,
                };
                destinationFile.TargetFiles = BL.GetTargetFiles(sourceGroup.ToList());

                destinationFiles.Add(destinationFile);
            }

            Files.WriteDestination(args[1], sourceServer, destinationFiles);
            Console.WriteLine($"destinationFiles: {destinationFiles.Count()} items");
            Console.ReadKey();
        }




        static SourceFile SourceFiles(string line)
        {
            if (!Validation.ValidateSource(line)) return null;
            var sourceFileParts = line.Split(',');

            //if no Target file name or it is invalid, copy to same as source
            //if no Server name or it is invalid, put to Common

            var sourceFile = new SourceFile
            {
                RecordNum = sourceFileParts[0],
                SourceFullNamePath = sourceFileParts[1],
                ServerType = Validation.ValidateServerName(sourceFileParts[2]),
                TargetFullNamePath = Validation.ValidatePath(sourceFileParts[3]) ? sourceFileParts[3] : sourceFileParts[0]
            };

            return sourceFile;
        }



        //todo Thread safe
        //static is safe

        static void Exit(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
            Environment.Exit(0);
        }
    }


}
