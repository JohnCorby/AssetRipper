﻿using AssetRipper.Core.Project;
using AssetRipper.Core.Classes.Misc;
using AssetRipper.Core.Classes.Misc.Serializable;
using AssetRipper.Core.Classes.Utils.Extensions;
using AssetRipper.Core.IO.Asset;
using AssetRipper.Core.YAML;
using System;
using UnityVersion = AssetRipper.Core.Parser.Files.UnityVersion;
using AssetRipper.Core.Math;

namespace AssetRipper.Core.Classes.AnimatorStateMachine
{
	public sealed class ChildAnimatorStateMachine : IYAMLExportable
	{
		public ChildAnimatorStateMachine(AnimatorStateMachine stateMachine, Vector3f position)
		{
			if (stateMachine == null)
			{
				throw new ArgumentNullException(nameof(stateMachine));
			}
			StateMachine = stateMachine.File.CreatePPtr(stateMachine);
			Position = position;
		}

		public static int ToSerializedVersion(UnityVersion version)
		{
#warning TODO: ToSerializedVersion
			return 1;
		}

		public YAMLNode ExportYAML(IExportContainer container)
		{
			YAMLMappingNode node = new YAMLMappingNode();
			node.AddSerializedVersion(ToSerializedVersion(container.ExportVersion));
			node.Add(StateMachineName, StateMachine.ExportYAML(container));
			node.Add(PositionName, Position.ExportYAML(container));
			return node;
		}

		public const string StateMachineName = "m_StateMachine";
		public const string PositionName = "m_Position";

		public PPtr<AnimatorStateMachine> StateMachine = new();
		public Vector3f Position;
	}
}
