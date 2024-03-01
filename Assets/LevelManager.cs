using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public bool debugMode;
    private void Start()
    {
        Application.targetFrameRate = 60;
        Cursor.visible = true;        
    }
    private void Update()
    {
        
    }
}
