using MFilesAPI;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MFClientAppDeploymentAutomation
{
    internal class Configuration
    {
        readonly DirectoryInfo currentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        public string appFilePath
        {
            get { return Path.Combine(Environment.GetEnvironmentVariable("CLIENT_APP_BUILD_FOLDER"), $"{currentDirectory.Name}.zip"); }
        }
        public string appGuid
        {
            get
            {
                bool testEnvironment = new Utils().IsTest();

                string appdefPath = testEnvironment
                    ? "C:\\Users\\A505471\\source\\repos\\MFClientAppDeploymentAutomation\\MFClientAppDeploymentAutomation\\test\\appdef.xml"
                    : Path.Combine(currentDirectory.FullName, "appdef.xml");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(appdefPath);

                return xmlDoc.SelectSingleNode("//guid")?.InnerText;
            }
        }
        public string vaultName = "Aleksin hiekkalaatikka (VAN02)";
        
        public MFAuthType authType = MFAuthType.MFAuthTypeSpecificWindowsUser;
        public string userName = "mfilestest";
        public string password = Environment.GetEnvironmentVariable("VAN02PASSWORD");
        public string domain = "dekrafinland.fi";
        public string spn = "";
        public string protocolSequence = "ncacn_ip_tcp";
        public string networkAddress = "van02.dekra.fi";
        public int endpoint = 2266;
        public bool encryptedConnection = false;
        public string localComputerName = "";
    }
}
