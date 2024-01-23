using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using MFilesAPI;

namespace MFClientAppDeploymentAutomation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Configuration config = new Configuration();

            Utils utils = new Utils();
            utils.Compress(config.CurrentDirectory.FullName, config.AppFilePath);
        }
    }
}
