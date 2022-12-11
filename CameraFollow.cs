using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform objectToFollow;
    public Vector3 offset;
    public float followSpeed = 15;
    public float lookSpeed = 180;
    
    
    public void lookAtTarget()
    {
        Vector3 _lookDirection = objectToFollow.position - transform.position;
        Quaternion _rot = Quaternion.LookRotation(_lookDirection, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, _rot, lookSpeed * Time.deltaTime);
    }
    public void MoveToTarget()
    {
        Vector3 _TargetPose = objectToFollow.position + objectToFollow.forward * offset.z +
                              objectToFollow.right * offset.x + objectToFollow.up * offset.y;
        transform.position = Vector3.Lerp(transform.position, _TargetPose, followSpeed * Time.deltaTime);
    }
    private void FixedUpdate()
    {
        lookAtTarget();
        MoveToTarget();
    }
}
