using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;

public class SaveGameMohjong : MonoBehaviour
{
    public GameObject Board;
 
    private Saves save = new Saves();

    private string path;
 
    private void Start()
    {
        path = Path.Combine(Application.persistentDataPath, "SaveMohjong.json");

        if (File.Exists(path))
        {   
            save = JsonUtility.FromJson<Saves>(File.ReadAllText(path));
            
            Board.GetComponent<GameBoardMohjong>().Record = save.Record;
        }
    }

    // Method for saving all parametrs.
    private void SaveAll()
    {
        save.Record = Board.GetComponent<GameBoardMohjong>().Record;
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
public class Saves
{
    public int Record = 0;
}