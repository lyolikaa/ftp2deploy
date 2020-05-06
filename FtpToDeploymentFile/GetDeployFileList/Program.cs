using System;
using System.Linq;
using System.Xml.Linq;

namespace GetDeployFileList
{
    class Program
    {
        static void Main(string[] args)
        {
            //check file names
            //in arguments
            //todo  named parameters
            if (args.Length < 1)
            {
                Exit("Please Deployment file name");
            }

            //default ServerName, or take it from run parameter #2
            var ServerName = args.Length > 1 ? args[1] : "WebServer";

            var xml = XDocument.Load(args[0]);
            var query = from c in xml.Root.Descendants("Files").Descendants("SourceFile")
                        where c.Element("TargetFile").Attribute("ServerType").Value == ServerName
                        select c.Attribute("name").Value ;

            foreach (string targetFile in query.Distinct().OrderBy(s=>s))
            {
                Console.WriteLine($"file Name: {targetFile}");
            }
            Console.ReadLine();
        }

        static void Exit(string message)
        {
            Console.WriteLine(message);
            Console.ReadLine();
            Environment.Exit(0);
        }
    }
}
