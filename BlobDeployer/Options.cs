using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobDeployer
{
	public class Options
	{		
		public string MainExe { get; set; }
		public string DeployMasterExe { get; set; }
		public string DeployMasterScript { get; set; }
		public string InstallerExe { get; set; }		
		public string StorageAccountName { get; set; }
		public string StorageAccountKey { get; set; }
		public string StorageContainer { get; set; }

		public string GetBlobName()
		{
			return Path.GetFileName(InstallerExe);
		}

		public string GetBlobUri()
		{			
			return $"https://{StorageAccountName}.blob.core.windows.net:443/{StorageContainer}/{GetBlobName()}";
		}
	}
}
