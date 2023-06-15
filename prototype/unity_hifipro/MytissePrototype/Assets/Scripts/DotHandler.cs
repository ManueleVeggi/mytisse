using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class DotHandler : MonoBehaviour, IPointerEnterHandler
{
    public Color32 dotColor;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!PaintingCanvas.isBrushActive)
        {
            Debug.Log(gameObject.name);
            Destroy(gameObject);
        }
    }
   
    public List<object> GetInfoDot()
    {
        List<object> InfoDotList = new List<object>();

        Vector3 DotPos = transform.position;
        Vector3 DotSize = transform.localScale;
        // Color32 = GetComponent<Image>().color;

        InfoDotList.Add(DotPos.x);
        InfoDotList.Add(DotPos.y);
        InfoDotList.Add(DotSize.x);
        InfoDotList.Add(DotSize.y);
        InfoDotList.Add(dotColor);

        return InfoDotList;
    }
}