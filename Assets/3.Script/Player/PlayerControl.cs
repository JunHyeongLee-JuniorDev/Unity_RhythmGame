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
        // �Է� ���� �� Ÿ�̹� ���� 
        if(Input.GetKeyDown(KeyCode.Space))
        {
            timemanager.checkTimming();
        }
    }
}
