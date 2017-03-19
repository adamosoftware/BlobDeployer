using BlobDeployer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test
{
	class Program
	{
		static void Main(string[] args)
		{
			var options = new Options();
			options.MainExe = @"C:\Users\Adam\Dropbox\Visual Studio 2015\Projects\BlobBackupLib\BlobakUIWinForm\bin\Release\BlobakUI.exe";
			options.DeployMasterExe = @"C:\Program Files\Just Great Software\DeployMaster\DeployMaster.exe";
			options.DeployMasterScript = @"C:\Users\Adam\Dropbox\Visual Studio 2015\Projects\BlobBackupLib\Setup.deploy";
			options.InstallerExe = @"C:\Users\Adam\Dropbox\Visual Studio 2015\Projects\BlobBackupLib\BlobakSetup.exe";
			options.StorageAccountName = "adamosoftware";
			options.StorageAccountKey = "CnwT4Y9GiATbmKvVgJk0y0s8plhddOugoHM5ZUm6tskN+gq2g2xEWpYSamdgcRhby12SOuDHN9/vshAHK5VsGw==";
			options.StorageContainer = "install";					

			PersistHelper.Save(options, @"C:\Users\Adam\Dropbox\Visual Studio 2015\Projects\BlobBackupLib\BlobDeploy.xml");
		}
	}
}
