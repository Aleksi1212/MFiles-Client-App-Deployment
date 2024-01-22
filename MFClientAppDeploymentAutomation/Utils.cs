using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace MFClientAppDeploymentAutomation
{
    internal class Utils
    {
        public bool IsTest()
        {
            Dictionary<string, string> dotenv = new Dictionary<string, string>();

            var dotenvFile = File.ReadLines("C:\\Users\\A505471\\source\\repos\\MFClientAppDeploymentAutomation\\MFClientAppDeploymentAutomation\\.env");
            foreach (var line in dotenvFile)
            {
                int index = line.IndexOf('=');
                string key = "";
                string value = "";

                if (index >= 0)
                {
                    key = line.Substring(0, index);
                    value = line.Substring(index + 1);
                }
                dotenv[key] = value;
            }

            return dotenv["WORKING_ENV"] == "TEST";
        }
    }
}
