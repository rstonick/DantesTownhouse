using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableCoin : MonoBehaviour
{
    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            CoinCollector cc = c.attachedRigidbody.gameObject.GetComponent<CoinCollector>();

            if (cc != null)
            {
                // EventManager.TriggerEvent<BombBounceEvent, Vector3>(c.transform.position);
                Destroy(this.gameObject);
                cc.ReceiveCoin();
            }
        }
    }
}
