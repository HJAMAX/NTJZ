using UnityEngine;
using System.IO;

public static class FileIOUtil
{
    public static void Write(string path, string dataText)
    {
#if UNITY_EDITOR
        path = Application.dataPath + path;
        File.WriteAllText(path, dataText);
        UnityEditor.AssetDatabase.Refresh();
#endif
        /*
#if UNITY_ANDROID
        path = Application.persistentDataPath + "/Resources/Data/my_info";
        File.WriteAllText(path, dataText);
#endif
    */
    }

    public static string Read(string path)
    {
        return Resources.Load<TextAsset>(path).text;
    }
}
