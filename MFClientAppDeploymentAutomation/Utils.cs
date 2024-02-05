﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Threading;


namespace MFClientAppDeploymentAutomation
{
    internal class Utils
    {

        public VaultConfiguration GetVaultConfig(string path)
        {
            if (!File.Exists(path))
            {
                throw new Exception("No Config File Found");
            }

            StreamReader reader = new StreamReader(path);
            string json = reader.ReadToEnd() ?? throw new Exception("No Config Data found");
            VaultConfiguration vaultConfig = JsonConvert.DeserializeObject<VaultConfiguration>(json);

            return vaultConfig;
        }
        public void SetVaultConfig(string destPath, string data, string type)
        {
            string jsonData = "";

            if (type == "path")
            {
                VaultConfiguration newVaultConfig = this.GetVaultConfig(data);
                jsonData = JsonConvert.SerializeObject(newVaultConfig, Formatting.Indented);
            }
            else if (type == "json")
            {
                VaultConfiguration jsonObj = JsonConvert.DeserializeObject<VaultConfiguration>(data);
                jsonData = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            }
            File.WriteAllText(destPath, jsonData);
        }

        public bool IsTest()
        {
            Dictionary<string, string> dotenv = new Dictionary<string, string>();

            string dotenvPath = "C:\\Users\\A505471\\source\\repos\\MFClientAppDeploymentAutomation\\MFClientAppDeploymentAutomation\\.env";
            IEnumerable<string> dotenvContent = File.ReadLines(dotenvPath);

            foreach (string line in dotenvContent)
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
            
            if (dotenv.ContainsKey("WORKING_ENV"))
            {
                return dotenv["WORKING_ENV"] == "TEST";
            }
            return false;
        }

        public void Compress(string folderPath, string zipPath)
        {
            try
            {
                ZipFile.CreateFromDirectory(folderPath, zipPath);
                Console.WriteLine("Folder zipped");
            }
             catch (Exception ex)
            {
                Console.WriteLine($"Error zipping folder: {ex.Message}");
            }
        }

        public void DeleteZipped(string zipPath)
        {
            if (File.Exists(zipPath))
            {
                File.Delete(zipPath);
            }
        }
    }
}
