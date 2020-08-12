using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class Menu : MonoBehaviour
{
    private string file;
    private int curCamp, curLvl, prgCamp, prgLvl;
    private string platform;

    void Start() {
        if (Application.platform == RuntimePlatform.Android) {
            platform = Application.persistentDataPath;
        } else {
            platform = "Assets";
        }
        UpdateValues();
        file = platform + "/current.txt";
        if (!File.Exists(file)) {
            File.CreateText(file).Close();
            NewGame();
        }
    }

    public void Play() {
        if (curLvl != 1) {
            SceneManager.LoadScene("GameScene");
        } else {
            SceneManager.LoadScene("TutorialScene");
        }
    }

    public void Exit() {
        Application.Quit();
    }

    public void Levels() {
        SceneManager.LoadScene("ChooseCampaignScene");
    }

    public void NewGame() {
        StreamWriter writer = new StreamWriter(file);
        writer.Write("currentCampaign:1;\ncurrentLevel:1;\nprogressCampaign:1;\nprogressLevel:1;\nend,of,file.");
        writer.Close();
    }

    public void UpdateValues() {
        string file = platform + "/current.txt";
        StreamReader reader = new StreamReader(file);
        string text = reader.ReadToEnd();
        curCamp = int.Parse(text.Split(new char[] { ';' })[0].Split(new char[] { ':' })[1]);
        curLvl = int.Parse(text.Split(new char[] { ';' })[1].Split(new char[] { ':' })[1]);
        prgCamp = int.Parse(text.Split(new char[] { ';' })[2].Split(new char[] { ':' })[1]);
        prgLvl = int.Parse(text.Split(new char[] { ';' })[3].Split(new char[] { ':' })[1]);
        reader.Close();
    }
}