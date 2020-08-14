using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class End : MonoBehaviour
{
    void Update() {
        if (Input.GetKeyDown(KeyCode.JoystickButton7)) {
            SceneManager.LoadScene("MenuScene");
        }
    }

    public void Menu() {
        SceneManager.LoadScene("MenuScene");
    }
}
