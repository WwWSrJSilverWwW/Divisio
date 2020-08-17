using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Preview : MonoBehaviour
{
    void Start() {
    }

    void Update() {
    }

    public void Continue() {
        SceneManager.LoadScene("MenuScene");
    }
}
