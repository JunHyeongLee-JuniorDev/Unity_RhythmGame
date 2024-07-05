using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    /*게임종료
        1. 결과창 나타나게 해야함 -> result
        2. note가 더 이상 나오지 않도록 해야됨 -> 구현 X -> 해결 -> noteManager
        3. audio에 finish 넣어야됨 -> 구현 X 
        4. player 움직이면 안됨 -> iscanpresskey
     */

    private Result result;
    private NoteManager notemanager;
    private void Start()
    {
        result = FindObjectOfType<Result>();
        notemanager = FindObjectOfType<NoteManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            result.show_Result();
            notemanager.Remove_Note();
            AudioManager.instance.PlaySFX("Finish");
            PlayerControl.isCanPressKey = false;
        }
    }
}
