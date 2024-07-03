using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EffectManager : MonoBehaviour
{
    /*
     * Animator -> trigger Hit 호출
     */

    [SerializeField] private Animator notehit_animator;
    [SerializeField] private Animator Judgement_animator;
    [SerializeField] private Image Judgement_img;

    private string Key = "Hit";

    [Header("Perfect -> Cool -> Good -> Bad")]
    [SerializeField] private Sprite[] Judgement_sprite; // 직접할당 예정

    private void Awake()
    {
        /*
         TryGetComponent가 빠른 이유

        Equals 와 == 의 차이와 동의
         */

        //notehit_animator = transform.GetChild(0).GetComponent<Animator>();
        //                 ㄴ> 이때 null 생성  -> null에 notehit_animator을 넣음 null 생성 제거 하며 시간이 걸림

        transform.GetChild(0).TryGetComponent(out notehit_animator);
        // null pointer가 생성될 이유가 없다
        // 요래 쓰면 좋아함
        // 결론 : TryGetComponent가 20배 빠르다

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
