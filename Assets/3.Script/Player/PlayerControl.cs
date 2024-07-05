using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static bool isCanPressKey = true;

    [Header("이동")]
    [SerializeField] private float MoveSpeed = 3f;

    //키보드의 입력과 실제로 플레이어가 움직이는 것은 다르다.
    [SerializeField] private Vector3 Input_Direction = new Vector3();
    //키보드 입력

    [SerializeField] private Vector3 Dest_pos = new Vector3();
    public Vector3 destpos => Dest_pos;
    //실제 움직이는 방향

    [Header("회전")]
    [SerializeField] private float SpinSpeed = 270f;
    [SerializeField] private Vector3 input_rotDirection = new Vector3(); // 포지션은 Vector3
    [SerializeField] private Quaternion Dest_rot = new Quaternion();//회전은 쿼터니언

    [SerializeField] private Transform fakeCube;
    [SerializeField] private Transform realCube;

    private bool isCanMove = true;
    private bool isCanRot = true;

    [Header("효과")]
    [SerializeField] private float EffectPos_Y = 0.25f;// 점프 효과
    [SerializeField] private float Effect_Speed = 1.5f;// 점프 효과

    private bool isFalling = false;

    //**************************************************************
    private Rigidbody player_r;

    private Vector3 Origin_pos; // 초기 위치

    //**************************************************************
    private TimeManager timemanager;
    private CameraManager Camera;

    private void Start()
    {
        timemanager = FindObjectOfType<TimeManager>();
        Camera = FindObjectOfType<CameraManager>();
    //**************************************************************
        player_r = GetComponentInChildren<Rigidbody>();
        Origin_pos = transform.position;
    }

    private void Update()
    {
        // 입력 했을 때 타이밍 판정 
        //if(Input.GetKeyDown(KeyCode.Space))
        //{
        //    timemanager.checkTimming();
        //}
        Check_Falling();
        if(Input.GetKeyDown(KeyCode.W) ||
            Input.GetKeyDown(KeyCode.A) ||
            Input.GetKeyDown(KeyCode.S) ||
            Input.GetKeyDown(KeyCode.D))
        {
            if (isCanMove && isCanRot && isCanPressKey)
            {
                AudioManager.instance.PlaySFX("Clap");
                Clac();

                if (timemanager.checkTimming())
                {
                    StartAction();
                }
            }
        }

    }

    private void Clac() // 플레이어 움직임 목표값을 계산
    {
        //input에 따른 방향계산 -> 방향벡터
        Input_Direction.Set
            (
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
            );
        // 이동 목표값 계산
        Dest_pos =
            transform.position +
            new Vector3
            (Input_Direction.x,
            0,
            Input_Direction.z);

        // 회전 목표값 계산
        // 좌우 -> z축
        // 앞뒤 -> x축
        input_rotDirection =
            new Vector3
            (-Input_Direction.z,
            0,
            Input_Direction.x);
        // 목표 회전값을 가지고 오기 위해서 임의의 transform을 먼저 돌림...
        fakeCube.RotateAround(transform.position,
            input_rotDirection,
            SpinSpeed);

        Dest_rot = fakeCube.rotation;
    }

    private void StartAction()
    {
        StartCoroutine(Move_co());
        StartCoroutine(Spin_co());
        StartCoroutine(Effect_co());
        StartCoroutine(Camera.zoomCam_co());
    }

    private IEnumerator Move_co()
    {
        isCanMove = false;
        while(Vector3.SqrMagnitude(transform.position - Dest_pos) >= 0.001f) // 길이를 재는 메소드 더 빠름
        {
            transform.position =
                Vector3.MoveTowards(transform.position, Dest_pos, MoveSpeed * Time.deltaTime);

            // deltatime : 현재와 이전 프레임 사이의 시간
            yield return null; // ???
            // deltatime을 정확하게 사용하기 위해서
        }
        transform.position = Dest_pos;
        isCanMove = true;
    }

    private IEnumerator Spin_co()
    {
        isCanRot = false;
        while(Quaternion.Angle(realCube.rotation, Dest_rot) > 0.5f)
        {
            realCube.rotation =
                Quaternion.RotateTowards(realCube.rotation, Dest_rot,
                SpinSpeed * Time.deltaTime);
            yield return null;
        }
        realCube.rotation = Dest_rot;
        isCanRot = true;
    }

    private IEnumerator Effect_co()
    {
        while(realCube.position.y < EffectPos_Y)
        {
            realCube.position += new Vector3(0, Effect_Speed * Time.deltaTime);
            yield return null;
        }
        while(realCube.position.y > 0)
        {
            realCube.position -= new Vector3(0,Effect_Speed * Time.deltaTime, 0);
            yield return null;
        }
        realCube.localPosition = Vector3.zero;
    }

    private void Check_Falling()
    {
        if(!isFalling && isCanMove)
        {
            if(!Physics.Raycast(transform.position, Vector3.down, 1.1f))
            {
                //밑으로 raycast를 쐈을 때 검출되는 오브젝트가 없다면
                Falling();
                Debug.Log("우왕! 떨어진당");
            }
        }
    }
    private void Falling()
    {
        isFalling = true;
        player_r.useGravity = true;
        player_r.isKinematic = false;
    }

    public void Reset_Falling()
    {
        AudioManager.instance.PlaySFX("Falling");
        isFalling = false;
        player_r.useGravity = false;
        player_r.isKinematic = true;

        transform.position = Origin_pos;
        realCube.localPosition = Vector3.zero;
    }
}

