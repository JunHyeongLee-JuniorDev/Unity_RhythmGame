using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ComboManger : MonoBehaviour
{
    /*
     * 콤보가 활성화(++)되는 시점은?
     *  1. miss가 아니면 카운트 

     *  콤보가 비활성화 되는 시점
     *      1. miss가 나온다면 Reset
     *      2. Bad가 나올때는 콤보를 셈하지 않음...
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
        combo_text.gameObject.SetActive(false); // 무조건 게임 오브젝트에 접근해서 꺼야한다.
    }
    public void ResetCombo()
    {
        current_combo = 0;
        combo_text.text = string.Format("{0:#,##0}", current_combo);
        combo_img.SetActive(false);
        combo_text.gameObject.SetActive(false);
    }
    public void Addcombo(int combo = 1) // 디폴트 매개변수 (매개변수에 아무것도 작성하지 않으면 콤보의 매개변수는 1로)
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
