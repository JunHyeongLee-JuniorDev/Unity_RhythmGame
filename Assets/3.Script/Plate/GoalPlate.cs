using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalPlate : MonoBehaviour
{
    /*��������
        1. ���â ��Ÿ���� �ؾ��� -> result
        2. note�� �� �̻� ������ �ʵ��� �ؾߵ� -> ���� X -> �ذ� -> noteManager
        3. audio�� finish �־�ߵ� -> ���� X 
        4. player �����̸� �ȵ� -> iscanpresskey
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
