using UnityEngine;
using UnityEditor;
using System.Collections;

namespace LevelCreator {

	public static class MenuItems {

		[MenuItem ("Level Creator/New/GameConfiguration")]
		private static void CreateDataGameConfiguration ()
		{
//			string path = EditorUtility.SaveFilePanelInProject (
//				"New GameConfiguration Data",
//				"GameConfiguration",
//				"asset",
//				"Define the name for the GameConfiguration asset");
//			if (path != "") {
//				EditorUtils.CreateAsset<GameConfiguration> (path);
//			}
		}

        [MenuItem("Level Creator/New/AttackActionInfo")]
        private static void CreateAttackActionInfo()
        {
            string path = EditorUtility.SaveFilePanelInProject(
                "New AttackActionInfo Data",
                "AttackActionInfo",
                "asset",
                "Define the name for the AttackActionInfo asset");
            if (path != "")
            {
                EditorUtils.CreateAsset<AttackActionInfo>(path);
            }
        }
    }
}