using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    /*
     * Animator -> trigger Hit ȣ��
     */

    [SerializeField] private Animator notehit_animator;
    [SerializeField] private Animator Judgement_animator;
    [SerializeField] private Image Judgement_img;

    private string Key = "Hit";

    [Header("Perfect -> Cool -> Good -> Bad")]
    [SerializeField] private Sprite[] Judgement_sprite; // �����Ҵ� ����

    private void Awake()
    {
        /*
         TryGetComponent�� ���� ����

        Equals �� == �� ���̿� ����
         */

        //notehit_animator = transform.GetChild(0).GetComponent<Animator>();
        //                 ��> �̶� null ����  -> null�� notehit_animator�� ���� null ���� ���� �ϸ� �ð��� �ɸ�

        transform.GetChild(0).TryGetComponent(out notehit_animator);
        // null pointer�� ������ ������ ����
        // �䷡ ���� ������
        // ��� : TryGetComponent�� 20�� ������

        transform.GetChild(1).TryGetComponent(out Judgement_animator);
        transform.GetChild(1).TryGetComponent(out Judgement_img);
    }

    public void Judgement_Effect(int effect)
    {
        Judgement_img.sprite = Judgement_sprite[effect];
        Judgement_animator.SetTrigger(Key);
    }

    public void NoteHit_Effect()
    {
        notehit_animator.SetTrigger(Key);

    }
}
