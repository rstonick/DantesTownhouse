using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpAngle_GW : MonoBehaviour {

    //Useful for interpolating between two referenced bones, used on Patella bone;

    public Transform bone01;
    public Transform bone02;

    public float interpolationValue = 0.5f;

    void LateUpdate () {

        if (bone01 && bone02)
        {
            transform.rotation = Quaternion.Slerp(bone01.rotation, bone02.rotation, interpolationValue);
        }
	}
}
