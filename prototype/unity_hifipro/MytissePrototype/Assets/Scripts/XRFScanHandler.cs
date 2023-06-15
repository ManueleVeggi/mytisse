//Attach this script to the GameObject you would like to have mouse hovering detected on
//This script outputs a message to the Console when the mouse pointer is currently detected hovering over the GameObject and also when the pointer leaves.

using UnityEngine;
using UnityEngine.EventSystems;

public class XRFScanHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    // For Hover
    public static bool isHoveringScan;

    public Texture2D cursorIcon;
    private Texture2D defaultCursorIcon;

    private Vector2 hotspot;
    public CursorMode cursorMode = CursorMode.Auto;

    // ==================================
    // === CHECK IF CURSOR IS ON XRF ====
    // ==================================

    //Detect if the Cursor starts to pass over the GameObject
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        isHoveringScan = true;
        Cursor.SetCursor(cursorIcon, Vector2.zero + new Vector2(16, 16), cursorMode);
    }

    //Detect when Cursor leaves the GameObject
    public void OnPointerExit(PointerEventData pointerEventData)
    {
        isHoveringScan = false;
        Cursor.SetCursor(null, Vector2.zero, cursorMode);
    }

    // ==================================



}