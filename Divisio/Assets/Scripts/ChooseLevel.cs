﻿using UnityEngine;
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
    private string platform;
    private int l1;

    public void ButtonClick(int c, int l) {
        UpdateValues();
        string file = platform + "/current.txt";
        StreamWriter writer = new StreamWriter(file);
        writer.Write("currentCampaign:" + c + ";\ncurrentLevel:" + l + ";\nprogressCampaign:" + prgCamp + ";\nprogressLevel:" + prgLvl + ";\nend,of,file.");
        writer.Close();
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
        float ratio = (float)(Screen.height / Screen.width);
        float ortSize = 720f * ratio / 200f;
        Camera.main.orthographicSize = ortSize;
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
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/LevelOpenedButton")) as GameObject;
                levelButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(r, v));
            } else {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/LevelClosedButton")) as GameObject;
            }
            levelButton.name = "LevelButton" + v;

            levelButton.transform.SetParent(Canvas.transform, false);
            levelButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(-208 + 208 * x, 392 + 192 * y);
            levelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(128, 128);

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

    public void Campaigns() {
        AnimateAll(new func(CampaignsContinue));
    }

    public void CampaignsContinue() {
        SceneManager.LoadScene("ChooseCampaignScene");
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