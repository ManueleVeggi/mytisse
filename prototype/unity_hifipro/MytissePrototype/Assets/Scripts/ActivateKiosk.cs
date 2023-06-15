using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActivateKiosk : MonoBehaviour
{
    public GameObject player;
    public GameObject UIManager;
    private GameObject[] canvas;

    void Start()
    {
        UIManager.SetActive(false);
    }

    private void OnTriggerEnter(Collider player)
    {
        List<Material> myMaterials = GetComponent<Renderer>().materials.ToList();
        myMaterials[1].SetColor("_Color", Color.white);
        UIManager.SetActive(true);

    }

    private void OnTriggerExit(Collider player)
    {
        List<Material> myMaterials = GetComponent<Renderer>().materials.ToList();
        myMaterials[1].SetColor("_Color", Color.black);
        UIManager.SetActive(false);
    }

    public void TurnOffScreen()
    {
        List<Material> myMaterials = GetComponent<Renderer>().materials.ToList();
        myMaterials[1].SetColor("_Color", Color.black);
    }
}
