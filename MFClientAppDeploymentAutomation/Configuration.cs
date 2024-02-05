using MFilesAPI;
using System;
using System.IO;
using System.Xml;

namespace MFClientAppDeploymentAutomation
{
    internal class AppConfiguration
    {
        private readonly bool testEnvironment = new Utils().IsTest();
        public readonly string VaultConfigFilePath = "C:\\Users\\A505471\\source\\repos\\deploymentConfig\\config.json";

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
        public string VaultName { get; set; }
        public int AuthType { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Domain { get; set; }
        public string Spn { get; set; }
        public string ProtocolSequence { get; set; }
        public string NetworkAddress { get; set; }
        public string Endpoint { get; set; }
        public bool EncryptedConnection { get; set; }
        public string LocalComputerName { get; set; }
    }
}
