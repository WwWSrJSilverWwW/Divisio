using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System;

public class LevelComplited : MonoBehaviour
{
    delegate void func();
    private int curCamp, curLvl, prgCamp, prgLvl, stopEndOfLevels;
    private int c0, l0, c1, l1;
    private string file;
    private int st = 4;

    void Start() {
        SetLang();
    }

    public void SetLang() {
        GameObject.Find("StandartButton").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.nextLevel;
        GameObject.Find("StandartButton (1)").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.campaigns;
        GameObject.Find("StandartButton (2)").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.backToMenu;
        GameObject.Find("LevelCompletedText").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.levelCompleted;
    }

    void Update() { 
        if (Input.GetKeyDown(KeyCode.Return)) {
            NextLevel();
        }
    }

    public void NextLevel() {
        UpdateValues();
        stopEndOfLevels = stopEnd(curCamp);
        AnimateAll(new func(NextLevelContinue));
    }

    public void NextLevelContinue() { 
        if (curLvl == stopEndOfLevels && curCamp == st) { 
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
        curCamp = PlayerPrefs.GetInt("curCamp");
        curLvl = PlayerPrefs.GetInt("curLvl");
        prgCamp = PlayerPrefs.GetInt("prgCamp");
        prgLvl = PlayerPrefs.GetInt("prgLvl");
    }

    public void Menu() {
        AnimateAll(new func(MenuContinue));
    }

    public void MenuContinue() {
        SceneManager.LoadScene("MenuScene");
    }

    public void ChooseCampaign() {
        AnimateAll(new func(ChooseCampaignContinue));
    }

    public void ChooseCampaignContinue() {
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

    private void AnimateAll(func cont) {
        StartCoroutine(Animate("StandartButton (2)", "UpButton"));
        StartCoroutine(Animate("StandartButton (1)", "CampaignsButton"));
        StartCoroutine(Animate("StandartButton", "PlayButton"));
        StartCoroutine(Animate("LevelCompletedText", "GameText"));
        StartCoroutine(Animate("Panel", "DownPanel", cont, true));
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
