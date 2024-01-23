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
        public DirectoryInfo CurrentDirectory = new DirectoryInfo(Directory.GetCurrentDirectory());
        public string AppFilePath
        {
            get { return Path.Combine(Environment.GetEnvironmentVariable("CLIENT_APP_BUILD_FOLDER"), $"{CurrentDirectory.Name}.zip"); }
        }
        public string AppGuid
        {
            get
            {
                bool testEnvironment = new Utils().IsTest();

                string appdefPath = testEnvironment
                    ? "C:\\Users\\A505471\\source\\repos\\MFClientAppDeploymentAutomation\\MFClientAppDeploymentAutomation\\test\\appdef.xml"
                    : Path.Combine(CurrentDirectory.FullName, "appdef.xml");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(appdefPath);

                return xmlDoc.SelectSingleNode("//guid")?.InnerText;
            }
        }
        public string VaultName = "Aleksin hiekkalaatikka (VAN02)";
        
        public MFAuthType AuthType = MFAuthType.MFAuthTypeSpecificWindowsUser;
        public string UserName = "mfilestest";
        public string Password = Environment.GetEnvironmentVariable("VAN02PASSWORD");
        public string Dmain = "dekrafinland.fi";
        public string Spn = "";
        public string ProtocolSequence = "ncacn_ip_tcp";
        public string NetworkAddress = "van02.dekra.fi";
        public int Endpoint = 2266;
        public bool EncryptedConnection = false;
        public string LocalComputerName = "";
    }
}
