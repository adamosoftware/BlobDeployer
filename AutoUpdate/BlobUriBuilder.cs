using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoUpdate
{
	public class BlobUriBuilder
	{
		public string StorageAccountName { get; set; }
		public string StorageContainer { get; set; }
		public string InstallerName { get; set; }
		
		public Uri ToUri()
		{
			return new Uri(ToString());
		}

		public override string ToString()
		{
			return $"https://{StorageAccountName}.blob.core.windows.net:443/{StorageContainer}/{InstallerName}";
		}
	}
}
