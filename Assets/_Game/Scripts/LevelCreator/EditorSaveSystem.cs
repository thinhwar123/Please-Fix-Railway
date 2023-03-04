using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class EditorSaveSystem
{
    const string SAVEPATH = "_Game/Datas/LevelDatas";

    public static void WriteToFile(string fileName, string json)
    {
        string path = GetFilePath(fileName);
        FileStream fileStream = new FileStream(path, FileMode.Create);

        using (StreamWriter writer = new StreamWriter(fileStream))
        {
            writer.Write(json);
        }
        fileStream.Close();


#if UNITY_EDITOR
        UnityEditor.AssetDatabase.Refresh();
        UnityEditor.AssetDatabase.SaveAssets();
#endif
    }

    public static string ReadFromFile(string fileName)
    {
        string path = GetFilePath(fileName);
        if (File.Exists(path))
        {
            using (StreamReader reader = new StreamReader(path))
            {
                string json = reader.ReadToEnd();
                return json;
            }
        }
        else
        {
            Debug.LogWarning("File not found");
        }

        return "";
    }

    private static string GetFilePath(string fileName)
    {
        return Path.Combine(Application.dataPath, SAVEPATH, fileName);
    }
    public static bool IsSaveExists(string fileName)
    {
        return File.Exists(Path.Combine(Application.dataPath, SAVEPATH, fileName));
    }
}
