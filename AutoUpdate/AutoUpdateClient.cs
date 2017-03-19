using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AutoUpdate
{
    public class AutoUpdateClient
    {		
		private readonly string _installerUri;
		private readonly Version _currentVersion;
		private readonly string _localInstaller;

		public AutoUpdateClient(BlobUriBuilder builder) : this(builder.ToString())
		{
		}

		public AutoUpdateClient(string installerUri)
		{			
			_installerUri = installerUri;
			_currentVersion = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetCallingAssembly().Location).FileVersion);
			_localInstaller = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Temp Installer", Path.GetFileName(installerUri));
			if (File.Exists(_localInstaller)) File.Delete(_localInstaller);
		}

		public bool IsNewVersionAvailable(out Version cloudVersion)
		{
			CloudBlockBlob installerBlob = new CloudBlockBlob(new Uri(_installerUri));
			cloudVersion = GetCloudVersion(installerBlob);
			return (cloudVersion > _currentVersion);
		}

		public bool IsNewVersionAvailable()
		{
			Version version;
			return IsNewVersionAvailable(out version);
		}

		public async Task DownloadAndInstallAsync()
		{
			if (!IsNewVersionAvailable()) throw new Exception("No new version is available");

			CloudBlockBlob installerBlob = new CloudBlockBlob(new Uri(_installerUri));			
			await installerBlob.DownloadToFileAsync(_localInstaller, FileMode.Create);

			ProcessStartInfo psi = new ProcessStartInfo(_localInstaller);
			Process.Start(psi);

			Application.Exit();
		}

		private Version GetCloudVersion(CloudBlockBlob installerBlob)
		{
			Version result = new Version("0.0.0");
			try
			{
				if (installerBlob?.Exists() ?? false)
				{
					result = new Version(installerBlob.Metadata["version"]);
				}
			}
			catch 
			{
				// do nothing
			}
			return result;
		}
	}
}
