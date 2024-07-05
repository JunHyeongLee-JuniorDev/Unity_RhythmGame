using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Result : MonoBehaviour
{
    [SerializeField] private GameObject UI_ob;

    [SerializeField] private Text[] Text_count;
    [SerializeField] private Text Score_Text;
    [SerializeField] private Text Maxcombo_Text;


    private ScoreManager score;
    private ComboManger combo;
    private TimeManager timeManager;

    private void Start()
    {
        score = FindObjectOfType<ScoreManager>();
        combo = FindObjectOfType<ComboManger>();
        timeManager = FindObjectOfType<TimeManager>();
    }

    private void Init_UI()
    {
        for (int i = 0; i < Text_count.Length;i++)
        {
            Text_count[i].text = "0";
        }
        Score_Text.text = "0";
        Maxcombo_Text.text = "0";
    }

    private string StringFormat(string s)
    {
        return string.Format("{0:#,##0", s);
    }

    public void show_Result()
    {
        AudioManager.instance.stopBGM();
        Init_UI();
        UI_ob.SetActive(true);

        //판정기록
        int[] record_arr = timeManager.Get_Judgement_Record();

        for(int i=0; i < record_arr.Length;i++)
        {
            Text_count[i].text = StringFormat(record_arr[i].ToString());
        }
        Score_Text.text = StringFormat(score.GetScore().ToString());
        Maxcombo_Text.text = StringFormat(combo.GetMaxCombo().ToString());

    }
}


