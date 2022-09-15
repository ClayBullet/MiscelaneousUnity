using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GraphicRaycasting : MonoBehaviour
{
    [SerializeField] private GraphicRaycaster raycastGraphic;
    [SerializeField] private EventSystem systemEvent;


    private void Awake()
    {
        GameManager.instance.raycastingGraphics = this;
    }
    public bool graphicDetectionBool()
    {


        PointerEventData myPointerEventData = new PointerEventData(systemEvent);

        myPointerEventData.position = Input.mousePosition;


        List<RaycastResult> results = new List<RaycastResult>();

        raycastGraphic.Raycast(myPointerEventData, results);



        return results.Count > 0;

    }

    public bool detectAConcretObject(GameObject obj)
    {
        PointerEventData myPointerEventData = new PointerEventData(systemEvent);

        myPointerEventData.position = Input.mousePosition;


        List<RaycastResult> results = new List<RaycastResult>();

        raycastGraphic.Raycast(myPointerEventData, results);


        for (int i = 0; i < results.Count; i++)
        {
            if (results[i].gameObject == obj) return true;
        }

        return false;
    }

}
