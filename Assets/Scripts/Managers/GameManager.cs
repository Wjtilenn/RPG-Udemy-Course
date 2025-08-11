using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour, ISaveManager
{
    public static GameManager instance;

    [SerializeField] private CheckPoint[] checkPoints;
    private CheckPoint lastCheckPoint;

    private void Awake()
    {
        if(instance != null)
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
        checkPoints = FindObjectsOfType<CheckPoint>(true);
    }

    public void RestartScene()
    {
        SaveManager.instance.SaveGame();

        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }

    public void LoadData(GameData _data)
    {
        foreach(KeyValuePair<string, bool> pair in _data.checkPoints)
        {
            foreach(CheckPoint checkPoint in checkPoints)
            {
                if(checkPoint.checkPointID == pair.Key &&  pair.Value)
                {
                    checkPoint.ActivateCheckPoint();
                }
            }
        }

        foreach(CheckPoint checkPoint in checkPoints)
        {
            if(_data.lastCheckPointID == checkPoint.checkPointID)
            {
                PlayerManager.instance.player.transform.position = checkPoint.transform.position;
            }
        }
    }

    public void SaveData(ref GameData _data)
    {
        if(lastCheckPoint != null)_data.lastCheckPointID = lastCheckPoint.checkPointID;

        _data.checkPoints.Clear();

        foreach(CheckPoint checkPoint in checkPoints)
        {
            _data.checkPoints.Add(checkPoint.checkPointID, checkPoint.activated);
        }
    }

    public void ActiveCheckPoint(CheckPoint _checkPoint)
    {
        lastCheckPoint = _checkPoint;
        SaveManager.instance.SaveGame();
    }

    public void PauseGame(bool _pause)
    {
        if (_pause) Time.timeScale = 0;
        else Time.timeScale = 1;
    }

}
