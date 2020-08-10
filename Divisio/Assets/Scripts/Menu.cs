using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class Menu : MonoBehaviour
{
    private string file;

    void Start() {
        file = Application.persistentDataPath + "/current.txt";
        if (!File.Exists(file)) {
            File.CreateText(file).Close();
            NewGame();
        }
    }

    public void Play() {
        SceneManager.LoadScene("GameScene");
    }

    public void Exit() {
        Application.Quit();
    }

    public void Levels() {
        SceneManager.LoadScene("ChooseCampaignScene");
    }

    public void NewGame() {
        StreamWriter writer = new StreamWriter(file);
        writer.Write("currentCampaign:1;\ncurrentLevel:1;\nprogressCampaign:1;\nprogressLevel:1;\nend,of,file.");
        writer.Close();
    }
}