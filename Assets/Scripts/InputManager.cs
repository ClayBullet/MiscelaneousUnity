using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public GameObject prevInteractableObject;
    public GameObject currentInteractableObject;

    private void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Input.GetKeyDown(KeyCode.Mouse0) && !GameManager.instance.raycastingGraphics.graphicDetectionBool())
        {
            GameManager.instance.interfaceRadial.detectionRadio.PressRadialBtn();

            if (Physics.Raycast(ray, out hit))
            {
                if (GameManager.instance.interfaceRadial.identifyLikeRadialActionAndValorateIfIsAvailableToUse(hit.collider.gameObject)) return;


                currentInteractableObject = hit.collider.gameObject;

                if (prevInteractableObject == null)
                    prevInteractableObject = currentInteractableObject;
                else if(prevInteractableObject != currentInteractableObject)
                {
                    prevInteractableObject = currentInteractableObject;
                }

                GameManager.instance.interfaceRadial.currentObjToFocus = hit.collider.transform;

            }
        }

    }
}
