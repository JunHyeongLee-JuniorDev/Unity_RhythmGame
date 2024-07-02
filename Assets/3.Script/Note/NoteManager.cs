using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteManager : MonoBehaviour
{
    /*
    BPM 120 �̶�� �����Ͽ�
    Beat Per Minute
    �� -> 60��
        ��� -> 4����ǥ(1beat)

    60/120 -> 0.5�� - 1��
     */

    public static NoteManager instance = null;

    public void Instance()
    {
        if(instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(this);
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

    [Header("BPM�� ������ �ּ���!")]
    [SerializeField]public int BPM = 0;

    private double current_time = 0d;

    [Header("ETC")]
    [SerializeField] 
    private GameObject notePrefabs;

    [SerializeField]
    private Queue<GameObject> Notes; // Ǯ���� ��Ʈť

    [SerializeField] private int noteCapacity; // �ʿ��� ��Ʈ��
    private readonly int divider = 20;
    private readonly Vector3 poolPos = new Vector3(0f, 2000f, 0f);

    [SerializeField]
    private Transform noteSpawner;
    private TimeManager timemanager;

    private void Start()
    {
        timemanager = FindObjectOfType<TimeManager>();

        if (BPM >= 20)
            noteCapacity = (BPM / divider) + 1;

        else
            noteCapacity = 1;

        InitNoteQueue();
    }



    private void Update()
    {
        current_time += Time.deltaTime;
        if(current_time >= (60d / BPM))
        {
            //******************************************************************************************************************
            //instantiate ���
            //GameObject note_ob =
            //    Instantiate(notePrefabs, noteSpawner.position, Quaternion.identity);
            //note_ob.transform.SetParent(this.transform); // UI�̱� ������ �θ� ���������� �־���Ѵ�(�� �׷� �� ����).

            //timemanager.boxnote_List.Add(note_ob);
            //instantiate ���
            //******************************************************************************************************************

            DequeueNote();
            current_time -= (60d / BPM);
        }
    }

    private void OnTriggerEnter2D(Collider2D col) // UI �ݶ��̴��� ��� 2D�̱� ������
    {
        if(col.CompareTag("Note"))
        {
            if(col.TryGetComponent(out Note n))
            {
                if(n.GetNoteFlag())
                {
                    Debug.Log("Miss");
                }
            }
            EnqueueNote(col.gameObject);

            //******************************************************************************************************************
            //instantiate ���
            //timemanager.boxnote_List.Remove(col.gameObject);
            //Destroy(col.gameObject);
            //instantiate ���
            //******************************************************************************************************************
        }
    }

    // ť �ʱ�ȭ �Լ�
    private void InitNoteQueue()
    {
        GameObject note;

        for (int i = 0; i < noteCapacity; i++)
        {
            note = Instantiate(notePrefabs, poolPos, Quaternion.identity);
            Notes.Enqueue(note);
            note.SetActive(false);
        }
    }

    // ť ������ �Լ�
    private void DequeueNote()
    {
        GameObject note = Notes.Dequeue();
        Image noteImg = note.GetComponent<Image>();

        note.SetActive(true);
        noteImg.enabled = true;
        note.transform.position = noteSpawner.position;
        note.transform.SetParent(this.transform);

        timemanager.boxnote_List.Add(note);
    }

    // ť �ִ� �Լ�
    private void EnqueueNote(GameObject note)
    {
        timemanager.boxnote_List.Remove(note);
        Notes.Enqueue(note);
        note.transform.position = poolPos;
        note.SetActive(false);
    }
}