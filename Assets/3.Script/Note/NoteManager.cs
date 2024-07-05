using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    /*
    BPM 120 이라는 가정하에
    Beat Per Minute
    분 -> 60초
        노드 -> 4분음표(1beat)

    60/120 -> 0.5초 - 1개
     */

    public static NoteManager instance = null;

    public void Instance()
    {
        if(instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(this);
            return;
        }

        Notes = new Queue<GameObject>();
    }

    private void Awake()
    {
        Instance();
    }

    [Header("BPM을 설정해 주세욧!")]
    [SerializeField]public int BPM = 0;

    private double current_time = 0d;

    [Header("ETC")]
    [SerializeField] 
    private GameObject notePrefabs;

    private bool isnoteActive = true;

    [SerializeField]
    private Queue<GameObject> Notes; // 풀링용 노트큐

    [SerializeField] private int noteCapacity; // 필요한 노트양
    private readonly int divider = 20;
    private readonly Vector3 poolPos = new Vector3(0f, 2000f, 0f);

    [SerializeField]
    private Transform noteSpawner;
    private TimeManager timemanager;
    private ComboManger combo;
    private EffectManager effect;

    private void Start()
    {
        timemanager = FindObjectOfType<TimeManager>();
        combo = FindObjectOfType<ComboManger>();
        effect = FindObjectOfType<EffectManager>();

        if (BPM >= 20)
            noteCapacity = (BPM / divider) + 1;

        else
            noteCapacity = 1;

        InitNoteQueue();
    }



    private void Update()
    {
        if (isnoteActive)
        {
            current_time += Time.deltaTime;
            if (current_time >= (60d / BPM))
            {
                //******************************************************************************************************************
                //instantiate 방식
                //GameObject note_ob =
                //    Instantiate(notePrefabs, noteSpawner.position, Quaternion.identity);
                //note_ob.transform.SetParent(this.transform); // UI이기 때문에 부모 포지션으로 있어야한다(안 그럼 안 보임).

                //timemanager.boxnote_List.Add(note_ob);
                //instantiate 방식
                //******************************************************************************************************************

                DequeueNote();
                current_time -= (60d / BPM);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D col) // UI 콜라이더가 모두 2D이기 때문에
    {
        if(col.CompareTag("Note"))
        {
            if(col.TryGetComponent(out Note n))
            {
                if(n.GetNoteFlag())
                {
                    //Debug.Log("Miss");
                    effect.Judgement_Effect(4);
                    combo.ResetCombo();
                }
            }
            EnqueueNote(col.gameObject);

            //******************************************************************************************************************
            //instantiate 방식
            //timemanager.boxnote_List.Remove(col.gameObject);
            //Destroy(col.gameObject);
            //instantiate 방식
            //******************************************************************************************************************
        }
    }

    // 큐 초기화 함수
    private void InitNoteQueue()
    {
        GameObject note;

        for (int i = 0; i < noteCapacity; i++)
        {
            note = Instantiate(notePrefabs, poolPos, Quaternion.identity);
            Notes.Enqueue(note);
            note.transform.SetParent(this.transform);
            note.SetActive(false);
        }
    }

    // 큐 꺼내는 함수
    private void DequeueNote()
    {
        GameObject note = Notes.Dequeue();

        note.SetActive(true);
        note.transform.position = noteSpawner.position;

        timemanager.boxnote_List.Add(note);
    }

    // 큐 넣는 함수
    private void EnqueueNote(GameObject note)
    {
        timemanager.boxnote_List.Remove(note);
        Notes.Enqueue(note);
        note.transform.position = poolPos;
        note.SetActive(false);
    }

    public void Remove_Note()
    {
        isnoteActive = false;
        for (int i = 0; i < timemanager.boxnote_List.Count; i++)
        {
            EnqueueNote(timemanager.boxnote_List[i]);
        }
        timemanager.boxnote_List.Clear();
    }
}
