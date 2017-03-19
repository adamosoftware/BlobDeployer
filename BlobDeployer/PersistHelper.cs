using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BlobDeployer
{
	public static class PersistHelper
	{
		public static T Load<T>(string fileName)
		{
			T result = default(T);
			XmlSerializer xs = new XmlSerializer(typeof(T));
			using (StreamReader reader = File.OpenText(fileName))
			{
				result = (T)xs.Deserialize(reader);
			}
			return result;
		}

		public static void Save<T>(T @object, string fileName, bool createFolder = true)
		{
			if (createFolder)
			{
				string folder = Path.GetDirectoryName(fileName);
				if (!Directory.Exists(folder)) Directory.CreateDirectory(folder);
			}

			XmlSerializer xs = new XmlSerializer(typeof(T));
			using (StreamWriter writer = File.CreateText(fileName))
			{
				xs.Serialize(writer, @object);
			}
		}
	}
}
