﻿using AssetRipper.Core.Project;
using AssetRipper.Core.Parser.Asset;
using AssetRipper.Core.Classes.Misc;
using AssetRipper.Core.Classes.Misc.Serializable;
using AssetRipper.Core.Parser.Files;
using AssetRipper.Core.IO.Asset;
using AssetRipper.Core.YAML;
using System.Collections.Generic;
using AssetRipper.Core.Math;

namespace AssetRipper.Core.Classes.LightingDataAsset
{
	public class RendererData : IAssetReadable, IYAMLExportable, IDependent
	{
		public RendererData() { }
		/// <summary>
		/// 2018.2 and greater
		/// </summary>
		public static bool HasExplicitProbeSetHash(UnityVersion version) => version.IsGreaterEqual(2018, 2);

		public void Read(AssetReader reader)
		{
			UVMesh.Read(reader);
			TerrainDynamicUVST.Read(reader);
			TerrainChunkDynamicUVST.Read(reader);
			LightmapIndex = reader.ReadUInt16();
			LightmapIndexDynamic = reader.ReadUInt16();
			LightmapST.Read(reader);
			LightmapSTDynamic.Read(reader);
			if (HasExplicitProbeSetHash(reader.Version))
			{
				ExplicitProbeSetHash.Read(reader);
			}
		}

		public IEnumerable<PPtr<Object.Object>> FetchDependencies(DependencyContext context)
		{
			yield return context.FetchDependency(UVMesh, UvMeshName);
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			node.Add(UvMeshName, UVMesh.ExportYAML(container));
			node.Add(TerrainDynamicUVSTName, TerrainDynamicUVST.ExportYAML(container));
			node.Add(TerrainChunkDynamicUVSTName, TerrainChunkDynamicUVST.ExportYAML(container));
			node.Add(LightmapIndexName, LightmapIndex);
			node.Add(LightmapIndexDynamicName, LightmapIndexDynamic);
			node.Add(LightmapSTName, LightmapST.ExportYAML(container));
			node.Add(LightmapSTDynamicName, LightmapSTDynamic.ExportYAML(container));
			if (HasExplicitProbeSetHash(container.ExportVersion))
			{
				node.Add(ExplicitProbeSetHashName, ExplicitProbeSetHash.ExportYAML(container));
			}
			return node;
		}

		public ushort LightmapIndex { get; set; }
		public ushort LightmapIndexDynamic { get; set; }

		public const string UvMeshName = "uvMesh";
		public const string TerrainDynamicUVSTName = "terrainDynamicUVST";
		public const string TerrainChunkDynamicUVSTName = "terrainChunkDynamicUVST";
		public const string LightmapIndexName = "lightmapIndex";
		public const string LightmapIndexDynamicName = "lightmapIndexDynamic";
		public const string LightmapSTName = "lightmapST";
		public const string LightmapSTDynamicName = "lightmapSTDynamic";
		public const string ExplicitProbeSetHashName = "explicitProbeSetHash";

		public PPtr<Mesh.Mesh> UVMesh = new();
		public Vector4f TerrainDynamicUVST;
		public Vector4f TerrainChunkDynamicUVST;
		public Vector4f LightmapST;
		public Vector4f LightmapSTDynamic;
		public Hash128 ExplicitProbeSetHash = new();
	}
}
