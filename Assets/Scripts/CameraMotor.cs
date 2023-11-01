using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public Transform lookAt; //Pengu or player character
    public Vector3 offset = new Vector3(0, 0f, 0f);
    public Vector3 rotation = new Vector3(25f, 0f, 0f);

    public bool IsMoving {  set; get; }

    private void Start()
    {
        //transform.position = lookAt.position + offset;
    }

    private void LateUpdate()
    {
        if (!IsMoving)
            return;

        Vector3 desiredPosition = lookAt.position + offset;
        desiredPosition.x = 0;
        transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(rotation), 0.1f);
        //transform.LookAt(lookAt);
    }
}
