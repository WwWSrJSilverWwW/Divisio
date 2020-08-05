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
    List<string> campaigns;

    public void ButtonClick(int c, int l) {
        UpdateValues();

        string file = "Assets/current.txt";
        StreamWriter writer = new StreamWriter(file);
        writer.Write("currentCampaign:" + c + ";\ncurrentLevel:" + l + ";\nprogressCampaign:" + prgCamp + ";\nprogressLevel:" + prgLvl + ";\nend,of,file.");
        writer.Close();

        SceneManager.LoadScene("GameScene");
    }

    void Start() {
        UpdateValues();

        string file = "Assets/openCampaign.txt";
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
            levelButton = new GameObject("LevelButton" + i);

            levelButton.AddComponent<CanvasRenderer>();
            levelButton.AddComponent<RectTransform>();
            levelButton.AddComponent<Image>();
            levelButton.transform.SetParent(Canvas.transform, false);
            
            levelButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-140 + 140 * x, 240 + 120 * y);
            levelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(80, 80);

            textLevelButton = new GameObject("TextLevelButton" + i);
            textLevelButton.AddComponent<Text>();
            textLevelButton.GetComponent<Text>().text = curCamp + "-" + i;
            textLevelButton.GetComponent<Text>().font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
            textLevelButton.GetComponent<Text>().fontStyle = FontStyle.Bold;
            textLevelButton.GetComponent<Text>().fontSize = 40;
            textLevelButton.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
            textLevelButton.transform.SetParent(levelButton.transform, false);

            if (i <= prgLvl && curCamp == prgCamp || curCamp < prgCamp)
            {
                levelButton.AddComponent<Button>();
                levelButton.GetComponent<Image>().color = new Color(0.86f, 0.86f, 0.86f, 1f);
                textLevelButton.GetComponent<Text>().color = new Color(0.47f, 0.47f, 0.47f, 1f);
                levelButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(r, v));
            }
            else
            {
                levelButton.GetComponent<Image>().color = new Color(0.47f, 0.47f, 0.47f, 1f);
                textLevelButton.GetComponent<Text>().color = new Color(0.86f, 0.86f, 0.86f, 1f);
            }

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
        string file = "Assets/current.txt";

        StreamReader reader = new StreamReader(file);
        string text = reader.ReadToEnd();
        curCamp = int.Parse(text.Split(new char[] { ';' })[0].Split(new char[] { ':' })[1]);
        curLvl = int.Parse(text.Split(new char[] { ';' })[1].Split(new char[] { ':' })[1]);
        prgCamp = int.Parse(text.Split(new char[] { ';' })[2].Split(new char[] { ':' })[1]);
        prgLvl = int.Parse(text.Split(new char[] { ';' })[3].Split(new char[] { ':' })[1]);
        reader.Close();

        TextAsset txt = (TextAsset)Resources.Load("Levels/campaigns", typeof(TextAsset));
        campaigns = new List<string>(txt.text.Split(';'));
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