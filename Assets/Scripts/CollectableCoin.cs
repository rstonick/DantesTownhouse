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
                Destroy(this.gameObject);
                cc.ReceiveCoin();
                cc.SetCountText();
            }
        }
    }
}
