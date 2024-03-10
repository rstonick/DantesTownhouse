using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAt_GW : MonoBehaviour {

    //Useful for improving scapula angle

    public Transform target;
    public Vector3 targetUpVector = Vector3.up;
    public Vector3 rotationOffset = Vector3.zero;

    void LateUpdate () {

        if (target)
        {
            transform.LookAt(target.position, target.transform.right * targetUpVector.x + target.transform.up * targetUpVector.y + target.transform.forward * targetUpVector.z);
            transform.Rotate(rotationOffset);
        }

    }
}
