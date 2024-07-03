using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Animator ani;

    private string key = "Score";

    private int Current_Score = 0;
    private int Default_Score = 10;

    [SerializeField] private float[] weight;

    private void Awake()
    {
        ani = GetComponent<Animator>();
        scoreText = transform.GetChild(0).GetComponent<Text>();
    }

    public void AddScore(int index)
    {
        int _score = (int)(Default_Score * weight[index]);

        Current_Score += _score;

        scoreText.text = string.Format("{0:#,##0}", Current_Score);
        //                                  ㄴ>어떤 방식으로 쓸지정하기
        ani.SetTrigger(key);

    }
}
