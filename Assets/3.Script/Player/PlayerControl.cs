using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    public static bool isCanPressKey = true;

    [Header("�̵�")]
    [SerializeField] private float MoveSpeed = 3f;

    //Ű������ �Է°� ������ �÷��̾ �����̴� ���� �ٸ���.
    [SerializeField] private Vector3 Input_Direction = new Vector3();
    //Ű���� �Է�

    [SerializeField] private Vector3 Dest_pos = new Vector3();
    public Vector3 destpos => Dest_pos;
    //���� �����̴� ����

    [Header("ȸ��")]
    [SerializeField] private float SpinSpeed = 270f;
    [SerializeField] private Vector3 input_rotDirection = new Vector3(); // �������� Vector3
    [SerializeField] private Quaternion Dest_rot = new Quaternion();//ȸ���� ���ʹϾ�

    [SerializeField] private Transform fakeCube;
    [SerializeField] private Transform realCube;

    private bool isCanMove = true;
    private bool isCanRot = true;

    [Header("ȿ��")]
    [SerializeField] private float EffectPos_Y = 0.25f;// ���� ȿ��
    [SerializeField] private float Effect_Speed = 1.5f;// ���� ȿ��

    private bool isFalling = false;

    //**************************************************************
    private Rigidbody player_r;

    private Vector3 Origin_pos; // �ʱ� ��ġ

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
        // �Է� ���� �� Ÿ�̹� ���� 
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

    private void Clac() // �÷��̾� ������ ��ǥ���� ���
    {
        //input�� ���� ������ -> ���⺤��
        Input_Direction.Set
            (
            Input.GetAxisRaw("Horizontal"),
            0,
            Input.GetAxisRaw("Vertical")
            );
        // �̵� ��ǥ�� ���
        Dest_pos =
            transform.position +
            new Vector3
            (Input_Direction.x,
            0,
            Input_Direction.z);

        // ȸ�� ��ǥ�� ���
        // �¿� -> z��
        // �յ� -> x��
        input_rotDirection =
            new Vector3
            (-Input_Direction.z,
            0,
            Input_Direction.x);
        // ��ǥ ȸ������ ������ ���� ���ؼ� ������ transform�� ���� ����...
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
        while(Vector3.SqrMagnitude(transform.position - Dest_pos) >= 0.001f) // ���̸� ��� �޼ҵ� �� ����
        {
            transform.position =
                Vector3.MoveTowards(transform.position, Dest_pos, MoveSpeed * Time.deltaTime);

            // deltatime : ����� ���� ������ ������ �ð�
            yield return null; // ???
            // deltatime�� ��Ȯ�ϰ� ����ϱ� ���ؼ�
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
                //������ raycast�� ���� �� ����Ǵ� ������Ʈ�� ���ٸ�
                Falling();
                Debug.Log("���! ��������");
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

