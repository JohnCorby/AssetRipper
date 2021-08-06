using AssetRipper.Core.Project;
using AssetRipper.Core.Logging;
using AssetRipper.Core.Parser.Asset;
using AssetRipper.Core.Classes.Misc;
using AssetRipper.Core.Parser.Files;
using AssetRipper.Core.IO.Asset;
using AssetRipper.Core.YAML;
using System.Collections.Generic;
using System.IO;

namespace AssetRipper.Core.Classes
{
	public sealed class MovieTexture : BaseVideoTexture
	{
		public MovieTexture(AssetInfo assetInfo) : base(assetInfo) { }

		/// <summary>
		/// Less than 5.0.0
		/// </summary>
		private static bool HasData(UnityVersion version) => version.IsLess(5);
		/// <summary>
		/// 5.0.0 to 2019.3 exclusive
		/// </summary>
		private static bool IsInherited(UnityVersion version) => version.IsGreaterEqual(5) && version.IsLess(2019, 3);

		public override void Read(AssetReader reader)
		{
			if (HasData(reader.Version) || IsInherited(reader.Version))
			{
				base.Read(reader);
			}
			else
			{
				ReadTexture(reader);
			}
		}

		public override void ExportBinary(IExportContainer container, Stream stream)
		{
			if (HasData(container.Version) || IsInherited(container.Version))
			{
				base.ExportBinary(container, stream);
			}
			else
			{
				Logger.Log(LogType.Warning, LogCategory.Export, "Movie texture doesn't have any data");
			}
		}

		public override IEnumerable<PPtr<Object.Object>> FetchDependencies(DependencyContext context)
		{
			if (HasData(context.Version) || IsInherited(context.Version))
			{
				foreach (PPtr<Object.Object> asset in base.FetchDependencies(context))
				{
					yield return asset;
				}
			}
			else
			{
				foreach (PPtr<Object.Object> asset in FetchDependenciesTexture(context))
				{
					yield return asset;
				}
			}
		}

		protected override YAMLMappingNode ExportYAMLRoot(IExportContainer container)
		{
			if (HasData(container.Version) || IsInherited(container.Version))
			{
				return base.ExportYAMLRoot(container);
			}
			else
			{
				return ExportYAMLRootTexture(container);
			}
		}
	}
}
