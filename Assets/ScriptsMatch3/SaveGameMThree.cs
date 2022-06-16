using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SaveGameMThree : MonoBehaviour
{
    public GameObject Board;
 
    private Save save = new Save();

    private string path;
 
    private void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "SaveMatch3.json");

        if (File.Exists(path))
        {   
            save = JsonUtility.FromJson<Save>(File.ReadAllText(path));
            
            Board.GetComponent<GameBoard>().Record = save.Record;
        }
    }

    // Method for saving all parametrs.
    private void SaveAll()
    {
        save.Record = Board.GetComponent<GameBoard>().Record;
    }

    private void OnApplicationPause(bool pause)
    {
        if (pause)
        {   
            SaveAll();
            File.WriteAllText(path, JsonUtility.ToJson(save));
        }   
    }

    private void OnDisable()
    {
        SaveAll();
        File.WriteAllText(path, JsonUtility.ToJson(save));
    }
}

// Class of what we need to save.
[Serializable]
public class Save
{
    public int Record = 0;
}