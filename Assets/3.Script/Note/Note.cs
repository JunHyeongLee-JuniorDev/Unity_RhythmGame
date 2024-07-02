using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Note : MonoBehaviour
{
    [SerializeField] private float note_speed = 400f;

    private Image img;

    private void OnEnable()
    {
        img = GetComponent<Image>();
        img.enabled = true;
    }

    public void HideNote()
    {
        img.enabled = false;
    }

    public bool GetNoteFlag()
    {
        return img.enabled;
    }

    private void Update()
    {
        transform.localPosition += Vector3.right * note_speed * Time.deltaTime;

        /* 
         * UI는 캔버스의 자식이어야함
         * Local pos 로 옮겨야한다 (캔버스의 상속을 받기 때문)
         * 때문에 아래는 잘못된 방법이다.(글로벌로 옮기기)

        //transform.position += 
        //    Vector3.right * note_speed * Time.deltaTime;
         */
    }
}
