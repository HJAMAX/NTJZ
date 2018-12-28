using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.IO;

public class MapMenu
{
#if UNITY_EDITOR
    [MenuItem("Chateau/CreateChateauPositions")]
    private static void CreateChateauPositions()
    {
        string assetName = "chateau_positions";
        string assetPath = "Assets";
        string containStr = "chateau_lv";
        ChateauPositions chateauPositions = ScriptableObject.CreateInstance<ChateauPositions>();

        GameObject[] gameObjs = MonoBehaviour.FindObjectsOfType<GameObject>();
        List<Vector3> posList = new List<Vector3>();

        foreach(GameObject gameObj in gameObjs)
        {
            if(gameObj.name.Contains(containStr))
            {
                posList.Add(gameObj.transform.position);
                Debug.Log(gameObj.transform.position);
            }
        }

        chateauPositions.positions = posList.ToArray();
        AssetDatabase.CreateAsset(chateauPositions, Path.Combine(assetPath, assetName) + ".asset");
    }
#endif
}
