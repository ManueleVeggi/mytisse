using System.Collections;
using System.Collections.Generic;
using System.IO;

using UnityEditor;

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TMPro;
using UnityEngine.Experimental.Rendering;

public class PaintingCanvas : MonoBehaviour
{
    private GameObject[] PaintPalette;
    private GameObject[] Tools;
    public Slider SizeSlider;

    public static bool isBrushActive = true;
    public Color32 currentColor;

    public GameObject BaseDot;
    public GameObject XrfScan;

    private List<GameObject> dotInstances = new List<GameObject>();
    public List<object> PaintedDotInfo = new List<object>();

    public void SetPalette(bool isPaletteA)
    {
        PaintPalette = GameObject.FindGameObjectsWithTag("Pigment");

        if (isPaletteA)
        {
            PaintPalette[0].GetComponent<Image>().color = new Color32(48, 52, 144, 255);
            PaintPalette[1].GetComponent<Image>().color = new Color32(153, 203, 50, 255);
            PaintPalette[2].GetComponent<Image>().color = new Color32(92, 64, 51, 255);
            PaintPalette[3].GetComponent<Image>().color = new Color32(255, 191, 203, 255);
            PaintPalette[4].GetComponent<Image>().color = new Color32(208, 65, 23, 255);
            PaintPalette[5].GetComponent<Image>().color = new Color32(85, 7, 11, 255);
        }
        else
        {
            PaintPalette[0].GetComponent<Image>().color = new Color32(50, 55, 56, 255);
            PaintPalette[1].GetComponent<Image>().color = new Color32(145, 146, 148, 255);
            PaintPalette[2].GetComponent<Image>().color = new Color32(163, 184, 201, 255);
            PaintPalette[3].GetComponent<Image>().color = new Color32(65, 144, 78, 255);
            PaintPalette[4].GetComponent<Image>().color = new Color32(197, 177, 173, 255);
            PaintPalette[5].GetComponent<Image>().color = new Color32(223, 226, 215, 255);
        }
    }

    public void ChangeColor(GameObject ClickedButton)
    {
        currentColor = ClickedButton.GetComponent<Image>().color;
    }

    public void ChangeTool(bool isBrush)
    {
        isBrushActive = isBrush;
        Tools = GameObject.FindGameObjectsWithTag("ToolButton");

        if (isBrush)
        {
            Tools[0].GetComponent<Image>().color = new Color32(0, 0, 0, 255);
            Tools[1].GetComponent<Image>().color = new Color32(130, 126, 126, 255);
        }
        else
        {
            Debug.Log("Eraser Active");
            Tools[1].GetComponent<Image>().color = new Color32(0, 0, 0, 255);
            Tools[0].GetComponent<Image>().color = new Color32(130, 126, 126, 255);
        }

    }

    void Update()
    {
        Vector2 objPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);

        float setSize = SizeSlider.value;

        if (Input.GetKey(KeyCode.Mouse0) && XRFScanHandler.isHoveringScan)// Use GetKey for ongoing event, GetKeyDown only one click 
        {
            if (isBrushActive)
            {
                GameObject dot = Instantiate(BaseDot);
                dot.transform.SetParent(XrfScan.transform);
                dot.transform.localScale = new Vector3(setSize + 0.5f, setSize + 0.5f, 0);
                dot.transform.position = objPos;
                dot.GetComponent<DotHandler>().dotColor = currentColor;
                dot.GetComponent<Image>().color = currentColor;
            }
        }
    }

    public void SubmitPainting(TMP_InputField givenTitle)
    {
        Debug.Log("Submitted");
        dotInstances.Clear();
        PaintedDotInfo.Clear();

        // -------- Set file path ---------------
        string projectPath = Application.dataPath;
        string folderPath = Path.Combine(projectPath, "Export");
        string filePath = Path.Combine(folderPath, "test.png");
        // --------------------------------------

        // 1. SAVE THE IMAGE

        Capture(XrfScan, filePath);

        // 2. RETRIEVE THE TITLE
        string paintingTitle = givenTitle.text.ToString();
        Debug.Log(paintingTitle);

        // 3. RETRIEVE THE DOTS
        GameObject[] dotObjects = GameObject.FindGameObjectsWithTag("Dot");

        foreach (GameObject dotObject in dotObjects) { PaintedDotInfo.Add(dotObject.GetComponent<DotHandler>().GetInfoDot()); }

        /* // Each line is a list with all the information related to a point
        foreach (List<object> dotInfo in PaintedDotInfo)
        {
            // ... [Use it to access single piece of info, e.g. Debug] ...
        }
        */
    }

    // ===========================================================
    // ========================= SAVE IMAGE ======================
    // ===========================================================

    public void Capture(GameObject gameObject, string filePathStr)
    {
        Debug.Log("Capture started");
        var rectTransform = gameObject.GetComponent<RectTransform>();
        var delta = rectTransform.sizeDelta;
        var position = rectTransform.position;

        var offset = new RectOffset((int)(delta.x / 2), (int)(delta.x / 2), (int)(delta.y / 2), (int)(delta.y / 2));
        var rect = new Rect(position, Vector2.zero);
        var add = offset.Add(rect);

        var tex = new Texture2D((int)add.width, (int)add.height);
        tex.ReadPixels(add, 0, 0);

        var encodeToPNG = tex.EncodeToPNG();

        File.WriteAllBytes(filePathStr, encodeToPNG);

        AssetDatabase.Refresh(); // Update Asset Folder in RunTime (for Debug!)
        DestroyImmediate(tex);
    }

    // ===========================================================
}