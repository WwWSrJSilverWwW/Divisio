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
    delegate void func();
    private int curCamp, curLvl, prgCamp, prgLvl, stopEndOfLevels;
    private int c0, l0, c1, l1;
    private string file;
    private string platform;
    private int st = 4;

    void Start() {
        float ratio = (float)(Screen.height / Screen.width);
        float ortSize = 720f * ratio / 200f;
        Camera.main.orthographicSize = ortSize;
        if (Application.platform == RuntimePlatform.Android) {
            platform = Application.persistentDataPath;
        } else {
            platform = "Assets";
        }
        file = platform + "/current.txt";
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
        StreamReader reader = new StreamReader(file);
        string text = reader.ReadToEnd();
        curCamp = int.Parse(text.Split(new char[] { ';' })[0].Split(new char[] { ':' })[1]);
        curLvl = int.Parse(text.Split(new char[] { ';' })[1].Split(new char[] { ':' })[1]);
        prgCamp = int.Parse(text.Split(new char[] { ';' })[2].Split(new char[] { ':' })[1]);
        prgLvl = int.Parse(text.Split(new char[] { ';' })[3].Split(new char[] { ':' })[1]);
        reader.Close();
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
        StartCoroutine(Animate("LevelComplitedText", "GameText"));
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
