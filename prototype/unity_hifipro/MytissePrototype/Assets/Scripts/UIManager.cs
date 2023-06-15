using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Create a variable for each panel
    public GameObject homePage;
    public GameObject creditPage;
    public GameObject palettePage;
    public GameObject paintPage;

    public void OnEnable()
    {
        creditPage.SetActive(false);
        palettePage.SetActive(false);
        paintPage.SetActive(false);
        homePage.SetActive(true);
    }

    public void ChosenPalette (int paletteID)
    {
        paintPage.GetComponent<PaintingCanvas>().SetPalette(paletteID == 1);
    }

    // For Fading In / Fading Out
    /*
    public void OpenScreen(int screenIndex)
    {
       Debug.Log("Open screen number " + screenIndex);
    // Reference for fade in/out: https://stackoverflow.com/questions/72245314/how-to-fade-out-in-a-canvas-canvas-group-and-its-children-objects
    }
    */
}
