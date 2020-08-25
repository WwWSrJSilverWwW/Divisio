using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Preview : MonoBehaviour
{
    void Start() {
        SetLang();
        SetBlacks();
    }

    public void SetBlacks() { 
        if (Screen.width > Screen.height) {
            GameObject.Find("BlackPanel1").GetComponent<RectTransform>().anchoredPosition = new Vector2(-2360, 0);
            GameObject.Find("BlackPanel2").GetComponent<RectTransform>().anchoredPosition = new Vector2(2360, 0);
        }
    }

    public void SetLang() {
        GameObject.Find("Text2").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.presents;
    }

    void Update() {
    }

    public void Continue() {
        SceneManager.LoadScene("MenuScene");
    }
}
