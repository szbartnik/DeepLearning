using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Wkiro.ImageClassification.Gui.Helpers
{
	public static class AssembliesHelpers
	{
		private static readonly string[] DirNames =
		{
			"Libraries"
		};

		public static Dictionary<string, string> LoadKnownAssemblies()
		{
			var toReturn = new Dictionary<string, string>();
			var workingDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
			var dllDirectories = DirNames.Select(x => Path.Combine(workingDirectory, x));
			foreach (var enumerateFile in dllDirectories.Where(Directory.Exists).SelectMany(x => Directory.GetFiles(x, "*.dll", SearchOption.AllDirectories)))
			{
				try
				{
					toReturn.Add(AssemblyName.GetAssemblyName(enumerateFile).FullName, Path.GetFullPath(enumerateFile));
				}
				catch { /* ignore native files */ }
			}
			return toReturn;
		}

		public static Assembly Resolve(IReadOnlyDictionary<string, string> knownAssemblies, object s, ResolveEventArgs e)
		{
			var requiredAssembly = new AssemblyName(e.Name);
			var assembly = AppDomain.CurrentDomain
									.GetAssemblies()
									.FirstOrDefault(x => new AssemblyName(x.FullName).Name == requiredAssembly.Name);

			if (assembly != null)
				return assembly;

			var binding = knownAssemblies.SingleOrDefault(x => x.Key.StartsWith(e.Name.Split(',')[0]));
			return Assembly.LoadFile(binding.Value);
		}
	}
}