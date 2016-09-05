using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace LevelCreator {

	public static class EditorUtils {

		public static T CreateAsset<T> (string path) where T : ScriptableObject
		{
			T dataClass = (T)ScriptableObject.CreateInstance<T> ();
			AssetDatabase.CreateAsset (dataClass, path);
			AssetDatabase.Refresh ();
			AssetDatabase.SaveAssets ();
			return dataClass;
		}

	}

}