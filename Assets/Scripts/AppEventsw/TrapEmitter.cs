using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapEmitter : MonoBehaviour {



    public void ExecuteTrap() {

        EventManager.TriggerEvent<TrapEvent, Vector3>(transform.position);
    }
}
