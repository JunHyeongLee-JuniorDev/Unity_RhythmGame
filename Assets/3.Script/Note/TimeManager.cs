using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    //Timing -> ��Ʈ ���� -> ����� ���� position�� �������� ������ ����
    [Header("Perfect -> cool -> good -> bad")]
    [SerializeField] private RectTransform[] timmingRect;
    private Vector2[] TimeBox;

    [SerializeField] private RectTransform Center;

             
    public List<GameObject> boxnote_List = new List<GameObject>(); //Ÿ�� �޴������� � ��Ʈ���� ������ؼ�

    private int[] Judgement_Record = new int[5];

    [SerializeField] private EffectManager effect;
    [SerializeField] private ScoreManager score;
    [SerializeField] private TimeManager timeManager;
    [SerializeField] private ComboManger combo;
    [SerializeField] private PlayerControl Player;
    [SerializeField] private StageControl stage;

    private void Awake()
    {
        timeManager = FindObjectOfType<TimeManager>();
    }

    private void Start()
    {
        effect = FindObjectOfType<EffectManager>();
        score = FindObjectOfType<ScoreManager>();
        combo = FindObjectOfType<ComboManger>();
        Player = FindObjectOfType<PlayerControl>();
        stage = FindObjectOfType<StageControl>();

        //*************************************************************************
        TimeBox = new Vector2[timmingRect.Length];
        //�ּҰ��� �ִ밪 (���� ������)
        //�̹����� �߽��� �������� +- (�̹����ʺ� / 2)
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
                // ���� timebox�� ���� �ȿ� �ִٸ�
                if ((TimeBox[j].x <= notepos_X) && (notepos_X <= TimeBox[j].y))
                {
                    if (boxnote_List[i].transform.TryGetComponent(out Note n))
                    {
                        n.HideNote();
                    }
                    //������ �� ��Ȳ
                    effect.NoteHit_Effect();

                    if (Check_NextPlate())
                    {
                        effect.Judgement_Effect(j);
                        score.AddScore(j);
                        stage.ShowNextPlate();
                        // j : 0 -> perfect, 1 -> cool, 2 -> good, 3 -> bad
                        if (j < 3)
                        {
                            combo.Addcombo(); // ����Ʈ ���ڰ� �ֱ� ������ �Ƚᵵ ��
                        }

                        else
                        {
                            combo.ResetCombo();
                        }
                        Judgement_Record[j]++;
                        return true;
                    }

                    else
                    {
                        effect.Judgement_Effect(5);
                        combo.ResetCombo();
                    }
                }
            }
        }
        return false;
    }

    /*
     * normal ����
     * ???
     */

    public bool Check_NextPlate()
    {

        if (Physics.Raycast(Player.destpos, Vector3.down,out RaycastHit h, 1.1f))
        {
            Debug.DrawRay(Player.destpos, Vector3.down, Color.red , 1.1f);
            if(h.transform.CompareTag("BasicPlate"))
            {
                if(h.transform.TryGetComponent(out Basic_Plate b))
                {
                    if(b.flag)
                    {
                        b.flag = false;
                        return true;
                    }
                }
            }
        }
        combo.ResetCombo();
        Judgement_Miss();
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

    private void Judgement_Miss()
    {
        Judgement_Record[4]++;
    }

    public int[] Get_Judgement_Record()
    {
        return Judgement_Record;
    }
}
