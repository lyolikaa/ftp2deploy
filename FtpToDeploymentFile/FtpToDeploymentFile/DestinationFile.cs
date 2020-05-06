using System;
using System.Collections.Generic;
using System.Text;

namespace FtpToDeploymentFile
{
    public class DestinationFile
    {
        public string SourceFile { get; set; }
        public string SourceFolder { get; set; }

        public IEnumerable<TargetFile> TargetFiles { get; set; }
    }

    public class TargetFile
    {
        public string ServerType { get; set; }

        public List<string> TargetFolders { get; set; }
    }
}
