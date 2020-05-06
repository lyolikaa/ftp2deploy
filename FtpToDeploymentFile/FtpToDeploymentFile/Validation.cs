using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace FtpToDeploymentFile
{
    public static class Validation
    {
        public static bool ValidateSource(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return false;
            //need source file at least
            var sourceFileParts = line.Split(',');
            if (sourceFileParts.Length < 2) return false;//can ignore this condition
            //check source file name
            return ValidatePath(sourceFileParts[1]);
        }

        public static bool ValidatePath(string filename)
        {
            if (string.IsNullOrWhiteSpace(filename)) return false;
            Regex r = new Regex(@"^(([a-zA-Z]\:)|(\\))(\\{1}|((\\{1})[^\\]([^/:*?<>""|]*))+)$");
            return r.IsMatch(filename);
        }

        public static string ValidateServerName(string serverName)
        {
            switch (serverName.ToLower())
            {
                case "webserver": return "WebServer";
                case "dbserver": return "DBServer";
                case "appserver": return "AppServer";
                default: return "Common";
            }
        }
    }
}
