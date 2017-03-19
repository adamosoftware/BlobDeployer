using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlobDeployer
{
	public enum DeployMasterLines
	{
		ProductName = 4,
		Version = 6
	}

	public class DeployMasterScript
	{
		private readonly string _fileName;
		private readonly string[] _lines;

		public DeployMasterScript(string fileName)
		{
			_fileName = fileName;
			_lines = File.ReadAllLines(fileName);
		}

		public void SetVersion(string version)
		{
			_lines[(int)DeployMasterLines.Version] = version;
			using (StreamWriter writer = File.CreateText(_fileName))
			{
				foreach (string line in _lines) writer.WriteLine(line);
			}
		}
	}
}
