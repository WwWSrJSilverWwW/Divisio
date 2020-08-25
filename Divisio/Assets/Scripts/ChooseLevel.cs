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
    delegate void func();
    private GameObject levelButton;
    private GameObject textLevelButton;
    private int curCamp, curLvl, prgCamp, prgLvl, stopEndOfLevels;
    private List<string> campaigns;
    private int l1;

    public void ButtonClick(int c, int l) {
        UpdateValues();
        PlayerPrefs.SetInt("curCamp", c);
        PlayerPrefs.SetInt("curLvl", l);
        l1 = l;
        AnimateAll(new func(ButtonClickContinue));
    }

    public void ButtonClickContinue() {
        if (l1 != 1) {
            SceneManager.LoadScene("GameScene");
        } else {
            SceneManager.LoadScene("TutorialScene");
        }
    }

    void Start() {
        SetBlacks();
        SetLang();
        ScrollRect scroll = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        UpdateValues();

        curCamp = int.Parse(PlayerPrefs.GetString("OpenCampaign"));
        
        stopEndOfLevels = stopEnd(curCamp);
        
        GameObject Canvas = GameObject.Find("Canvas");

        int x = 0;
        int y = 0;
        GameObject three = null;
        for (int i = 1; i <= stopEndOfLevels; i++) {
            int v = i;
            int r = curCamp;

            if ((v - 1) % 3 == 0) {
                int g = (v - 1) / 3 + 1;
                three = new GameObject("LevelThree" + g);
                three.AddComponent<RectTransform>();
                three.transform.SetParent(scroll.content, false);
            }
            
            if (i <= prgLvl && curCamp == prgCamp || curCamp < prgCamp) {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/LevelOpenedButton")) as GameObject;
                levelButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(r, v));
            } else {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/LevelClosedButton")) as GameObject;
            }
            levelButton.name = "LevelButton" + v;

            levelButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-208 + 208 * x, 0);

            levelButton.transform.SetParent(three.transform, false);

            textLevelButton = levelButton.transform.GetChild(0).gameObject;
            textLevelButton.GetComponent<Text>().text = curCamp + "-" + i;
            textLevelButton.GetComponent<Text>().fontSize = 50;

            x++;
            if (x == 3) 
            {
                x = 0;
                y--;
            }
        }
    }

    public void SetBlacks() { 
        if (Screen.width > Screen.height) {
            GameObject.Find("BlackPanel1").GetComponent<RectTransform>().anchoredPosition = new Vector2(-2360, 0);
            GameObject.Find("BlackPanel2").GetComponent<RectTransform>().anchoredPosition = new Vector2(2360, 0);
        }
    }

    public void SetLang() {
        GameObject.Find("StandartButton").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.backToCampaigns;
        GameObject.Find("Panel").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.levels;
    }

    public void Campaigns() {
        AnimateAll(new func(CampaignsContinue));
    }

    public void CampaignsContinue() {
        SceneManager.LoadScene("ChooseCampaignScene");
    }

    public void UpdateValues() {
        curCamp = PlayerPrefs.GetInt("curCamp");
        curLvl = PlayerPrefs.GetInt("curLvl");
        prgCamp = PlayerPrefs.GetInt("prgCamp");
        prgLvl = PlayerPrefs.GetInt("prgLvl");
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

    private void AnimateAll(func cont) {
        for (int i = 1; i <= stopEndOfLevels; i++) {
            StartCoroutine(Animate("LevelButton" + i, "AppearWithScale"));
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