using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManger : MonoBehaviour
{
    /*
     * �޺��� Ȱ��ȭ(++)�Ǵ� ������?
     *  1. miss�� �ƴϸ� ī��Ʈ 

     *  �޺��� ��Ȱ��ȭ �Ǵ� ����
     *      1. miss�� ���´ٸ� Reset
     *      2. Bad�� ���ö��� �޺��� ������ ����...
     */
    [SerializeField] private GameObject combo_img;
    [SerializeField] private Text combo_text;

    private int current_combo = 0;

    private Animator ani;
    private string key = "combo";

    private void Start()
    {
        ani = GetComponent<Animator>();
        combo_img.SetActive(false);
        combo_text.gameObject.SetActive(false); // ������ ���� ������Ʈ�� �����ؼ� �����Ѵ�.
    }
    public void ResetCombo()
    {
        current_combo = 0;
        combo_text.text = string.Format("{0:#,##0}", current_combo);
        combo_img.SetActive(false);
        combo_text.gameObject.SetActive(false);
    }
    public void Addcombo(int combo = 1) // ����Ʈ �Ű����� (�Ű������� �ƹ��͵� �ۼ����� ������ �޺��� �Ű������� 1��)
    {
        current_combo += combo;
        combo_text.text = string.Format("{0:#,##0}", current_combo);

        if(current_combo >= 2)
        {
            combo_img.SetActive(true);
            combo_text.gameObject.SetActive(true);
            ani.SetTrigger(key);
        }
    }
}
