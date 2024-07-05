using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageControl : MonoBehaviour
{
    [SerializeField] private GameObject Stage;
    private Transform[] Stage_Plate;

    [SerializeField] private float Offset_Y = -3f; // �ؿ��� ������ �� y pos
    [SerializeField] private float Plate_Speed = 10f; // �÷���Ʈ �Ʒ��� ���� �ö���� ���ǵ�

    //���� ���� �÷���Ʈ�� ��
    [SerializeField] private int Step_Count = 0;
    //��� �÷���Ʈ�� ��
    [SerializeField] private int totalPlateCount = 0;

    private void Start()
    {
        Setting_Stage();
    }

    /*
     * Step_Count�� �� ������ ++
     * �ʱ� 2���� �̸� ���?
     * 
     */

    private void Setting_Stage()
    {
        Step_Count = 2;
        // �ʱ⼼�� 
        Stage_Plate = Stage.GetComponent<Stage>().Plate;
        totalPlateCount = Stage_Plate.Length;

        for(int i=0; i<totalPlateCount;i++)
        {
            if(!Stage_Plate[i].gameObject.activeSelf) // docs ã�ƺ���
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
        //���� �������� ����� �ݽô�.

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
            yield return null; // Time.deltaTime�̱� ������
        }
        Stage_Plate[index].position = destPos;
    }
}
