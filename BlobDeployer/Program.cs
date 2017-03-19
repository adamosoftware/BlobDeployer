using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Diagnostics;
using System.IO;

namespace BlobDeployer
{
	class Program
	{
		static void Main(string[] args)
		{
			try
			{
				Options options = PersistHelper.Load<Options>(args[0]);				

				var buildVersion = GetFileVersion(options.MainExe);
				Console.WriteLine($"build version = {buildVersion}");

				var onlineVersion = GetOnlineVersion(options);
				Console.WriteLine($"online version = {onlineVersion}");

				if (buildVersion > onlineVersion)
				{
					Console.WriteLine("building new installer");
					ExecuteInstallerBuild(options, buildVersion.ToString());

					Console.WriteLine("uploading installer");
					Upload(options, buildVersion.ToString());
				}
				else
				{
					Console.WriteLine("no upload done");
				}
			}
			catch (Exception exc)
			{
				Console.WriteLine(exc.Message);
				Console.ReadLine();
			}			
		}

		private static void Upload(Options options, string version)
		{
			CloudBlobClient client = new CloudStorageAccount(new StorageCredentials(options.StorageAccountName, options.StorageAccountKey), true).CreateCloudBlobClient();

			var container = client.GetContainerReference(options.StorageContainer);
			container.CreateIfNotExists();

			string fileName = Path.GetFileName(options.InstallerExe);
			CloudBlockBlob blob = container.GetBlockBlobReference(fileName);
			blob.Metadata.Add("version", version);
			blob.UploadFromFile(options.InstallerExe);			
		}

		private static void ExecuteInstallerBuild(Options options, string version)
		{
			DeployMasterScript dms = new DeployMasterScript(options.DeployMasterScript);
			dms.SetVersion(version);

			ProcessStartInfo psi = new ProcessStartInfo(options.DeployMasterExe);
			psi.Arguments = $"\"{options.DeployMasterScript}\" /b /q";
			var p = Process.Start(psi);
			p.WaitForExit();
			if (p.ExitCode >= 2) throw new Exception("DeployMaster process failed.");
		}

		private static Version GetOnlineVersion(Options options)
		{
			CloudBlobClient client = new CloudStorageAccount(new StorageCredentials(options.StorageAccountName, options.StorageAccountKey), true).CreateCloudBlobClient();

			Version result = new Version("0.0.0");

			var container = client.GetContainerReference(options.StorageContainer);
			if (container.Exists())
			{
				var blob = container.GetBlockBlobReference(options.GetBlobName());
				if (blob?.Exists() ?? false)
				{
					try
					{
						result = new Version(blob.Metadata["version"]);
					}
					catch
					{
						// do nothing
					}					
				}
			}

			return result;
		}

		private static Version GetFileVersion(string fileName)
		{
			var fv = FileVersionInfo.GetVersionInfo(fileName);
			return new Version(fv.FileVersion);
		}
	}

}
