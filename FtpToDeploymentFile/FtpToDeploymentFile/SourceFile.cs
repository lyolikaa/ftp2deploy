using System;
using System.Collections.Generic;
using System.Text;

namespace FtpToDeploymentFile
{
    public class SourceFile
    {
        public string RecordNum { get; set; }
        public string SourceFullNamePath { get; set; }
        public string ServerType { get; set; }
        public string TargetFullNamePath { get; set; }
    }
}
