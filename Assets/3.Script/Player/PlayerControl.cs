using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    private TimeManager timemanager;

    private void Start()
    {
        timemanager = FindObjectOfType<TimeManager>();
    }

    private void Update()
    {
        // 입력 했을 때 타이밍 판정 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            timemanager.checkTimming();
        }
    }
}
