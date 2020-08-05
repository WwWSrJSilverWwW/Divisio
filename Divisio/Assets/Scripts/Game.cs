using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

public class Game : MonoBehaviour
{
    private RectTransform outPanel;
    private RectTransform inPanel;
    private LevelStructure curLevelStruct;
    private List<Vector2> noWay = new List<Vector2>();
    private RectTransform rect;
    public int exitX, exitY, enterX, enterY;
    public List<Vector2> whitePoints;
    public List<Vector2> blackPoints;
    private Vector2 p;
    private GameObject whitePanel;
    private GameObject blackPanel;
    private GameObject Canvas;
    private GameObject newGameObj;
    private List<Vector3> pointsList;
    public List<List<int>> splittedPlane;
    private Vector2 pub;
    public int N;
    public int M;
    private int ans1X, ans1Y, ans2X, ans2Y;
    public int level;
    private GameObject wrongWhiteSquare;
    private GameObject wrongBlackSquare;
    private int x = 5;
    private float k = 0.5f;
    private int curCamp, curLvl, prgCamp, prgLvl;

    public class LevelStructure {
        public List<string> whiteSquare = new List<string>();
        public List<string> blackSquare = new List<string>();
        public List<string> noWay = new List<string>();
        public string enter = "2,0";
        public string exit = "2,4";
        public int N = 4, M = 4;
    }

    void Start() {
        Canvas = GameObject.Find("Canvas");
        UpdateValues();
        TextAsset txt = (TextAsset)Resources.Load("Levels/" + curCamp + "/" + curLvl, typeof(TextAsset));
        string settings = txt.text;
        curLevelStruct = JsonUtility.FromJson<LevelStructure>(settings);
        SetEnterExitSettings();
        AddWhiteSquare();
        AddBlackSquare();
        AddNoWay();
    }

    void Update() {
        ShowWrongSquares();
    }

    public void SetEnterExitSettings() {
        whitePoints.Clear();
        blackPoints.Clear();
        GameObject.Find("LevelText").GetComponent<Text>().text = "Level " + curCamp + "-" + curLvl;
        N = curLevelStruct.N;
        M = curLevelStruct.M;
        exitX = int.Parse(curLevelStruct.exit.Split(new char[] { ',' })[0]);
        exitY = int.Parse(curLevelStruct.exit.Split(new char[] { ',' })[1]);
        enterX = int.Parse(curLevelStruct.enter.Split(new char[] { ',' })[0]) - 2;
        enterY = int.Parse(curLevelStruct.enter.Split(new char[] { ',' })[1]);
        outPanel = GameObject.Find("OutPanel").GetComponent<RectTransform>();
        outPanel.anchoredPosition = new Vector2(-160 + 80 * exitX, -80 + 80 * exitY);
        inPanel = GameObject.Find("InPanel").GetComponent<RectTransform>();
        inPanel.anchoredPosition = new Vector2(80 * enterX, -80 + 80 * enterY);
    }

    public void AddWhiteSquare() {
        for (int i = 0; i < curLevelStruct.whiteSquare.Count; i++) {
            whitePoints.Add(new Vector2(int.Parse(curLevelStruct.whiteSquare[i].Split(new char[] { ',' })[0]), int.Parse(curLevelStruct.whiteSquare[i].Split(new char[] { ',' })[1])));
        }
        for (int i = 0; i < whitePoints.Count; i++) {
            if (GameObject.Find("whitePanel" + whitePoints[i].x + whitePoints[i].y) == null) {
                whitePanel = new GameObject("whitePanel" + whitePoints[i].x + whitePoints[i].y);
                whitePanel.AddComponent<CanvasRenderer>();
                whitePanel.AddComponent<RectTransform>();
                whitePanel.AddComponent<Image>();
            }
            else {
                whitePanel = GameObject.Find("whitePanel" + whitePoints[i].x + whitePoints[i].y);
            }
            whitePanel.GetComponent<Image>().color = Color.white;
            whitePanel.transform.SetParent(Canvas.transform, false);
            rect = GameObject.Find("whitePanel" + whitePoints[i].x + whitePoints[i].y).GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-120 + 80 * whitePoints[i].x, -40 + 80 * whitePoints[i].y);
            rect.sizeDelta = new Vector2(20, 20);
        }
    }

    public void AddBlackSquare() {
        for (int i = 0; i < curLevelStruct.blackSquare.Count; i++) {
            blackPoints.Add(new Vector2(int.Parse(curLevelStruct.blackSquare[i].Split(new char[] { ',' })[0]), int.Parse(curLevelStruct.blackSquare[i].Split(new char[] { ',' })[1])));
        }
        for (int i = 0; i < blackPoints.Count; i++) {
            if (GameObject.Find("blackPanel" + blackPoints[i].x + blackPoints[i].y) == null) {
                blackPanel = new GameObject("blackPanel" + blackPoints[i].x + blackPoints[i].y);
                blackPanel.AddComponent<CanvasRenderer>();
                blackPanel.AddComponent<RectTransform>();
                blackPanel.AddComponent<Image>();
            }
            else {
                blackPanel = GameObject.Find("blackPanel" + blackPoints[i].x + blackPoints[i].y);
            }
            blackPanel.GetComponent<Image>().color = Color.black;
            blackPanel.transform.SetParent(Canvas.transform, false);
            rect = GameObject.Find("blackPanel" + blackPoints[i].x + blackPoints[i].y).GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-120 + 80 * blackPoints[i].x, -40 + 80 * blackPoints[i].y);
            rect.sizeDelta = new Vector2(20, 20);
        }
    }

    public void AddNoWay() {
        for (int i = 0; i < curLevelStruct.noWay.Count; i++) {
            noWay.Add(new Vector2(int.Parse(curLevelStruct.noWay[i].Split(new char[] { ',' })[0]), int.Parse(curLevelStruct.noWay[i].Split(new char[] { ',' })[1])));
        }
        RectTransform rect;
        for (int i = 0; i < noWay.Count; i++) {
            GameObject passWay = new GameObject("noWay" + noWay[i].x + noWay[i].y);
            passWay.AddComponent<CanvasRenderer>();
            passWay.AddComponent<RectTransform>();
            passWay.AddComponent<Image>();
            passWay.GetComponent<Image>().color = new Color(0.1176f, 0.5686f, 1f, 1f);
            passWay.transform.SetParent(Canvas.transform, false);
            rect = passWay.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(-160 + 40 * noWay[i].x, -80 + 40 * noWay[i].y);
            if (noWay[i].x % 2 == 0 && noWay[i].y % 2 != 0) {
                rect.sizeDelta = new Vector2(40, 20);
            }
            else if (noWay[i].x % 2 != 0 && noWay[i].y % 2 == 0) {
                rect.sizeDelta = new Vector2(20, 40);
            }
            else {
                rect.sizeDelta = new Vector2(40, 40);
            }
        }
    }

    public void MakeBFSSplitPlane() {
        pointsList = GameObject.Find("Line").GetComponent<DrawLine>().pointsList;
        float lineExitX = pointsList[pointsList.Count - 1].x + 2;
        float lineExitY = pointsList[pointsList.Count - 1].y + 1;
        if (lineExitX != exitX || lineExitY != exitY) {
            Debug.Log("CheckError 1: The line doesn't connect to the exit.");
        }
        else {
            splittedPlane = new List<List<int>>();
            for (int i = 0; i < N; i++) {
                List<int> newList = new List<int>();
                for (int j = 0; j < M; j++) {
                    newList.Add(-1);
                }
                splittedPlane.Add(newList);
            }
            int curPlane = 0;
            for (int i = 0; i < N; i++) {
                for (int j = 0; j < M; j++) {
                    if (splittedPlane[j][i] == -1) {
                        StartBFS(i, j, curPlane);
                        curPlane++;
                    }
                }
            }
            CheckAll();
        }
    }

    public void StartBFS(int x0, int y0, int plane) {
        List<Vector2> queue = new List<Vector2>();
        Vector2 pt = new Vector2(x0, y0);
        queue.Add(pt);
        while (queue.Count != 0) {
            Vector2 cur = queue[0];
            queue.RemoveAt(0);
            splittedPlane[(int)cur.y][(int)cur.x] = plane;
            if (0 <= cur.x + 1 && cur.x + 1 <= 3 && 0 <= cur.y && cur.y <= 3 && splittedPlane[(int)cur.y][(int)cur.x + 1] == -1 && PathExists((int)cur.x, (int)cur.y, (int)(cur.x + 1), (int)cur.y)) {
                pub = new Vector2(cur.x + 1, cur.y);
                queue.Add(pub);
            }
            if (0 <= cur.x - 1 && cur.x - 1 <= 3 && 0 <= cur.y && cur.y <= 3 && splittedPlane[(int)cur.y][(int)cur.x - 1] == -1 && PathExists((int)(cur.x - 1), (int)cur.y, (int)cur.x, (int)cur.y)) {
                pub = new Vector2(cur.x - 1, cur.y);
                queue.Add(pub);
            }
            if (0 <= cur.x && cur.x <= 3 && 0 <= cur.y + 1 && cur.y + 1 <= 3 && splittedPlane[(int)cur.y + 1][(int)cur.x] == -1 && PathExists((int)cur.x, (int)cur.y, (int)cur.x, (int)(cur.y + 1))) {
                pub = new Vector2(cur.x, cur.y + 1);
                queue.Add(pub);
            }
            if (0 <= cur.x && cur.x <= 3 && 0 <= cur.y - 1 && cur.y - 1 <= 3 && splittedPlane[(int)cur.y - 1][(int)cur.x] == -1 && PathExists((int)cur.x, (int)(cur.y - 1), (int)cur.x, (int)cur.y)) {
                pub = new Vector2(cur.x, cur.y - 1);
                queue.Add(pub);
            }
        }
    }

    public bool PathExists(int x1, int y1, int x2, int y2) {
        if (x1 == x2) {
            ans1Y = y2;
            ans2Y = y2;
            ans1X = x1;
            ans2X = x1 + 1;
        }
        else {
            ans1Y = y1;
            ans2Y = y1 + 1;
            ans1X = x2;
            ans2X = x2;
        }
        for (int i = 0; i < pointsList.Count - 1; i++) {
            if (pointsList[i].x == ans1X - 2 && pointsList[i].y == ans1Y - 1 && pointsList[i + 1].x == ans2X - 2 && pointsList[i + 1].y == ans2Y - 1) {
                return false;
            }
            if (pointsList[i].x == ans2X - 2 && pointsList[i].y == ans2Y - 1 && pointsList[i + 1].x == ans1X - 2 && pointsList[i + 1].y == ans1Y - 1) {
                return false;
            }
        }
        return true;
    }

    public void CheckAll() {
        if (!CheckSquares()) {
            x = 0;
            Debug.Log("CheckError2: White and black squares aren't splitted.");
        }
        else {
            Debug.Log("You won!");
            SceneManager.LoadScene("LevelComplitedScene");
        }
    }

    public bool CheckSquares() {
        List<int> whitePlanes = new List<int>();
        List<int> blackPlanes = new List<int>();
        for (int i = 0; i < whitePoints.Count; i++) {
            whitePlanes.Add(splittedPlane[(int)whitePoints[i].y][(int)whitePoints[i].x]);
        }
        for (int i = 0; i < blackPoints.Count; i++) {
            blackPlanes.Add(splittedPlane[(int)blackPoints[i].y][(int)blackPoints[i].x]);
        }
        for (int i = 0; i < whitePoints.Count; i++) {
            for (int j = 0; j < blackPoints.Count; j++) {
                if (blackPlanes[j] == whitePlanes[i]) {
                    wrongWhiteSquare = GameObject.Find("whitePanel" + whitePoints[i].x + whitePoints[i].y);
                    wrongBlackSquare = GameObject.Find("blackPanel" + blackPoints[j].x + blackPoints[j].y);
                    return false;
                }
            }
        }
        return true;
    }

    public void ShowWrongSquares() {
        if (x < 5) {
            RectTransform white = wrongWhiteSquare.GetComponent<RectTransform>();
            RectTransform black = wrongBlackSquare.GetComponent<RectTransform>();
            if (white.sizeDelta.x <= 20) {
                k = 0.5f;
                x++;
            }
            else if (white.sizeDelta.x >= 30) {
                k = -0.5f;
            }
            white.sizeDelta = new Vector2(white.sizeDelta.x + k, white.sizeDelta.y + k);
            black.sizeDelta = new Vector2(black.sizeDelta.x + k, black.sizeDelta.y + k);
        }
    }

    public void Menu() {
        SceneManager.LoadScene("MenuScene");
    }

    public void UpdateValues() {
        string file = "Assets/current.txt";
        StreamReader reader = new StreamReader(file);
        string text = reader.ReadToEnd();
        curCamp = int.Parse(text.Split(new char[] { ';' })[0].Split(new char[] { ':' })[1]);
        curLvl = int.Parse(text.Split(new char[] { ';' })[1].Split(new char[] { ':' })[1]);
        prgCamp = int.Parse(text.Split(new char[] { ';' })[2].Split(new char[] { ':' })[1]);
        prgLvl = int.Parse(text.Split(new char[] { ';' })[3].Split(new char[] { ':' })[1]);
        reader.Close();
    }
}