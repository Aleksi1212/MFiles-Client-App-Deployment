using MFilesAPI;
using System;
using System.IO;
using System.Xml;

namespace MFClientAppDeploymentAutomation
{
    internal class AppConfiguration
    {
        private readonly bool testEnvironment = new Utils().IsTest();

        public DirectoryInfo CurrentDirectory
        {
            get
            {
                return testEnvironment
                    ? new DirectoryInfo("C:\\Users\\A505471\\source\\repos\\MFClientAppDeploymentAutomation\\MFClientAppDeploymentAutomation\\test")
                    : new DirectoryInfo(Directory.GetCurrentDirectory());
            }
        }
        public string AppFilePath
        {
            get { return Path.Combine(Environment.GetEnvironmentVariable("CLIENT_APP_BUILD_FOLDER"), $"{CurrentDirectory.Name}.zip"); }
        }
        public string AppGuid
        {
            get
            {
                string appdefPath = testEnvironment
                    ? "C:\\Users\\A505471\\source\\repos\\MFClientAppDeploymentAutomation\\MFClientAppDeploymentAutomation\\test\\appdef.xml"
                    : Path.Combine(CurrentDirectory.FullName, "appdef.xml");

                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.Load(appdefPath);

                return xmlDoc.SelectSingleNode("//guid")?.InnerText;
            }
        }
    }

    internal class VaultConfiguration
    {
        public readonly string VaultName = "Aleksin hiekkalaatikka (VAN02)";
        public readonly MFAuthType AuthType = MFAuthType.MFAuthTypeSpecificWindowsUser;
        public readonly string UserName = "mfilestest";
        public readonly string Password = Environment.GetEnvironmentVariable("VAN02PASSWORD");
        public readonly string Domain = "dekrafinland.fi";
        public readonly string Spn = "";
        public readonly string ProtocolSequence = "ncacn_ip_tcp";
        public readonly string NetworkAddress = "van02.dekra.fi";
        public readonly string Endpoint = "2266";
        public readonly bool EncryptedConnection = false;
        public readonly string LocalComputerName = "";
    }
}
