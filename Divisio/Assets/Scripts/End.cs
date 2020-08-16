using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class End : MonoBehaviour
{
    void Start() {
        float ratio = (float)(Screen.height / Screen.width);
        float ortSize = 720f * ratio / 200f;
        Camera.main.orthographicSize = ortSize;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.JoystickButton7)) {
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void Menu() {
        SceneManager.LoadScene("MenuScene");
    }
}
