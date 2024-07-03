using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //Timing -> 노트 판정 -> 노드의 현재 position의 기준으로 판정을 내림
    [Header("Perfect -> cool -> good -> bad")]
    [SerializeField] private RectTransform[] timmingRect;
    private Vector2[] TimeBox;

    [SerializeField] private RectTransform Center;

             
    public List<GameObject> boxnote_List = new List<GameObject>(); //타임 메니저에서 어떤 노트인지 담기위해서
    [SerializeField] private EffectManager effect;
    [SerializeField] private ScoreManager score;
    [SerializeField] private TimeManager timeManager;
    
    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }
    private void Start()
    {
        effect = FindObjectOfType<EffectManager>();
        score = FindObjectOfType<ScoreManager>();
        //*************************************************************************
        TimeBox = new Vector2[timmingRect.Length];
        //최소값과 최대값 (가로 길이의)
        //이미지의 중심을 기준으로 +- (이미지너비 / 2)
        for(int i=0;i < timmingRect.Length;i++)
        {
            TimeBox[i].Set
                (
                    Center.localPosition.x - (timmingRect[i].rect.width / 2),
                    Center.localPosition.x + (timmingRect[i].rect.width / 2)
                );
        }
    }


    public bool checkTimming()
    {
        for(int i=0; i< boxnote_List.Count;i++)
        {
            float notepos_X = boxnote_List[i].transform.localPosition.x;
           for(int j=0;j < TimeBox.Length;j++)
            {
                // 현재 timebox의 범위 안에 있다면
                if((TimeBox[j].x <= notepos_X) && (notepos_X <= TimeBox[j].y))
                {
                    if(boxnote_List[i].transform.TryGetComponent(out Note n))
                    {
                        n.HideNote();
                    }
                    //판정이 난 상황
                    effect.NoteHit_Effect();
                    effect.Judgement_Effect(j);
                    score.AddScore(j);
                        return true;
                }
            }
        }
        return false;
    }

    public string Debug_Note(int x)
    {
        switch(x)
        {
            case 0:
                return "Perfect";

            case 1:
                return "Cool";

            case 2:
                return "Good";

            case 3:
                return "Bad";

            default:
                return string.Empty;
        }
    }
}
