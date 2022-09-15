using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [HideInInspector] public RadialInterface interfaceRadial;
    [HideInInspector] public GraphicRaycasting raycastingGraphics;
    [HideInInspector] public InputManager managerInput;

    private void Awake()
    {
        instance = this;
    }
}
