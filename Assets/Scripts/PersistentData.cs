using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance;

    public Stack<int> previousScenes;
    public bool readOnStart;

    public string Name = ""; // Only for savefiles, do not modify in game
    public int HP = 100; // Same as above
    public long Gold = 0;
    public PlayerStats Stats; // This should ideally be better than this but eh
    public SerializableList<InventoryItem> Inventory;

    private const string _jsonFilename = "save.json";
    private string _jsonPath;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(Instance);
        _jsonPath = Application.persistentDataPath + "/" + _jsonFilename;
        if (readOnStart)
        {
            ReadFromJson();
        }
        if (previousScenes == null) InitSceneStack();
    }

    private void InitSceneStack()
    {
        previousScenes = new Stack<int>();
    }

    public void QuickSave()
    {
        if (Game.Instance != null)
        {
            Name = Game.Instance.Name;
            HP = Game.Instance.health;
            Gold = Game.Instance.gold;
        }
        WriteToJson();
    }

    public void WriteToJson()
    {
        DataObject data = new DataObject(Name, HP, Gold, Stats.Str, Stats.Dex, Stats.Int, Stats.LeftoverPoints, Inventory);
        string jsonData = JsonUtility.ToJson(data);
        File.WriteAllText(_jsonPath, jsonData);
    }

    public void ClearJsonSave()
    {
        Name = "";
        HP = 100;
        Gold = 0;
        Stats = new PlayerStats();
        Inventory = new SerializableList<InventoryItem>();
        File.WriteAllText(_jsonPath, string.Empty);
    }

    public void ReadFromJson()
    {
        if (File.Exists(_jsonPath))
        {
            string jsonData = File.ReadAllText(_jsonPath);
            DataObject data = JsonUtility.FromJson<DataObject>(jsonData);
            if (data == null) return;
            Name = data.Name;
            HP = data.HP;
            Gold = data.Gold;
            Stats = new PlayerStats(data.Str, data.Dex, data.Int, data.LeftoverPoints);
            Inventory = data.Inventory;
        }
        else
        {
            ClearJsonSave();
        }
    }

    public bool FileExistsAndNotEmpty()
    {
        if (!File.Exists(_jsonPath))
            return false;
        var info = new FileInfo(_jsonPath);
        return info.Length != 0;
    }
}
