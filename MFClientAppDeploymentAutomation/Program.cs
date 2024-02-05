using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using MFilesAPI;

namespace MFClientAppDeploymentAutomation
{
	internal class Program
	{
		static void Main(string[] args)
		{
			Configuration Config = new Configuration();
			Utils Utils = new Utils();
			Configuration.App appConfig = new Configuration.App();

			if (args.Length == 0)
			{
				Configuration.Vault vaultConfig = Config.GetVaultConfig(appConfig.VaultConfigFilePath);

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
					Utils.Compress(appConfig.CurrentDirectory.FullName, appConfig.FilePath);

					// Connect to server
					Console.WriteLine($"Connecting to {vaultConfig.VaultName} on {vaultConfig.NetworkAddress}");
					server.ConnectAdministrativeEx(
						tzi,
						(MFAuthType)vaultConfig.AuthType,
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
					vault.CustomApplicationManagementOperations.InstallCustomApplication(appConfig.FilePath);
					Console.WriteLine("Application succesfully installed");

					Utils.DeleteZipped(appConfig.FilePath);
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
			else
			{
				if (args[0].StartsWith("--"))
				{
					string[] flagContent = args[0].Substring(2).Split('=');
					string flagKey = flagContent[0],
						flagValue = flagContent[1];

					if (flagKey == "set-config")
					{
						bool isPath = new Regex("^([a-zA-Z]\\:)(\\\\[^\\\\/:*?<>\"|]*(?<![ ]))*(\\.[a-zA-Z]{2,6})$").IsMatch(flagValue);
						if (isPath)
							Config.SetVaultConfig(appConfig.VaultConfigFilePath, flagValue, "path");
						else
							Config.SetVaultConfig(appConfig.VaultConfigFilePath, flagValue, "json");
						Console.WriteLine("Config updated");
					}
					else
					{
						Console.WriteLine($"Invalid flag: {flagKey}");
					}
				}
				else
				{
					Console.WriteLine("No flags provided");

				}
			}
		}
	}
}
