using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Format for storage (without brakets):
// [Name]:[Score];[Name]:[Score]; . . .
[System.Serializable]
class ScoreData
{
    public string data;
}

public struct PlayerData
{
    public string name;
    public int score;
}

public class ScoreComparer : IComparer
{
    public int Compare(object a, object b)
    {
        PlayerData x = (PlayerData) a;
        PlayerData y = (PlayerData) b;

        if (x.score == y.score)
            return 0;
        if (x.score < y.score)
            return -1;

        return 1;
    }
}

public class ScoreScript : MonoBehaviour {

    private List<PlayerData> scoreTable = new List<PlayerData>();
    private ScoreData storage = new ScoreData();
    private int score = 0;
    private string playerName = "";

	// Use this for initialization
	void Start () {
        Load();
	}
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Points: " + score);
        }
	}

    public void AddPlayer()
    {

    }

    public void Save()
    {

    }

    public void AddCollectable()
    {

    }

    public void AddToScore(int value)
    {
        score += value;
    }

    public void Load()
    {
        /*
        if (File.Exists(Application.persistentDataPath + "/scoreTable.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            // . . .
        }
        else
        {
            storage.data = "";
        }
        */
    }
}
