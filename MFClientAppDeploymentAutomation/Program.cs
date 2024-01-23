using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using MFilesAPI;

namespace MFClientAppDeploymentAutomation
{
    internal class Program
    {
        static void Main(string[] args)
        {
            AppConfiguration appConfig = new AppConfiguration();
            VaultConfiguration vaultConfig = new VaultConfiguration();

            Utils utils = new Utils();
            utils.Compress(appConfig.CurrentDirectory.FullName, appConfig.AppFilePath);

            MFilesServerApplicationClass server = new MFilesServerApplicationClass();
            TimeZoneInformationClass tzi = new TimeZoneInformationClass();
            tzi.LoadWithCurrentTimeZone();

            try
            {
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
