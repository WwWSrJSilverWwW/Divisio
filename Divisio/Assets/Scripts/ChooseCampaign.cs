using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class ChooseCampaign : MonoBehaviour
{
    public int nowCamp;
    private int curCamp, curLvl, prgCamp, prgLvl, stopEndOfLevels;
    private List<string> campaigns;
    private List<string> campNames = new List<string>();
    private GameObject levelButton, textLevelButton;
    private string platform;

    void Start() {
        if (Application.platform == RuntimePlatform.Android) {
            platform = Application.persistentDataPath;
        } else {
            platform = "Assets";
        }
        GameObject Canvas = GameObject.Find("Canvas");
        UpdateValues(); 
        
        campNames.Add("Find your path");
        campNames.Add("White and black");

        int x = 0;

        for (int i = 1; i <= campNames.Count; i++)
        {
            int v = i;
                
            if (v <= prgCamp) {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/StandartButton")) as GameObject;
                levelButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(v));
            } else {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/CampaignClosedButton")) as GameObject;
            }

            levelButton.transform.SetParent(Canvas.transform, false);

            levelButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(0, 392 - 160 * x);
            levelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(416, 128);

            textLevelButton = levelButton.transform.GetChild(0).gameObject;
            textLevelButton.GetComponent<Text>().text = campNames[i - 1];
            textLevelButton.GetComponent<Text>().fontSize = 40;

            if (i <= prgCamp) {
                levelButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(v));
            }
            else { 
            
            }

            x++;
        }
    }

    public void ButtonClick(int camp) {
        string file = platform + "/openCampaign.txt";
        StreamWriter writer = new StreamWriter(file);
        writer.Write(camp);
        writer.Close();

        SceneManager.LoadScene("ChooseLevelScene");
    }

    public void Menu() {
        SceneManager.LoadScene("MenuScene");
    }

    public void UpdateValues()
    {
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