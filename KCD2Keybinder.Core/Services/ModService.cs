using KDC2Keybinder.Core.Models;
using KDC2Keybinder.Core.Utils;
using Microsoft.Extensions.Logging;
using System.IO.Compression;
using System.Xml.Linq;

namespace KDC2Keybinder.Core.Services
{
	public class ModService
	{
		private readonly ILogger<ModService> logger;
		private readonly IPathProvider pathProvider;

		public ModService(ILogger<ModService> logger, IPathProvider pathProvider)
		{
			this.logger = logger;
			this.pathProvider = pathProvider;
		}

		public ModDescription CreateNewMod()
		{
			return new ModDescription
			{
				Name = "Keybinding Mod",
				Description = "Best Keybinding Mod since 1403",
				Author = "Destuur",
				ModVersion = "1.0",
				CreatedOn = DateTime.UtcNow.ToString("yyyy-MM-dd"),
				Id = "zz_keybinder",
				ModifiesLevel = false,
			};
		}

		public bool WriteModManifest(ModDescription mod)
		{
			try
			{
				var modRootPath = Path.Combine(pathProvider.ModPath, mod.Id);
				var dataPath = Path.Combine(modRootPath, "Data");
				var localizationPath = Path.Combine(modRootPath, "Localization");
				var manifestPath = Path.Combine(modRootPath, "mod.manifest");

				Directory.CreateDirectory(dataPath);

				if (File.Exists(manifestPath))
				{
					return true;
				}

				var doc = new XDocument(
					new XDeclaration("1.0", "utf-8", null),
					new XElement("kcd_mod",
						new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
						new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
						new XElement("info",
							new XElement("name", mod.Name),
							new XElement("description", mod.Description),
							new XElement("author", mod.Author),
							new XElement("version", mod.ModVersion),
							new XElement("created_on", mod.CreatedOn),
							new XElement("modid", mod.Id),
							new XElement("modifies_level", mod.ModifiesLevel.ToString().ToLowerInvariant())
						)
					)
				);

				doc.Save(manifestPath);
				return true;
			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error writing mod manifest for mod {ModId}", mod.Id);
				return false;
			}
		}

		public void CreateModPak(string baseFolder, string pakFilePath)
		{
			try
			{
				if (File.Exists(pakFilePath))
				{
					File.Delete(pakFilePath);
				}

				var outputFolder = Path.GetDirectoryName(pakFilePath);
				if (!string.IsNullOrEmpty(outputFolder) && !Directory.Exists(outputFolder))
				{
					Directory.CreateDirectory(outputFolder);
				}

				using var fs = new FileStream(pakFilePath, FileMode.CreateNew);
				using var archive = new ZipArchive(fs, ZipArchiveMode.Create);

				var files = Directory.GetFiles(baseFolder, "*", SearchOption.AllDirectories);

				foreach (var file in files)
				{
					if (Path.GetFullPath(file) == Path.GetFullPath(pakFilePath))
						continue;

					AddFileToArchive(archive, file, baseFolder);
				}

			}
			catch (Exception ex)
			{
				logger.LogError(ex, "Error creating mod .pak file at {PakFilePath}", pakFilePath);
			}
		}

		private void AddFileToArchive(ZipArchive archive, string filePath, string baseFolder)
		{
			var relativePath = Path.GetRelativePath(baseFolder, filePath).Replace('\\', '/');
			var entry = archive.CreateEntry(relativePath, CompressionLevel.NoCompression);

			using var entryStream = entry.Open();
			using var fileStream = File.OpenRead(filePath);
			fileStream.CopyTo(entryStream);

		}
	}
}
