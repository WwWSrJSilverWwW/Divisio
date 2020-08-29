using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Settings : MonoBehaviour
{
    delegate void func();

    void Start() {
        SetBlacks();
        SetLang();
        SetMusic();
    }
    
    public void SetBlacks() { 
        if (Screen.width > Screen.height) {
            GameObject.Find("BlackPanel1").GetComponent<RectTransform>().anchoredPosition = new Vector2(-2360, 0);
            GameObject.Find("BlackPanel2").GetComponent<RectTransform>().anchoredPosition = new Vector2(2360, 0);
        }
    }

    public void SetLang() {
        GameObject.Find("StandartButton").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.backToMenu;
        GameObject.Find("StandartButton (1)").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.newGame;
        GameObject.Find("Panel").transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.settings;
        GameObject.Find("SliderText").GetComponent<Text>().text = GameObject.Find("Main Camera").GetComponent<Languages>().lang.musicVolume;
    }

    public void SetMusic() {
        GameObject.Find("Slider").GetComponent<Slider>().value = float.Parse(PlayerPrefs.GetString("MusicVolume"));
    }

    public void Menu() {
        AnimateAll(new func(MenuContinue));
    }

    public void MenuContinue() {
        SceneManager.LoadScene("MenuScene");
    }

    public void NewGame() {
        PlayerPrefs.SetInt("curCamp", 1);
        PlayerPrefs.SetInt("curLvl", 1);
        PlayerPrefs.SetInt("prgCamp", 1);
        PlayerPrefs.SetInt("prgLvl", 1);
    }

    public void VolumeChanged() {
        PlayerPrefs.SetString("MusicVolume", GameObject.Find("Slider").GetComponent<Slider>().value.ToString());
        GameObject.Find("Main Camera Phone Started").GetComponent<AudioSource>().volume = float.Parse(PlayerPrefs.GetString("MusicVolume"));
    }

    private void AnimateAll(func cont) {
        StartCoroutine(Animate("Slider", "PlayButton"));
        StartCoroutine(Animate("StandartButton (1)", "SettingsRight"));
        StartCoroutine(Animate("Dropdown", "SettingsLeft"));
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
