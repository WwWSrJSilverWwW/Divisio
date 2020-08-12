using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class DrawLine : MonoBehaviour
{
    private LineRenderer line;
    private List<Vector2> noWay = new List<Vector2>();
    public List<Vector3> pointsList = new List<Vector3>();
    private Vector3 newPoint;
    private Vector3 startPoint;
    private RectTransform inPanel;
    private StreamReader reader;
    private int level;
    private GameObject Canvas;
    private int curCamp, curLvl, prgCamp, prgLvl, stopEndOfLevels;
    private string file;
    private int moveX = 0, moveY = 90;
    private int w = 60;
    private int tX, tY;
    public int N, M;
    private List<int> ws = new List<int>() { 160, 108, 80, 64, 52, 44, 40, 36, 32, 28 };
    private string platform;

    public class LevelStructure {
        public List<string> whiteSquare = new List<string>();
        public List<string> blackSquare = new List<string>();
        public List<string> noWay = new List<string>();
        public List<string> deleteObject = new List<string>();
        public List<string> takePoint = new List<string>();
        public string enter = "2,0";
        public string exit = "2,4";
        public int N = 4, M = 4;
    }

    void Start() {
        if (Application.platform == RuntimePlatform.Android) {
            platform = Application.persistentDataPath;
        } else {
            platform = "Assets";
        }
        file = platform + "/current.txt";
        UpdateValues();
        Canvas = GameObject.Find("Canvas");
        line = GetComponent<LineRenderer>();
        line.positionCount = 1;
        TextAsset txt = (TextAsset)Resources.Load("Levels/" + curCamp + "/" + curLvl, typeof(TextAsset));
        string settings = txt.text;
        LevelStructure curLevelStruct = JsonUtility.FromJson<LevelStructure>(settings);
        N = curLevelStruct.N;
        M = curLevelStruct.M;
        w = ws[Mathf.Max(N, M) - 1];
        if (M % 2 == 0) {
            tX = -(M / 2) * w + w / 2 + moveX;
        } else {
            tX = -(int)(M / 2) * w + moveX;
        }
        if (N % 2 == 0) {
            tY = -(N / 2) * w + w / 2 + moveY;
        } else {
            tY = -(int)(N / 2) * w + moveY;
        }
        int x0 = int.Parse(curLevelStruct.enter.Split(new char[] { ',' })[0]);
        int y0 = int.Parse(curLevelStruct.enter.Split(new char[] { ',' })[1]);
        startPoint = new Vector3(tX + x0 * w - w / 2, tY + y0 * w - w / 2, 0);
        pointsList.Add(startPoint);
        line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
        for (int i = 0; i < curLevelStruct.noWay.Count; i++) {
            noWay.Add(new Vector2(int.Parse(curLevelStruct.noWay[i].Split(new char[] { ',' })[0]), int.Parse(curLevelStruct.noWay[i].Split(new char[] { ',' })[1])));
        }
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Escape)) {
            if (line.positionCount > 0) {
                line.positionCount--;
                pointsList.RemoveAt(pointsList.Count - 1);
            }
        }
    }

    public void UpPressed() {
        newPoint.x = pointsList[pointsList.Count - 1].x;
        newPoint.y = pointsList[pointsList.Count - 1].y + w;
        newPoint.z = 0.0f;
        AddPointToList();
    }

    public void LeftPressed() {
        newPoint.x = pointsList[pointsList.Count - 1].x - w;
        newPoint.y = pointsList[pointsList.Count - 1].y;
        newPoint.z = 0.0f;
        AddPointToList();
    }

    public void RightPressed() {
        newPoint.x = pointsList[pointsList.Count - 1].x + w;
        newPoint.y = pointsList[pointsList.Count - 1].y;
        newPoint.z = 0.0f;
        AddPointToList();
    }

    public void DownPressed() {
        newPoint.x = pointsList[pointsList.Count - 1].x;
        newPoint.y = pointsList[pointsList.Count - 1].y - w;
        newPoint.z = 0.0f;
        AddPointToList();
    }

    public void AddPointToList() {
        Vector3 lastPoint = pointsList[pointsList.Count - 1];
        int X, Y;
        if (M % 2 == 0) {
            X = (M / 2) * w;
        } else {
            X = (int)(M / 2) * w + w / 2;
        }
        if (N % 2 == 0) {
            Y = (N / 2) * w;
        } else {
            Y = (int)(N / 2) * w + w / 2;
        }
        float x1 = ((newPoint.x + lastPoint.x) / 2 - tX + w / 2) / (w / 2);
        float y1 = ((newPoint.y + lastPoint.y) / 2 - tY + w / 2) / (w / 2);
        if (-X + moveX <= newPoint.x && newPoint.x <= X + moveX && -Y + moveY <= newPoint.y && newPoint.y <= Y + moveY && !pointsList.Contains(newPoint) && !noWay.Contains(new Vector2(x1, y1))) {
            pointsList.Add(newPoint);
            line.positionCount = pointsList.Count;
            line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
        }
    }

    public void Reset() {
        SceneManager.LoadScene("GameScene");
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
}