using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class Menu : MonoBehaviour
{
    delegate void func();
    private string file;
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
        file = platform + "/current.txt";
        if (!File.Exists(file)) {
            File.CreateText(file).Close();
            NewGame();
        }
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
        Application.Quit();
    }

    public void Levels() {
        AnimateAll(new func(LevelsContinue));
    }

    public void LevelsContinue() {
        SceneManager.LoadScene("ChooseCampaignScene");
    }

    public void NewGame() {
        StreamWriter writer = new StreamWriter(file);
        writer.Write("currentCampaign:1;\ncurrentLevel:1;\nprogressCampaign:1;\nprogressLevel:1;\nend,of,file.");
        writer.Close();
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

    private void AnimateAll(func cont) {
        StartCoroutine(Animate("StandartButton", "PlayButton"));
        StartCoroutine(Animate("StandartButton (2)", "UpButton"));
        StartCoroutine(Animate("StandartButton (1)", "CampaignsButton"));
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
}