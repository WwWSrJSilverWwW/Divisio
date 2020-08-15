using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System;

public class ChooseLevel : MonoBehaviour
{
    private GameObject levelButton;
    private GameObject textLevelButton;
    private int curCamp, curLvl, prgCamp, prgLvl, stopEndOfLevels;
    private List<string> campaigns;
    private string platform;

    public void ButtonClick(int c, int l) {
        UpdateValues();

        string file = platform + "/current.txt";
        StreamWriter writer = new StreamWriter(file);
        writer.Write("currentCampaign:" + c + ";\ncurrentLevel:" + l + ";\nprogressCampaign:" + prgCamp + ";\nprogressLevel:" + prgLvl + ";\nend,of,file.");
        writer.Close();
        if (l != 1) {
            SceneManager.LoadScene("GameScene");
        } else {
            SceneManager.LoadScene("TutorialScene");
        }
    }

    void Start() {
        if (Application.platform == RuntimePlatform.Android) {
            platform = Application.persistentDataPath;
        } else {
            platform = "Assets";
        }
        UpdateValues();

        string file = platform + "/openCampaign.txt";
        StreamReader reader = new StreamReader(file);
        curCamp = int.Parse(reader.ReadToEnd());
        reader.Close();
        
        stopEndOfLevels = stopEnd(curCamp);
        
        GameObject Canvas = GameObject.Find("Canvas");

        int x = 0;
        int y = 0;
        for (int i = 1; i <= stopEndOfLevels; i++)
        {
            int v = i;
            int r = curCamp;

            if (i <= prgLvl && curCamp == prgCamp || curCamp < prgCamp) {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/StandartButton")) as GameObject;
                levelButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(r, v));
            } else {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/LevelClosedButton")) as GameObject;
            }
            levelButton.transform.SetParent(Canvas.transform, false);
            levelButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-140 + 140 * x, 200 + 120 * y);
            levelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);

            textLevelButton = levelButton.transform.GetChild(0).gameObject;
            textLevelButton.GetComponent<Text>().text = curCamp + "-" + i;
            textLevelButton.GetComponent<Text>().fontSize = 35;

            x++;
            if (x == 3) 
            {
                x = 0;
                y--;
            }
        }
    }

    public void Menu() {
        SceneManager.LoadScene("MenuScene");
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

    public int stopEnd(int cc) {
        int i = 1;
        try
        {
            while (i < 100)
            {
                TextAsset txt = (TextAsset)Resources.Load("Levels/" + cc + "/" + i, typeof(TextAsset));
                string s = txt.text;
                i++;
            }
        }
        catch (Exception)
        {
            return i - 1;
            throw;
        }
        return 0;
    }
}