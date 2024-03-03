using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinSpinner : MonoBehaviour
{
    public bool spinCoin = false;

    public void SpinCoin()
    {
        spinCoin = true;
    }

    public void StopSpinning()
    {
        spinCoin = false;
    }
}