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
    delegate void func();
    public int nowCamp;
    private int curCamp, curLvl, prgCamp, prgLvl, stopEndOfLevels;
    private List<string> campaigns;
    private List<string> campNames = new List<string>();
    private GameObject levelButton, textLevelButton;
    private string platform;

    void Start() {
        ScrollRect scroll = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        if (Application.platform == RuntimePlatform.Android) {
            platform = Application.persistentDataPath;
        } else {
            platform = "Assets";
        }
        GameObject Canvas = GameObject.Find("Canvas");
        UpdateValues(); 
        
        campNames.Add("Find your path");
        campNames.Add("White and black");
        campNames.Add("Like in 'Snake'");
        campNames.Add("Delete it");

        int x = 0;

        for (int i = 1; i <= campNames.Count; i++) {
            int v = i;
                
            if (v <= prgCamp) {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/CampaignOpenedButton")) as GameObject;
                levelButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(v));
            } else {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/CampaignClosedButton")) as GameObject;
            }
            levelButton.name = "CampaignButton" + v;

            levelButton.transform.SetParent(scroll.content, false);

            levelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(416, 128);

            textLevelButton = levelButton.transform.GetChild(0).gameObject;
            textLevelButton.GetComponent<Text>().text = campNames[i - 1];
            textLevelButton.GetComponent<Text>().fontSize = 40;

            if (v <= prgCamp) {
                levelButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(v));
            }

            x++;
        }
    }

    public void ButtonClick(int camp) {
        string file = platform + "/openCampaign.txt";
        StreamWriter writer = new StreamWriter(file);
        writer.Write(camp);
        writer.Close();
        AnimateAll(new func(ButtonClickContinue));
    }

    public void ButtonClickContinue() {
        SceneManager.LoadScene("ChooseLevelScene");
    }

    public void Menu() {
        AnimateAll(new func(MenuContinue));
    }

    public void MenuContinue() {
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

    private void AnimateAll(func cont) {
        for (int i = 1; i <= campNames.Count; i++) {
            StartCoroutine(Animate("CampaignButton" + i, "AppearWithScale"));
        }
        StartCoroutine(Animate("StandartButton", "UpButton"));
        StartCoroutine(Animate("Panel", "DownButton", cont, true));
    }

    private IEnumerator Animate(string obj, string an, func cont = null, bool k = false) {
        Animation anim = GameObject.Find(obj).GetComponent<Animation>();
        anim[an].speed = -1;
        anim[an].time = anim[an].length;
        anim.CrossFade(an);
        if (k) {
            while (anim.isPlaying) {
                yield return null;
            }
            yield return new WaitForSeconds(0.5f);
            cont.Invoke();
        }
    }
}