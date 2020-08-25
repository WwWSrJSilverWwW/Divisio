using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Preview : MonoBehaviour
{
    void Start() {
        SetLang();
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
