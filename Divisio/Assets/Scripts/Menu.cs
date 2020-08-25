using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class Menu : MonoBehaviour
{
    delegate void func();
    private int curCamp, curLvl, prgCamp, prgLvl;

    void Start() {
        SetLang();
        if (!PlayerPrefs.HasKey("curLvl")) {
            NewGame();
        }
    }

    public void SetLang() {
        GameObject.Find("StandartButton").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.play;
        GameObject.Find("StandartButton (1)").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.campaigns;
        GameObject.Find("StandartButton (2)").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.exit;
        GameObject.Find("StandartButton (3)").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.settings;
    }

    public void Play() {
        AnimateAll(new func(PlayContinue));
    }

    public void PlayContinue() { 
        UpdateValues();
        if (curLvl != 1) {
            SceneManager.LoadScene("GameScene");
        } else {
            SceneManager.LoadScene("TutorialScene");
        }
    }

    public void Exit() {
        AnimateAll(new func(ExitContinue));
    }

    public void ExitContinue() {
        StartCoroutine(AnimateNormal("UpBlackPanel", "UpBlackPanel"));
        StartCoroutine(AnimateNormal("DownBlackPanel", "DownBlackPanel", new func(ExitContinue1), true));
    }

    public void ExitContinue1() {
        Application.Quit();
    }

    public void Levels() {
        AnimateAll(new func(LevelsContinue));
    }

    public void LevelsContinue() {
        SceneManager.LoadScene("ChooseCampaignScene");
    }

    public void Settings() {
        AnimateAll(new func(SettingsContinue));
    }

    public void SettingsContinue() {
        SceneManager.LoadScene("SettingsScene");
    }

    public void NewGame() {
        PlayerPrefs.SetInt("curCamp", 1);
        PlayerPrefs.SetInt("curLvl", 1);
        PlayerPrefs.SetInt("prgCamp", 1);
        PlayerPrefs.SetInt("prgLvl", 1);
    }

    public void UpdateValues() {
        curCamp = PlayerPrefs.GetInt("curCamp");
        curLvl = PlayerPrefs.GetInt("curLvl");
        prgCamp = PlayerPrefs.GetInt("prgCamp");
        prgLvl = PlayerPrefs.GetInt("prgLvl");
    }

    private void AnimateAll(func cont) {
        StartCoroutine(Animate("StandartButton", "PlayButton"));
        StartCoroutine(Animate("StandartButton (2)", "UpButton"));
        StartCoroutine(Animate("StandartButton (1)", "CampaignsButton"));
        StartCoroutine(Animate("StandartButton (3)", "Settings"));
        StartCoroutine(Animate("GameText", "GameText"));
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

    private IEnumerator AnimateNormal(string obj, string an, func cont = null, bool k = false) {
        Animation anim = GameObject.Find(obj).GetComponent<Animation>();
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