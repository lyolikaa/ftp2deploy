using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FtpToDeploymentFile
{
    public static class BL
    {
        public static IEnumerable<TargetFile> GetTargetFiles (IEnumerable<SourceFile> sourceList)
        {
            //get all target ServerTypes
            var ServerTypes = sourceList.Select(f => f.ServerType.ToLower()).Distinct();
            var targetFolders = sourceList.Select(f => f.TargetFullNamePath.ToLower()).Distinct();

            //if we put to common, aggregating targetFolders 
            if (ServerTypes.Contains("common") || ServerTypes.Count() > 2)
            {
                return new List<TargetFile>
                        { new TargetFile{
                            ServerType = "Common",
                            TargetFolders = targetFolders.ToList()
                        }};
            }
            else //add for every ServerType
            {
                var TargetFiles = new List<TargetFile>();
                foreach (var sType in sourceList.GroupBy(s => s.ServerType))
                {
                    TargetFiles.Add(new TargetFile
                    {

                        ServerType = sType.Key,
                        TargetFolders = sType.ToList().Select(f => f.TargetFullNamePath.ToLower()).Distinct().ToList()

                    });
                }
                return TargetFiles;
            }
        }
    }
}
