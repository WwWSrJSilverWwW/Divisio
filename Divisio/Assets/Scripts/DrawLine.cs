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
    private string file = "Assets/current.txt";

    public class LevelStructure {
        public List<string> whiteSquare = new List<string>();
        public List<string> blackSquare = new List<string>();
        public List<string> noWay = new List<string>();
        public string enter = "2,0";
        public string exit = "2,4";
        public int N = 4, M = 4;
    }

    void Start() {
        UpdateValues();
        Canvas = GameObject.Find("Canvas");
        line = GetComponent<LineRenderer>();
        line.positionCount = 1;
        line.useWorldSpace = true;
        TextAsset txt = (TextAsset)Resources.Load("Levels/" + curCamp + "/" + curLvl, typeof(TextAsset));
        string settings = txt.text;
        LevelStructure curLevelStruct = JsonUtility.FromJson<LevelStructure>(settings);
        int x0 = int.Parse(curLevelStruct.enter.Split(new char[] { ',' })[0]) - 2;
        int y0 = int.Parse(curLevelStruct.enter.Split(new char[] { ',' })[1]);
        startPoint = new Vector3(x0, -1 + y0, 0);
        pointsList.Add(startPoint);
        line.SetPosition(pointsList.Count - 1, (Vector3)pointsList[pointsList.Count - 1]);
        for (int i = 0; i < curLevelStruct.noWay.Count; i++) {
            noWay.Add(new Vector2(int.Parse(curLevelStruct.noWay[i].Split(new char[] { ',' })[0]), int.Parse(curLevelStruct.noWay[i].Split(new char[] { ',' })[1])));
        }
    }

    public void UpPressed() {
        newPoint.x = pointsList[pointsList.Count - 1].x;
        newPoint.y = pointsList[pointsList.Count - 1].y + 1;
        newPoint.z = 0.0f;
        AddPointToList();
    }

    public void LeftPressed() {
        newPoint.x = pointsList[pointsList.Count - 1].x - 1;
        newPoint.y = pointsList[pointsList.Count - 1].y;
        newPoint.z = 0.0f;
        AddPointToList();
    }

    public void RightPressed() {
        newPoint.x = pointsList[pointsList.Count - 1].x + 1;
        newPoint.y = pointsList[pointsList.Count - 1].y;
        newPoint.z = 0.0f;
        AddPointToList();
    }

    public void DownPressed() {
        newPoint.x = pointsList[pointsList.Count - 1].x;
        newPoint.y = pointsList[pointsList.Count - 1].y - 1;
        newPoint.z = 0.0f;
        AddPointToList();
    }

    public void AddPointToList() {
        Vector3 lastPoint = pointsList[pointsList.Count - 1];
        float y1 = lastPoint.y + newPoint.y + 2;
        float x1 = lastPoint.x + newPoint.x + 4;
        float y2 = (newPoint.y + 1) * 2;
        float x2 = (newPoint.x + 2) * 2;
        if (-2 <= newPoint.x && newPoint.x <= 2 && -1 <= newPoint.y && newPoint.y <= 3 && !pointsList.Contains(newPoint) && !noWay.Contains(new Vector2(x1, y1)) && !noWay.Contains(new Vector2(x2, y2))) {
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