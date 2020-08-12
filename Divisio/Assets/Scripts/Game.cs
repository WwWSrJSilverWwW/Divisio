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
    public List<Vector2> deletePoints;
    public List<Vector2> takePoints;
    private Vector2 p;
    private GameObject whitePanel;
    private GameObject blackPanel;
    private GameObject deletePanel;
    private GameObject takePanel;
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
    private int moveX = 0, moveY = 90;
    private int w;
    private int tX, tY;
    private List<List<int>> infoPlanes;
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
        Canvas = GameObject.Find("Canvas");
        UpdateValues();
        TextAsset txt = (TextAsset)Resources.Load("Levels/" + curCamp + "/" + curLvl, typeof(TextAsset));
        string settings = txt.text;
        curLevelStruct = JsonUtility.FromJson<LevelStructure>(settings);
        SetEnterExitSettings();
        SetWalls();
        AddWhiteSquare();
        AddBlackSquare();
        AddNoWay();
        AddTakePoints();
        AddDeleteObject();
    }

    void Update() {
        //ShowWrongSquares();
    }

    public void SetEnterExitSettings() {
        whitePoints.Clear();
        blackPoints.Clear();
        GameObject.Find("LevelText").GetComponent<Text>().text = "Level " + curCamp + "-" + curLvl;
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
        for (int i = 0; i < N; i++) {
            for (int j = 0; j < M; j++) {
                GameObject cell = Instantiate(Resources.Load("Prefabs/Cell")) as GameObject;
                cell.name = "cellPanel" + i + j;
                cell.transform.SetParent(Canvas.transform, false);
                cell.transform.SetSiblingIndex(0);
                cell.GetComponent<RectTransform>().anchoredPosition = new Vector2(tX + j * w, tY + i * w);
                cell.GetComponent<RectTransform>().sizeDelta = new Vector2(w, w);
            }
        }
        exitX = int.Parse(curLevelStruct.exit.Split(new char[] { ',' })[0]);
        exitY = int.Parse(curLevelStruct.exit.Split(new char[] { ',' })[1]);
        enterX = int.Parse(curLevelStruct.enter.Split(new char[] { ',' })[0]);
        enterY = int.Parse(curLevelStruct.enter.Split(new char[] { ',' })[1]);
        outPanel = GameObject.Find("OutPanel").GetComponent<RectTransform>();
        outPanel.anchoredPosition = new Vector2(tX + w * exitX - w / 2, tY + w * exitY - w / 2);
        outPanel.sizeDelta = new Vector2(w / 2, w / 2);
        inPanel = GameObject.Find("InPanel").GetComponent<RectTransform>();
        inPanel.anchoredPosition = new Vector2(tX + w * enterX - w / 2, tY + w * enterY - w / 2);
        inPanel.sizeDelta = new Vector2(w / 2, w / 2);
    }
    
    public void SetWalls() {
        GameObject cornerUL = Instantiate(Resources.Load("Prefabs/cornerUL")) as GameObject;
        GameObject cornerUR = Instantiate(Resources.Load("Prefabs/cornerUR")) as GameObject;
        cornerUL.transform.SetParent(Canvas.transform, false);
        cornerUR.transform.SetParent(Canvas.transform, false);
        cornerUL.GetComponent<RectTransform>().anchoredPosition = new Vector2(tX - w / 2 - w / 4 - 2 * moveX, -(tY - w / 2 - w / 4 - 2 * moveY));
        cornerUR.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(tX - w / 2 - w / 4 - 2 * moveX), -(tY - w / 2 - w / 4 - 2 * moveY));
        cornerUL.GetComponent<RectTransform>().sizeDelta = new Vector2(w / 2, w / 2);
        cornerUR.GetComponent<RectTransform>().sizeDelta = new Vector2(w / 2, w / 2);
        cornerUL.transform.SetSiblingIndex(0);
        cornerUR.transform.SetSiblingIndex(0);
        GameObject cornerDL = Instantiate(Resources.Load("Prefabs/cornerDL")) as GameObject;
        GameObject cornerDR = Instantiate(Resources.Load("Prefabs/cornerDR")) as GameObject;
        cornerDL.transform.SetParent(Canvas.transform, false);
        cornerDR.transform.SetParent(Canvas.transform, false);
        cornerDL.GetComponent<RectTransform>().anchoredPosition = new Vector2(tX - w / 2 - w / 4, tY - w / 2 - w / 4);
        cornerDR.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(tX - w / 2 - w / 4), tY - w / 2 - w / 4);
        cornerDL.GetComponent<RectTransform>().sizeDelta = new Vector2(w / 2, w / 2);
        cornerDR.GetComponent<RectTransform>().sizeDelta = new Vector2(w / 2, w / 2);
        cornerDL.transform.SetSiblingIndex(0);
        cornerDR.transform.SetSiblingIndex(0);
        for (int i = 0; i < N; i++) {
            GameObject wallL = Instantiate(Resources.Load("Prefabs/wallL")) as GameObject;
            GameObject wallR = Instantiate(Resources.Load("Prefabs/wallR")) as GameObject; 
            wallL.transform.SetParent(Canvas.transform, false);
            wallR.transform.SetParent(Canvas.transform, false);
            wallL.GetComponent<RectTransform>().anchoredPosition = new Vector2(tX - w / 2 - w / 4, tY - w + (i + 1) * w);
            wallR.GetComponent<RectTransform>().anchoredPosition = new Vector2(-(tX - w / 2 - w / 4), tY - w + (i + 1) * w);
            wallL.GetComponent<RectTransform>().sizeDelta = new Vector2(w / 2, w);
            wallR.GetComponent<RectTransform>().sizeDelta = new Vector2(w / 2, w);
            wallL.transform.SetSiblingIndex(0);
            wallR.transform.SetSiblingIndex(0);
        }
        for (int i = 0; i < M; i++) {
            GameObject wallU = Instantiate(Resources.Load("Prefabs/wallU")) as GameObject;
            GameObject wallD = Instantiate(Resources.Load("Prefabs/wallD")) as GameObject; 
            wallU.transform.SetParent(Canvas.transform, false);
            wallD.transform.SetParent(Canvas.transform, false);
            wallU.GetComponent<RectTransform>().anchoredPosition = new Vector2(-tX + w - (i + 1) * w, -tY + moveY * 2 + w / 2 + w / 4);
            wallD.GetComponent<RectTransform>().anchoredPosition = new Vector2(-tX + w - (i + 1) * w, tY - w / 2 - w / 4);
            wallU.GetComponent<RectTransform>().sizeDelta = new Vector2(w, w / 2);
            wallD.GetComponent<RectTransform>().sizeDelta = new Vector2(w, w / 2);
            wallU.transform.SetSiblingIndex(0);
            wallD.transform.SetSiblingIndex(0);
        }
    }

    public void AddWhiteSquare() {
        for (int i = 0; i < curLevelStruct.whiteSquare.Count; i++) {
            whitePoints.Add(new Vector2(int.Parse(curLevelStruct.whiteSquare[i].Split(new char[] { ',' })[0]), int.Parse(curLevelStruct.whiteSquare[i].Split(new char[] { ',' })[1])));
        }
        for (int i = 0; i < whitePoints.Count; i++) {
            whitePanel = new GameObject("whitePanel" + whitePoints[i].x + whitePoints[i].y);
            whitePanel.AddComponent<CanvasRenderer>();
            whitePanel.AddComponent<RectTransform>();
            whitePanel.AddComponent<Image>();
            whitePanel.GetComponent<Image>().color = Color.white;
            whitePanel.transform.SetParent(Canvas.transform, false);
            rect = GameObject.Find("whitePanel" + whitePoints[i].x + whitePoints[i].y).GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(tX + w * whitePoints[i].x, tY + w * whitePoints[i].y);
            rect.sizeDelta = new Vector2(w / 4, w / 4);
        }
    }

    public void AddBlackSquare() {
        for (int i = 0; i < curLevelStruct.blackSquare.Count; i++) {
            blackPoints.Add(new Vector2(int.Parse(curLevelStruct.blackSquare[i].Split(new char[] { ',' })[0]), int.Parse(curLevelStruct.blackSquare[i].Split(new char[] { ',' })[1])));
        }
        for (int i = 0; i < blackPoints.Count; i++) {
            blackPanel = new GameObject("blackPanel" + blackPoints[i].x + blackPoints[i].y);
            blackPanel.AddComponent<CanvasRenderer>();
            blackPanel.AddComponent<RectTransform>();
            blackPanel.AddComponent<Image>();
            blackPanel.GetComponent<Image>().color = Color.black;
            blackPanel.transform.SetParent(Canvas.transform, false);
            rect = GameObject.Find("blackPanel" + blackPoints[i].x + blackPoints[i].y).GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(tX + w * blackPoints[i].x, tY + w * blackPoints[i].y);
            rect.sizeDelta = new Vector2(w / 4, w / 4);
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
            passWay.GetComponent<Image>().color = new Color(0.247058f, 0.282352f, 0.8f, 1);
            passWay.transform.SetParent(Canvas.transform, false);
            rect = passWay.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(tX + w / 2 * noWay[i].x - w / 2, tY + w / 2 * noWay[i].y - w / 2);
            if (noWay[i].x % 2 == 0 && noWay[i].y % 2 != 0) {
                rect.sizeDelta = new Vector2(w / 2, w / 4);
            }
            else if (noWay[i].x % 2 != 0 && noWay[i].y % 2 == 0) {
                rect.sizeDelta = new Vector2(w / 4, w / 2);
            }
            else {
                rect.sizeDelta = new Vector2(w / 2, w / 2);
            }
        }
    }

    public void AddDeleteObject() { 
        for (int i = 0; i < curLevelStruct.deleteObject.Count; i++) {
            deletePoints.Add(new Vector2(int.Parse(curLevelStruct.deleteObject[i].Split(new char[] { ',' })[0]), int.Parse(curLevelStruct.deleteObject[i].Split(new char[] { ',' })[1])));
        }
        for (int i = 0; i < deletePoints.Count; i++) {
            deletePanel = Instantiate(Resources.Load("Prefabs/DeleteObject")) as GameObject;
            deletePanel.name = "deletePanel" + deletePoints[i].x + deletePoints[i].y;
            deletePanel.transform.SetParent(Canvas.transform, false);
            rect = deletePanel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(tX + w * deletePoints[i].x, tY + w * deletePoints[i].y);
            rect.sizeDelta = new Vector2(w / 4, w / 4);
        }
    }

    public void AddTakePoints() { 
        for (int i = 0; i < curLevelStruct.takePoint.Count; i++) {
            takePoints.Add(new Vector2(int.Parse(curLevelStruct.takePoint[i].Split(new char[] { ',' })[0]), int.Parse(curLevelStruct.takePoint[i].Split(new char[] { ',' })[1])));
        }
        for (int i = 0; i < takePoints.Count; i++) {
            takePanel = Instantiate(Resources.Load("Prefabs/TakePoint")) as GameObject;
            takePanel.name = "takePanel" + takePoints[i].x + takePoints[i].y;
            takePanel.transform.SetParent(Canvas.transform, false);
            rect = takePanel.GetComponent<RectTransform>();
            rect.anchoredPosition = new Vector2(tX + w * takePoints[i].x / 2 - w / 2, tY + w * takePoints[i].y / 2 - w / 2);
            rect.sizeDelta = new Vector2(w / 3, w / 3);
        }
    }

    public void MakeBFSSplitPlane() {
        pointsList = GameObject.Find("Line").GetComponent<DrawLine>().pointsList;
        float lineExitX = pointsList[pointsList.Count - 1].x;
        float lineExitY = pointsList[pointsList.Count - 1].y;
        if (lineExitX != tX + w * exitX - w / 2 || lineExitY != tY + w * exitY - w / 2) {
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
                    if (splittedPlane[i][j] == -1) {
                        StartBFS(j, i, curPlane);
                        curPlane++;
                    }
                }
            }
            infoPlanes = new List<List<int>>();
            for (int i = 0; i < curPlane; i++) {
                List<int> add = new List<int>();
                add.Add(0);
                add.Add(0);
                add.Add(0);
                infoPlanes.Add(add);
            }
            for (int i = 0; i < N; i++) {
                for (int j = 0; j < M; j++) { 
                    if (GameObject.Find("whitePanel" + j + i) != null) {
                        infoPlanes[splittedPlane[i][j]][0]++;
                    } else if (GameObject.Find("blackPanel" + j + i) != null) {
                        infoPlanes[splittedPlane[i][j]][1]++;
                    } else if (GameObject.Find("deletePanel" + j + i) != null) {
                        infoPlanes[splittedPlane[i][j]][2]++;
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
            if (0 <= cur.x + 1 && cur.x + 1 <= M - 1 && 0 <= cur.y && cur.y <= N - 1 && splittedPlane[(int)cur.y][(int)cur.x + 1] == -1 && PathExists((int)cur.x, (int)cur.y, (int)(cur.x + 1), (int)cur.y)) {
                pub = new Vector2(cur.x + 1, cur.y);
                queue.Add(pub);
            }
            if (0 <= cur.x - 1 && cur.x - 1 <= M - 1 && 0 <= cur.y && cur.y <= N - 1 && splittedPlane[(int)cur.y][(int)cur.x - 1] == -1 && PathExists((int)(cur.x - 1), (int)cur.y, (int)cur.x, (int)cur.y)) {
                pub = new Vector2(cur.x - 1, cur.y);
                queue.Add(pub);
            }
            if (0 <= cur.x && cur.x <= M - 1 && 0 <= cur.y + 1 && cur.y + 1 <= N - 1 && splittedPlane[(int)cur.y + 1][(int)cur.x] == -1 && PathExists((int)cur.x, (int)cur.y, (int)cur.x, (int)(cur.y + 1))) {
                pub = new Vector2(cur.x, cur.y + 1);
                queue.Add(pub);
            }
            if (0 <= cur.x && cur.x <= M - 1 && 0 <= cur.y - 1 && cur.y - 1 <= N - 1 && splittedPlane[(int)cur.y - 1][(int)cur.x] == -1 && PathExists((int)cur.x, (int)(cur.y - 1), (int)cur.x, (int)cur.y)) {
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
        } else {
            ans1Y = y1;
            ans2Y = y1 + 1;
            ans1X = x2;
            ans2X = x2;
        }
        for (int i = 0; i < pointsList.Count - 1; i++) {
            float a = (pointsList[i].x - moveX + M * w / 2) / w;
            float b = (pointsList[i].y - moveY + N * w / 2) / w;
            float c = (pointsList[i + 1].x - moveX + M * w / 2) / w;
            float d = (pointsList[i + 1].y - moveY + N * w / 2) / w;
            if (a == ans1X && b == ans1Y && c == ans2X && d == ans2Y) {
                return false;
            }
            if (a == ans2X && b == ans2Y && c == ans1X && d == ans1Y) {
                return false;
            }
        }
        return true;
    }

    public void CheckAll() {
        if (!CheckSquares()) {
            x = 0;
            Debug.Log("CheckError2: White and black squares aren't splitted.");
        } else if (!CheckTakePoints()) {
            Debug.Log("CheckError3: Some points aren't taken.");
        } else {
            Debug.Log("You won!");
            SceneManager.LoadScene("LevelComplitedScene");
        }
    }

    public bool CheckSquares() {
        for (int i = 0; i < infoPlanes.Count; i++) {
            List<int> li = infoPlanes[i];
            if (li[0] + li[1] < li[2]) {
                return false;
            } else if (Mathf.Min(li[0], li[1]) > li[2]) {
                return false;
            }
        }
        return true;
    }

    public bool CheckTakePoints() {
        List<Vector3> e = new List<Vector3>();
        for (int i = 0; i < pointsList.Count - 1; i++) {
            e.Add(new Vector3((pointsList[i].x + pointsList[i + 1].x) / 2, (pointsList[i].y + pointsList[i + 1].y) / 2, 0));
        }
        for (int i = 0; i < takePoints.Count; i++) {
            Vector2 g = GameObject.Find("takePanel" + takePoints[i].x + takePoints[i].y).GetComponent<RectTransform>().anchoredPosition;
            Vector3 gg = new Vector3(g.x, g.y, 0);
            Debug.Log(gg);
            if (!pointsList.Contains(gg) && !e.Contains(gg)) {
                return false;
            }
        }
        return true;
    }

    public void ShowWrongSquares() {
        if (x < 5) {
            RectTransform white = wrongWhiteSquare.GetComponent<RectTransform>();
            RectTransform black = wrongBlackSquare.GetComponent<RectTransform>();
            if (white.sizeDelta.x <= w / 4) {
                k = 0.5f;
                x++;
            }
            else if (white.sizeDelta.x >= w / 2) {
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
        string file = platform + "/current.txt";
        StreamReader reader = new StreamReader(file);
        string text = reader.ReadToEnd();
        curCamp = int.Parse(text.Split(new char[] { ';' })[0].Split(new char[] { ':' })[1]);
        curLvl = int.Parse(text.Split(new char[] { ';' })[1].Split(new char[] { ':' })[1]);
        prgCamp = int.Parse(text.Split(new char[] { ';' })[2].Split(new char[] { ':' })[1]);
        prgLvl = int.Parse(text.Split(new char[] { ';' })[3].Split(new char[] { ':' })[1]);
        reader.Close();
    }
}