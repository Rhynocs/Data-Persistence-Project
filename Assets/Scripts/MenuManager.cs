using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class MenuManager : MonoBehaviour
{
    [System.Serializable]
    private class Data
    {
        public string name;
        public int score;
    }

    public static MenuManager Instance;

    public string name;
    public string bestScoreName;
    public int bestScore;

    [SerializeField] private Text bestScoreText;

    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        LoadData();
        LoadText();
    }

    public void ReadStringInput(string input)
    {
        name = input;

        if (bestScoreName is null)
            bestScoreName = name;
    }

    public void SaveData()
    {
        Data data = new();
        data.name = bestScoreName;
        data.score = bestScore;

        string json = JsonUtility.ToJson(data);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            Data data = JsonUtility.FromJson<Data>(json);

            bestScoreName = data.name;
            bestScore = data.score;
        }  
    }

    private void LoadText()
    {
        bestScoreText.text = "Best Score : " + bestScoreName + " : " + bestScore;
    }

    public void LoadScene()
    {
        SceneManager.LoadScene(1);
    }

    public void Exit()
    {
        #if UNITY_EDITOR
            EditorApplication.ExitPlaymode();
        #else
            Application.Quit();
        #endif
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
