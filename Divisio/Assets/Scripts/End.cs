using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class End : MonoBehaviour
{
    delegate void func();

    void Start() {
        SetBlacks();
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
        GameObject.Find("Text").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.thanks;
        GameObject.Find("StandartButton").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.backToMenu;
        GameObject.Find("EndText").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.fin;
        GameObject.Find("IdeaText").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.idea;
        GameObject.Find("MadeByText").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.madeBy;
        GameObject.Find("ThanksToText").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.thanksTo;
    }

    public void Menu() {
        AnimateAll(new func(MenuContinue));
    }

    public void MenuContinue() {
        SceneManager.LoadScene("MenuScene");
    }
    
    private void AnimateAll(func cont) {
        StartCoroutine(Animate("StandartButton", "UpButtonEnd"));
        StartCoroutine(Animate("Text", "EndText", cont, true));
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
