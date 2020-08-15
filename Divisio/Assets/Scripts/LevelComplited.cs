using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System;

public class LevelComplited : MonoBehaviour
{
    private int curCamp, curLvl, prgCamp, prgLvl, stopEndOfLevels;
    private int c0, l0, c1, l1;
    private string file;
    private string platform;

    void Start() {
        if (Application.platform == RuntimePlatform.Android) {
            platform = Application.persistentDataPath;
        } else {
            platform = "Assets";
        }
        file = platform + "/current.txt";
    }

    void Update() { 
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton7)) {
            NextLevel();
        }
    }

    public void NextLevel() {
        UpdateValues();
        stopEndOfLevels = stopEnd(curCamp);
        if (curLvl == stopEndOfLevels && curCamp == 2) { 
            SceneManager.LoadScene("EndScene");
        } else {
            if (curLvl != 1) {
                SceneManager.LoadScene("GameScene");
            } else {
                SceneManager.LoadScene("TutorialScene");
            }
        }
    }

    public void UpdateValues() {
        StreamReader reader = new StreamReader(file);
        string text = reader.ReadToEnd();
        curCamp = int.Parse(text.Split(new char[] { ';' })[0].Split(new char[] { ':' })[1]);
        curLvl = int.Parse(text.Split(new char[] { ';' })[1].Split(new char[] { ':' })[1]);
        prgCamp = int.Parse(text.Split(new char[] { ';' })[2].Split(new char[] { ':' })[1]);
        prgLvl = int.Parse(text.Split(new char[] { ';' })[3].Split(new char[] { ':' })[1]);
        reader.Close();
    }

    public void Menu() {
        SceneManager.LoadScene("MenuScene");
    }

    public void ChooseCampaign() {
        SceneManager.LoadScene("ChooseCampaignScene");
    }

    public int stopEnd(int cc) {
        int i = 1;
        try {
            while (true) {
                TextAsset txt = (TextAsset)Resources.Load("Levels/" + cc + "/" + i, typeof(TextAsset));
                string s = txt.text;
                i++;
            }
        }
        catch (Exception) {
            return i - 1;
            throw;
        }
    }
}
