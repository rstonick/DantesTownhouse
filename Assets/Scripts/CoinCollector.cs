using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    public bool hasCoin = false;

    public void ReceiveCoin()
    {
        hasCoin = true;
    }
}