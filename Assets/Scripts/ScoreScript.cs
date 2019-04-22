using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

//Format for storage (without brakets):
// [Name]:[Score]:[Name]:[Score]: . . .
[System.Serializable]
class ScoreData
{
    public string data;
}

public struct PlayerData
{
    public string name;
    public int score;

    public PlayerData(string str, int points)
    {
        name = str;
        score = points;
    }
}

public class ScoreComparer : IComparer<PlayerData>
{
    public int Compare(PlayerData a, PlayerData b)
    {
        if (a.score == b.score)
            return 0;
        if (a.score < b.score)
            return 1;

        return -1;
    }
}

public class ScoreScript : MonoBehaviour {

    public static ScoreScript instance = null;

    private List<PlayerData> scoreTable = new List<PlayerData>();
    private ScoreData storage = new ScoreData();
    [HideInInspector] public int score { get; private set;}
    private string playerName = "";
    private ScoreComparer scoreComparer;
    private const int MAX_NAME_LENGTH = 6;
    private const int SCORE_TABLE_TABS = 8;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != null)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    // Use this for initialization
    void Start () {
        Load();
        score = 0;
        scoreComparer = new ScoreComparer();
	}
	
	// Update is called once per frame
	void Update () {
        
    }

    public void AddPlayer(string name)
    {
        scoreTable.Add(new PlayerData(name, score));
        scoreTable.Sort(scoreComparer);
    }

    public string PrintScores()
    {
        string result = "";

        for(int i = 0; i < scoreTable.Count; i++)
        {
            result += scoreTable[i].name;
            for (int j = 0; j < (MAX_NAME_LENGTH - scoreTable[i].name.Length); j++)
                result += " ";
            for (int k = 1; k <= SCORE_TABLE_TABS; k++)
                result += "\t";
            result += scoreTable[i].score + "\n";
        }

        return result;
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
        scoreTable.Clear();

        if (File.Exists(Application.persistentDataPath + "/scoreTable.dat"))
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/scoreTable.dat", FileMode.Open);
            ScoreData data = (ScoreData) binaryFormatter.Deserialize(file);
            file.Close();

            string[] entries = data.data.Substring(1).Split(':');
            for (int i = 0; i < entries.Length; i += 2)
            {
                scoreTable.Add(new PlayerData(entries[i], int.Parse(entries[i + 1])));
            }
        }
    }

    public void Save()
    {
        BinaryFormatter binaryFormatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/scoreTable.dat");
        string data = "";
        ScoreData scoreData = new ScoreData();

        for(int i = 0; i < scoreTable.Count; i++)
        {
            data += ":" + scoreTable[i].name + ":" + scoreTable[i].score;
        }

        scoreData.data = data;
        binaryFormatter.Serialize(fileStream, scoreData);
        fileStream.Close();
    }
}
