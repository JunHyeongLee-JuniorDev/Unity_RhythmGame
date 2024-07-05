using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] private Transform Target;
    [SerializeField] private float followSpeed = 15f;

    private Vector3 playerToDistance = new Vector3();

    private float HitDistance = 0;
    [SerializeField] private float zoomDistance = 1.25f;

    private void Start()
    {
        playerToDistance = (transform.position - Target.position);

    }

    private void Update()
    {
        Vector3 destpos =
            Target.position + playerToDistance + (transform.forward * HitDistance);

        transform.position =
            Vector3.Lerp(transform.position, destpos, followSpeed * Time.deltaTime);
    }

    public IEnumerator zoomCam_co()
    {
        HitDistance = zoomDistance;
        yield return new WaitForSeconds(0.15f);
        HitDistance = 0;
    }


}
