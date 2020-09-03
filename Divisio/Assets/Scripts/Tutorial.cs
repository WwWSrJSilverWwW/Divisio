using System;
using System.Linq;
using System.Text;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    delegate void func();
    private int curCamp, curLvl, prgCamp, prgLvl;

    void Start() {
        SetBlacks();
        UpdateValues();
        GameObject Canvas = GameObject.Find("Canvas");
        GameObject t = Instantiate(Resources.Load("Prefabs/Tutorials/" + curCamp)) as GameObject;
        t.transform.SetParent(Canvas.transform, false);
        t.transform.SetSiblingIndex(0);
        t.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonClick());
        SetLang();
    }

    void Update() {
    }

    public void SetBlacks() { 
        if (Screen.width > Screen.height) {
            GameObject.Find("BlackPanel1").GetComponent<RectTransform>().anchoredPosition = new Vector2(-2360, 0);
            GameObject.Find("BlackPanel2").GetComponent<RectTransform>().anchoredPosition = new Vector2(2360, 0);
        }
    }

    public void SetLang() {
        GameObject.Find("StandartButton").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.gotIt;
        GameObject.Find("Panel").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.campaignsName[curCamp - 1];
        GameObject.Find("Text").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.campaignsText[curCamp - 1];
    }

    public void UpdateValues() {
        curCamp = PlayerPrefs.GetInt("curCamp");
        curLvl = PlayerPrefs.GetInt("curLvl");
        prgCamp = PlayerPrefs.GetInt("prgCamp");
        prgLvl = PlayerPrefs.GetInt("prgLvl");
    }

    public void ButtonClick() {
        AnimateAll(new func(ButtonClickContinue));
    }

    public void ButtonClickContinue() {
        SceneManager.LoadScene("GameScene");
    }

    private void AnimateAll(func cont) {
        StartCoroutine(Animate("Text", "TutorialText"));
        StartCoroutine(Animate("StandartButton", "UpButton"));
        StartCoroutine(Animate("Panel", "TutorialCampText", cont, true));
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
