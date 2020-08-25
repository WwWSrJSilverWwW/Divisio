using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class End : MonoBehaviour
{
    delegate void func();

    void Start() {
        SetLang();
    }

    void Update() {
    }

    public void SetLang() {
        GameObject.Find("Text").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.thanks;
        GameObject.Find("StandartButton").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.backToMenu;
        GameObject.Find("EndText").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.fin;
    }

    public void Menu() {
        AnimateAll(new func(MenuContinue));
    }

    public void MenuContinue() {
        SceneManager.LoadScene("MenuScene");
    }
    
    private void AnimateAll(func cont) {
        StartCoroutine(Animate("StandartButton", "UpButton"));
        StartCoroutine(Animate("Text", "EndText"));
        StartCoroutine(Animate("EndText", "GameText"));
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
