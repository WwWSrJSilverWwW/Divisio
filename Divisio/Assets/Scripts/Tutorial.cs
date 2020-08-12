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
    private int curCamp, curLvl, prgCamp, prgLvl;

    void Start() {
        GameObject Canvas = GameObject.Find("Canvas"); 
        UpdateValues();
        GameObject t = Instantiate(Resources.Load("Prefabs/Tutorials/" + curCamp)) as GameObject;
        t.transform.SetParent(Canvas.transform, false);
        t.transform.GetChild(0).gameObject.GetComponent<Button>().onClick.AddListener(() => ButtonClick());
    }

    void Update() {  
    }

    public void UpdateValues() {
        string file = Application.persistentDataPath + "/current.txt";
        StreamReader reader = new StreamReader(file);
        string text = reader.ReadToEnd();
        curCamp = int.Parse(text.Split(new char[] { ';' })[0].Split(new char[] { ':' })[1]);
        curLvl = int.Parse(text.Split(new char[] { ';' })[1].Split(new char[] { ':' })[1]);
        prgCamp = int.Parse(text.Split(new char[] { ';' })[2].Split(new char[] { ':' })[1]);
        prgLvl = int.Parse(text.Split(new char[] { ';' })[3].Split(new char[] { ':' })[1]);
        reader.Close();
    }

    public void ButtonClick() {
        SceneManager.LoadScene("GameScene");
    }
}
