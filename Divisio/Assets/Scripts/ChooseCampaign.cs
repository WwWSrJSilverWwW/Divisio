using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class ChooseCampaign : MonoBehaviour
{
    delegate void func();
    public int nowCamp;
    private int curCamp, curLvl, prgCamp, prgLvl, stopEndOfLevels;
    private List<string> campaigns;
    private List<string> campNames = new List<string>();
    private GameObject levelButton, textLevelButton;

    void Start() {
        SetBlacks();
        SetLang();
        ScrollRect scroll = GameObject.Find("Scroll View").GetComponent<ScrollRect>();
        GameObject Canvas = GameObject.Find("Canvas");
        UpdateValues(); 
        
        for (int i = 0; i < GameObject.Find("Main Camera").GetComponent<Languages>().lang.campaignsName.Count; i++) {
            campNames.Add(GameObject.Find("Main Camera").GetComponent<Languages>().lang.campaignsName[i]);
        }

        int x = 0;

        for (int i = 1; i <= campNames.Count; i++) {
            int v = i;
                
            if (v <= prgCamp) {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/CampaignOpenedButton")) as GameObject;
                levelButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(v));
            } else {
                levelButton = Instantiate(Resources.Load("Prefabs/Objects/CampaignClosedButton")) as GameObject;
            }
            levelButton.name = "CampaignButton" + v;

            levelButton.transform.SetParent(scroll.content, false);

            levelButton.GetComponent<RectTransform>().sizeDelta = new Vector2(416, 128);

            textLevelButton = levelButton.transform.GetChild(0).gameObject;
            textLevelButton.GetComponent<Text>().text = campNames[i - 1];
            textLevelButton.GetComponent<Text>().fontSize = 40;

            if (v <= prgCamp) {
                levelButton.GetComponent<Button>().onClick.AddListener(() => ButtonClick(v));
            }

            x++;
        }
    }

    public void SetBlacks() { 
        if (Screen.width > Screen.height) {
            GameObject.Find("BlackPanel1").GetComponent<RectTransform>().anchoredPosition = new Vector2(-2360, 0);
            GameObject.Find("BlackPanel2").GetComponent<RectTransform>().anchoredPosition = new Vector2(2360, 0);
        }
    }
    
    public void SetLang() {
        GameObject.Find("StandartButton").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.backToMenu;
        GameObject.Find("Panel").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.campaigns;
    }

    public void ButtonClick(int camp) {
        PlayerPrefs.SetString("OpenCampaign", camp.ToString());
        AnimateAll(new func(ButtonClickContinue));
    }

    public void ButtonClickContinue() {
        SceneManager.LoadScene("ChooseLevelScene");
    }

    public void Menu() {
        AnimateAll(new func(MenuContinue));
    }

    public void MenuContinue() {
        SceneManager.LoadScene("MenuScene");
    }

    public void UpdateValues() {
        curCamp = PlayerPrefs.GetInt("curCamp");
        curLvl = PlayerPrefs.GetInt("curLvl");
        prgCamp = PlayerPrefs.GetInt("prgCamp");
        prgLvl = PlayerPrefs.GetInt("prgLvl");
    }

    private void AnimateAll(func cont) {
        for (int i = 1; i <= campNames.Count; i++) {
            StartCoroutine(Animate("CampaignButton" + i, "AppearWithScale"));
        }
        StartCoroutine(Animate("StandartButton", "UpButton"));
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