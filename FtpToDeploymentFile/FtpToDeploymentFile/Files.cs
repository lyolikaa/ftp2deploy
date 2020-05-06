using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml;

namespace FtpToDeploymentFile
{
    public static class Files
    {
        static ReaderWriterLock rwl = new ReaderWriterLock();
        public static IEnumerable<string> ReadFile(string fileName)
        {
            try
            {
                rwl.AcquireReaderLock(100);
                return File.ReadLines(fileName);
            }
            catch
            {
                return null;
            }
            finally
            {
                rwl.ReleaseReaderLock();
            }
        }

 
        public static void WriteDestination(string destinationFile, string sourceServer, IEnumerable<DestinationFile> files)
        {
            try
            {
                rwl.AcquireWriterLock(1000);
                using (XmlWriter xmlWriter = XmlWriter.Create(destinationFile))
                {
                    xmlWriter.WriteStartDocument();
                    xmlWriter.WriteStartElement("DeploymentInfo");

                    xmlWriter.WriteStartElement("Metadata");
                    xmlWriter.WriteAttributeString("Version", "1.5");
                    xmlWriter.WriteEndElement();

                    xmlWriter.WriteStartElement("Files");

                    foreach (var file in files)
                    {
                        xmlWriter.WriteStartElement("SourceFile");
                        xmlWriter.WriteAttributeString("name", file.SourceFile);
                        xmlWriter.WriteAttributeString("sourceServer", sourceServer);
                        xmlWriter.WriteAttributeString("sourceFolder", file.SourceFolder);

                        foreach (var targetFile in file.TargetFiles)
                        {
                            xmlWriter.WriteStartElement("TargetFile");
                            xmlWriter.WriteAttributeString("ServerType", targetFile.ServerType);
                            xmlWriter.WriteAttributeString("targetFolder", targetFile.TargetFolders.Aggregate((a, b) => a + ';' + b));
                            xmlWriter.WriteFullEndElement();
                        }

                        xmlWriter.WriteEndElement();

                        //todo 
                    };

                    xmlWriter.Close();
                }
            }
            catch
            {
                Console.WriteLine("Error while writing to xml");
            }
            finally
            {
                rwl.ReleaseWriterLock();
            }
        }

    }
}
