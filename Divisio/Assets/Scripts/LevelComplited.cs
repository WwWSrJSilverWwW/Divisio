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
    private int curCamp, curLvl, prgCamp, prgLvl, stopEndOfLevels;
    private int c0, l0, c1, l1;
    private string file;

    void Start() {
        file = Application.persistentDataPath + "/current.txt";
    }

    public void NextLevel() {
        UpdateValues();
        stopEndOfLevels = stopEnd(curCamp);
        if (curLvl != stopEndOfLevels) {
            c0 = curCamp;
            l0 = curLvl + 1;
        }
        else {
            c0 = curCamp + 1;
            l0 = 1;
        }
        if (c0 > prgCamp) {
            c1 = c0;
            l1 = l0;
        }
        else if (c0 == prgCamp) {
            c1 = c0;
            l1 = Mathf.Max(l0, prgLvl);
        }
        else {
            c1 = prgCamp;
            l1 = prgLvl;
        }
        if (curLvl == stopEndOfLevels && curCamp == 4) { 
            SceneManager.LoadScene("EndScene");
        }
        else {
            StreamWriter writer = new StreamWriter(file);
            writer.Write("currentCampaign:" + c0 + ";\ncurrentLevel:" + l0 + ";\nprogressCampaign:" + c1 + ";\nprogressLevel:" + l1 + ";\nend,of,file.");
            writer.Close();
            SceneManager.LoadScene("GameScene");
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
}
