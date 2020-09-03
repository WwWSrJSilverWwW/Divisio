using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundsEffects : MonoBehaviour
{
    void Start() {
        if (!PlayerPrefs.HasKey("SoundVolume")) {
            PlayerPrefs.SetString("SoundVolume", "0,5");
        }
    }

    void Update() {
    }

    public void PlaySound() { 
        AudioSource.PlayClipAtPoint(GetComponent<AudioSource>().clip, new Vector3(0, 0, -10), float.Parse(PlayerPrefs.GetString("SoundVolume")));
    }
}
