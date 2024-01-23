using System;
using System.IO;
using System.Linq;
using MFilesAPI;

namespace MFClientAppDeploymentAutomation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppConfiguration appConfig = new AppConfiguration();
            VaultConfiguration vaultConfig = new VaultConfiguration();

            MFilesServerApplicationClass server = new MFilesServerApplicationClass();
            TimeZoneInformationClass tzi = new TimeZoneInformationClass();
            tzi.LoadWithCurrentTimeZone();

            try
            {
                // Check for appdef.xml file
                string[] files = Directory.GetFiles(appConfig.CurrentDirectory.FullName);
                if (!files.Contains<string>($"{appConfig.CurrentDirectory.FullName}\\appdef.xml"))
                {
                    throw new Exception("No appdef.xml file found");
                }

                // Zip folder containing project
                Utils utils = new Utils();
                utils.Compress(appConfig.CurrentDirectory.FullName, appConfig.AppFilePath);

                // Connect to server
                Console.WriteLine($"Connecting to {vaultConfig.VaultName} on {vaultConfig.NetworkAddress}");
                server.ConnectAdministrativeEx(
                    tzi,
                    vaultConfig.AuthType,
                    vaultConfig.UserName,
                    vaultConfig.Password,
                    vaultConfig.Domain,
                    vaultConfig.Spn,
                    vaultConfig.ProtocolSequence,
                    vaultConfig.NetworkAddress,
                    vaultConfig.Endpoint,
                    vaultConfig.EncryptedConnection,
                    vaultConfig.LocalComputerName
                    );

                // Get vault on server with provided name and try logging into it
                VaultOnServer vaultOnServer = server.GetOnlineVaults().GetVaultByName(vaultConfig.VaultName);
                Vault vault = vaultOnServer.LogIn();
                if (!vault.LoggedIn)
                {
                    throw new Exception("Error logging in");
                }
                Console.WriteLine("Succesfully connected to vault");

                // Install application
                Console.WriteLine("Installing application...");
                vault.CustomApplicationManagementOperations.InstallCustomApplication(appConfig.AppFilePath);
                Console.WriteLine("Application succesfully installed");

                utils.DeleteZipped(appConfig.AppFilePath);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("0x80040031"))
                {
                    Console.WriteLine("This application version already exists on the vault, installation skipped");
                }
                else if (ex.Message.Contains("0x8004091E"))
                {
                    Console.WriteLine("A newer version of this application is already installed on the vault, installation skipped");
                }
                else
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
