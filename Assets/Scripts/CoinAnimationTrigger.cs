using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinAnimationTrigger : MonoBehaviour
{
    private Animator coinAnimator;

    void Start()
    {
        coinAnimator = GetComponent<Animator>();
        coinAnimator.SetBool("isSpinning", false);
    }

    void OnTriggerEnter(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            CoinSpinner cs = c.attachedRigidbody.gameObject.GetComponent<CoinSpinner>();

            if (cs != null)
            {
                coinAnimator.SetBool("isSpinning", true);
                cs.SpinCoin();
            }
        }
    }

    void OnTriggerExit(Collider c)
    {
        if (c.attachedRigidbody != null)
        {
            CoinSpinner cs = c.attachedRigidbody.gameObject.GetComponent<CoinSpinner>();

            if (cs != null)
            {
                coinAnimator.SetBool("isSpinning", false);
                cs.StopSpinning();
            }
        }
    }
}
