using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpEmitter : MonoBehaviour {



    public void ExecuteJump() {

        EventManager.TriggerEvent<JumpEvent, Vector3>(transform.position);
    }
}
