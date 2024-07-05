using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{
    [SerializeField] private GameObject Stage;
    private Transform[] Stage_Plate;

    [SerializeField] private float Offset_Y = -3f; // 밑에서 생성될 때 y pos
    [SerializeField] private float Plate_Speed = 10f; // 플레이트 아래서 위로 올라오는 스피드

    //현재 밟은 플레이트의 수
    [SerializeField] private int Step_Count = 0;
    //모든 플레이트의 수
    [SerializeField] private int totalPlateCount = 0;

    private void Start()
    {
        Setting_Stage();
    }

    /*
     * Step_Count를 갈 때마다 ++
     * 초기 2개는 미리 계산?
     * 
     */

    private void Setting_Stage()
    {
        Step_Count = 2;
        // 초기세팅 
        Stage_Plate = Stage.GetComponent<Stage>().Plate;
        totalPlateCount = Stage_Plate.Length;

        for(int i=0; i<totalPlateCount;i++)
        {
            if(!Stage_Plate[i].gameObject.activeSelf) // docs 찾아보기
            {
                Stage_Plate[i].position =
                    new Vector3(Stage_Plate[i].position.x,
                    Stage_Plate[i].position.y + Offset_Y,
                    Stage_Plate[i].position.z);
            }
        }

    }


    public void ShowNextPlate()
    {
        if(Step_Count < totalPlateCount)
        {
            StartCoroutine(MovePlate_co(Step_Count++)); //???
        }
    }
    private IEnumerator MovePlate_co(int index)
    {
        Stage_Plate[index].gameObject.SetActive(true);
        //목적 포지션을 만들어 줍시다.

        Vector3 destPos = new Vector3(Stage_Plate[index].position.x,
            Stage_Plate[index].position.y - Offset_Y,
            Stage_Plate[index].position.z);

        while (Vector3.SqrMagnitude(Stage_Plate[index].position - 
            destPos) >= 0.001f)
        {
            Stage_Plate[index].position =
                Vector3.Lerp(Stage_Plate[index].position,
                destPos,
                Plate_Speed * Time.deltaTime);
            yield return null; // Time.deltaTime이기 때문에
        }
        Stage_Plate[index].position = destPos;
    }
}
