using System;
using System.Collections.Generic;

using UnityEngine;

using Displays;

namespace Build
{
	public class VersionInfoContainer : MonoBehaviour, IEquatable<VersionInfoContainer>
	{
		[SerializeField]
		TextAsset _versionInfoFile;

		[SerializeField, HideInInspector]
		VersionInfo currentVersionInfo;

		[SerializeField]
		List<VersionDisplay> _displays;

		public VersionInfo CurrentVersionInfo
		{
			get
			{
				return currentVersionInfo;
			}
		}

		public void ReadFromFile(TextAsset source = null)
		{
			if(source != null)
			{
				_versionInfoFile = source;
			}

			if (_versionInfoFile != null)
			{
				currentVersionInfo = JsonUtility.FromJson<VersionInfo>(_versionInfoFile.text);
			}
		}

		void OnEnable()
		{
			ReadFromFile();
			foreach(VersionDisplay display in _displays)
			{
				display.Refresh();
			}
		}

		public bool Equals(VersionInfoContainer other)
		{
			return currentVersionInfo.Equals(other.currentVersionInfo);
		}

		[Serializable]
		public class VersionInfo : IEquatable<VersionInfo>
		{
			// Product Title - MajorVersion.MinorVersion.HotfixVersion + VersionTag + (Commit) - BuildInfo

			public string ProductTitle = "";

			public string Commit = "";

			public string Branch = "";

			public int MinorVersion = 0;

			public int MajorVersion = 0;

			public int PatchVersion = 0;

			public string VersionTag = "";

			public string BuildInfo = "";

			public Color StampColor = Color.red;

			public override string ToString()
			{
				return ProductTitle + " - " + MajorVersion +"."+ MinorVersion + "." + PatchVersion + VersionTag + " [" + Commit + "/" + Branch + "]  -  " + BuildInfo;
			}

			public bool Equals(VersionInfo other)
			{
				return ProductTitle == other.ProductTitle &&
					Commit == other.Commit &&
					Branch == other.Branch &&
					MinorVersion == other.MinorVersion &&
					MajorVersion == other.MajorVersion &&
					PatchVersion == other.PatchVersion &&
					VersionTag == other.VersionTag &&
					BuildInfo == other.BuildInfo;
			}
		}
	}
}
