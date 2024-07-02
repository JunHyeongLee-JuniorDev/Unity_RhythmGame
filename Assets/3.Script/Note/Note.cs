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
         * UI�� ĵ������ �ڽ��̾����
         * Local pos �� �Űܾ��Ѵ� (ĵ������ ����� �ޱ� ����)
         * ������ �Ʒ��� �߸��� ����̴�.(�۷ι��� �ű��)

        //transform.position += 
        //    Vector3.right * note_speed * Time.deltaTime;
         */
    }
}
