using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    [SerializeField] private string fileName; 

    private GameData gameData;

    private List<ISaveManager> saveManagers;
    private FileDataHandler dataHandler;

    [ContextMenu("删除保存的游戏数据")]
    public void DeleteSaveData()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataHandler.Delete();
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        saveManagers = FindAllSavaManagers();
        LoadGame();
    }

    public void NewGame()
    {
        Debug.Log("新建游戏");
        gameData = new GameData();
    }

    public void LoadGame()
    {
        gameData = dataHandler.Load();

        if(this.gameData == null)
        {
            Debug.Log("没有保存的游戏");
            NewGame();
        }

        foreach (ISaveManager saveManager in saveManagers)
        {
            saveManager.LoadData(gameData);
        }

        Debug.Log("数据已加载");
    }

    public void SaveGame()
    {
        foreach(ISaveManager savaManager in saveManagers)
        {
            savaManager.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
        Debug.Log("保存游戏");
    }

    public bool HasSaveData()
    {
        Debug.Log(dataHandler == null);
        if(dataHandler.Load() != null)
        {
            return true;
        }
        return false;
    }


    private void OnApplicationQuit()
    {
        SaveGame();
    }

    private List<ISaveManager> FindAllSavaManagers()
    {
        IEnumerable<ISaveManager> saveManagers = FindObjectsOfType<MonoBehaviour>(true).OfType<ISaveManager>();
        return new List<ISaveManager>(saveManagers);
    }
}
