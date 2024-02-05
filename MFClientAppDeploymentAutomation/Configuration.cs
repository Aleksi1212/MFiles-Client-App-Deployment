using MFilesAPI;
using System;
using System.IO;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace MFClientAppDeploymentAutomation
{
    internal class Configuration
    {
        public class App
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
            public string FilePath
            {
                get { return Path.Combine(Environment.GetEnvironmentVariable("CLIENT_APP_BUILD_FOLDER"), $"{CurrentDirectory.Name}.zip"); }
            }
            public string Guid
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

        public class Vault
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

        public Vault GetVaultConfig(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("No Config File Found");
            }

            StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd() ?? throw new Exception("No Config Data found");
            Vault vaultConfig = JsonConvert.DeserializeObject<Vault>(json);

            return vaultConfig;
        }
        public void SetVaultConfig(string destPath, string data, string type)
        {
            string jsonData = "";

            if (type == "path")
            {
                Vault newVaultConfig = this.GetVaultConfig(data);
                jsonData = JsonConvert.SerializeObject(newVaultConfig, Formatting.Indented);
            }
            else if (type == "json")
            {
                Vault jsonObj = JsonConvert.DeserializeObject<Vault>(data);
                jsonData = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            }
            File.WriteAllText(destPath, jsonData);
        }
    }
}
