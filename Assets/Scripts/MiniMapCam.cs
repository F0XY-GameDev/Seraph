using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MiniMapCam : MonoBehaviour
{
    public Image mapBackground;
    public Color bigColor;
    public Color smallColor;
    Camera cam;
    CustomInput input;

    private void Awake()
    {
        input = new CustomInput();
    }
    private void Start()
    {
        cam = GetComponent<Camera>();
    }
    private void OnEnable()
    {
        input.Enable();
        input.Player.ToggleMap.performed += OnToggleMap;
    }

    private void OnToggleMap(InputAction.CallbackContext value)
    {
        if (cam.orthographicSize != 90)
        {
            cam.orthographicSize = 90;
            mapBackground.color = bigColor;
        } else
        {
            cam.orthographicSize = 40;
            mapBackground.color = smallColor;
        }
    }
}
