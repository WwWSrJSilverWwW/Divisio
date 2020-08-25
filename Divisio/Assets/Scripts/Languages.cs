using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Lang
{
    public List<string> campaignsName;
    public List<string> campaignsText;
    public string play;
    public string campaigns;
    public string exit;
    public string presents;
    public string settings;
    public string backToMenu;
    public string levels;
    public string backToCampaigns;
    public string level;
    public string menu;
    public string reset;
    public string levelCompleted;
    public string nextLevel;
    public string fin;
    public string thanks;
    public string gotIt;
    public string newGame;
}

public class Languages : MonoBehaviour
{
    delegate void func();
    public Lang lang = new Lang();
    public List<string> allLangsJson = new List<string>() { "Russian", "English" };
    public bool k1 = false;

    void Awake() {
        SetLang();
    }

    void Start() {
        if (SceneManager.GetActiveScene().name == "SettingsScene") {
            if (GameObject.Find("Dropdown").GetComponent<Dropdown>().value != allLangsJson.IndexOf(PlayerPrefs.GetString("Language"))) {
                GameObject.Find("Dropdown").GetComponent<Dropdown>().value = allLangsJson.IndexOf(PlayerPrefs.GetString("Language"));
            } else {
                k1 = true;
            }
        }
    }

    public void SetLang() {
        if (!PlayerPrefs.HasKey("Language")) {
            if (Application.systemLanguage == SystemLanguage.Russian) {
                PlayerPrefs.SetString("Language", allLangsJson[0]);
            } else {
                PlayerPrefs.SetString("Language", allLangsJson[1]);
            }
        }
        LoadLang();
    }

    public void LoadLang() {
        TextAsset txt = (TextAsset)Resources.Load("Languages/" + PlayerPrefs.GetString("Language"));
        string settings = txt.text;
        lang = JsonUtility.FromJson<Lang>(settings);
    }

    public void ChangedLang() {
        if (k1) {
            PlayerPrefs.SetString("Language", allLangsJson[GameObject.Find("Dropdown").GetComponent<Dropdown>().value]);
            LoadLang();
            AnimateAll(new func(ChangedLangContinue));
        } else {
            k1 = true;
        }
    }

    public void ChangedLangContinue() {
        SceneManager.LoadScene("SettingsScene");
    }

    private void AnimateAll(func cont) {
        StartCoroutine(Animate("StandartButton", "UpButton"));
        StartCoroutine(Animate("StandartButton (1)", "PlayButton"));
        StartCoroutine(Animate("Dropdown", "CampaignsButton"));
        StartCoroutine(Animate("Panel", "DownButton", cont, true));
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