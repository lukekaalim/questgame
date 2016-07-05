using UnityEngine;
using UnityEditor;

using System;
using System.IO;

using Displays;

namespace Build
{
	public class VersioningWindow : EditorWindow
	{
		VersionInfoContainer.VersionInfo _info;

		[MenuItem("Window/Version")]
		static void Init()
		{
			VersioningWindow window = GetWindow<VersioningWindow>("Version", false);
			window._info = GetInfo();
			window.Show();
		}

		public static VersionInfoContainer.VersionInfo GetInfo()
		{
			VersionInfoContainer.VersionInfo info = new VersionInfoContainer.VersionInfo();

			TextAsset file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/versionInfo.json");
			if (file != null)
			{
				info = JsonUtility.FromJson<VersionInfoContainer.VersionInfo>(file.text);

				if (info == null)
				{
					info = new VersionInfoContainer.VersionInfo();
				}
			}

			return info;
		}

		void OnGUI()
		{
			/*
			TextAsset oldFile = _file;
			_file = EditorGUILayout.ObjectField("Version File Source", _file, typeof(TextAsset),false) as TextAsset;
			if(oldFile != _file)
			{
				_info = JsonUtility.FromJson<VersionInfoContainer.VersionInfo>(_file.text);
			}
			*/

			if(_info == null)
			{
				_info = GetInfo();
			}

			EditorGUILayout.Space();

			EditorGUILayout.LabelField("Contol the version of the Product", EditorStyles.boldLabel);

			EditorGUILayout.Space();

			_info.ProductTitle = EditorGUILayout.TextField("Product Title", _info.ProductTitle);

			EditorGUILayout.BeginHorizontal();
			_info.MajorVersion = EditorGUILayout.IntField("Major Version",_info.MajorVersion);
			if(GUILayout.Button("+", GUILayout.Width(30)))
			{
				_info.MajorVersion++;
				GUI.FocusControl("");
			}
			if (_info.MajorVersion > 0 && GUILayout.Button("-", GUILayout.Width(30)))
			{
				_info.MajorVersion--;
				GUI.FocusControl("");
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			_info.MinorVersion = EditorGUILayout.IntField("Minor Version", _info.MinorVersion);
			if (GUILayout.Button("+", GUILayout.Width(30)))
			{
				_info.MinorVersion++;
				GUI.FocusControl("");
			}
			if (_info.MinorVersion > 0 && (GUILayout.Button("-", GUILayout.Width(30))))
			{
				_info.MinorVersion--;
				GUI.FocusControl("");
			}
			EditorGUILayout.EndHorizontal();

			EditorGUILayout.BeginHorizontal();
			_info.PatchVersion = EditorGUILayout.IntField("Patch Version", _info.PatchVersion);
			if (GUILayout.Button("+", GUILayout.Width(30)))
			{
				_info.PatchVersion++;
				GUI.FocusControl("");
			}
			if (_info.PatchVersion > 0 && GUILayout.Button("-", GUILayout.Width(30)))
			{
				_info.PatchVersion--;
				GUI.FocusControl("");
			}
			EditorGUILayout.EndHorizontal();

			_info.VersionTag = EditorGUILayout.TextField("Version Tag", _info.VersionTag);

			EditorGUILayout.LabelField("Commit ID", _info.Commit);
			EditorGUILayout.LabelField("Branch", _info.Branch);

			_info.BuildInfo = EditorGUILayout.TextField("Build Info", _info.BuildInfo);

			_info.StampColor = EditorGUILayout.ColorField("Stamp Color", _info.StampColor);

			EditorGUILayout.Space();

			Color originalColor = GUI.color;

			EditorGUILayout.BeginHorizontal();
			GUI.color = Color.yellow;
			if (GUILayout.Button("Save"))
			{
				string jsonInfo = JsonUtility.ToJson(_info);
				TextAsset file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/versionInfo.json");

				if (file != null)
				{
					File.Delete("Assets/versionInfo.json");
				}

				StreamWriter writer = new StreamWriter("Assets/versionInfo.json", false);
				writer.Write(jsonInfo);
				writer.Close();

				AssetDatabase.Refresh();

				file = AssetDatabase.LoadAssetAtPath<TextAsset>("Assets/versionInfo.json");

				VersionInfoContainer[] everyVersionInfoInScene = FindObjectsOfType<VersionInfoContainer>();
				foreach (VersionInfoContainer info in everyVersionInfoInScene)
				{
					info.ReadFromFile(file);
				}

				VersionDisplay[] everyVersionDisplayInScene = FindObjectsOfType<VersionDisplay>();
				foreach (VersionDisplay display in everyVersionDisplayInScene)
				{
					display.Refresh();
				}
			}
			GUI.color = Color.green;
			if (GUILayout.Button("Refresh Git Info"))
			{
				string gitDirectory;
				if (GetGitDirectory(out gitDirectory))
				{
					GetGitInformation(gitDirectory);
				}
				else
				{
					Debug.LogWarning("No Git directory found. Are you sure this is a repo?");
				}
			}
			EditorGUILayout.EndHorizontal();
			GUI.color = originalColor;
		}

		//Do some searching to get the info I want
		//First, grab the root directory of the repo
		bool GetGitDirectory(out string directory)
		{
			directory = "";
			DirectoryInfo currentDirectory = new DirectoryInfo("Assets");
			while(currentDirectory.Parent != null && !Directory.Exists(currentDirectory.FullName + "/.git"))
			{
				currentDirectory = currentDirectory.Parent;
			}
			if(currentDirectory.Parent == null)
			{
				return false;
			}

			directory = currentDirectory.FullName + "/.git";
			return true;
		}

		void GetGitInformation(string directory)
		{
			StreamReader reader = new StreamReader(directory + "/HEAD");

			string fileContents = reader.ReadToEnd();

			reader.Close();

			if(fileContents.StartsWith("ref: "))
			{
				fileContents = fileContents.Substring(5, fileContents.Length - 6);

				directory = directory.Replace('\\','/');
				fileContents = fileContents.Replace(Environment.NewLine, "");

				FileInfo refFile = new FileInfo(directory + "/" + fileContents);

				_info.Branch = refFile.Name.ToLower();

				reader = new StreamReader(directory + "/" + fileContents);
				fileContents = reader.ReadToEnd();
				reader.Close();

				_info.Commit = fileContents.Substring(0, 5).ToLower();

			}
			else
			{
				_info.Branch = "disconnected";
				_info.Commit = fileContents.Substring(0, 5).ToLower();
			}
		}
	}
}
