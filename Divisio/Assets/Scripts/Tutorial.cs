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
    private string platform;

    void Start() {
        float ratio = (float)(Screen.height / Screen.width);
        float ortSize = 720f * ratio / 200f;
        Camera.main.orthographicSize = ortSize;
        if (Application.platform == RuntimePlatform.Android) {
            platform = Application.persistentDataPath;
        } else {
            platform = "Assets";
        }
        GameObject Canvas = GameObject.Find("Canvas"); 
        UpdateValues();
        GameObject t = Instantiate(Resources.Load("Prefabs/Tutorials/" + curCamp)) as GameObject;
        t.transform.SetParent(Canvas.transform, false);
        t.transform.SetSiblingIndex(0);
        t.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonClick());
    }

    void Update() {  
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
