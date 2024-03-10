using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    private int count = 0;
    private int orbs = 0;
    public TextMeshProUGUI countText;
    public TextMeshProUGUI orbText;

    void Start()
    {
        SetCountText();
        SetOrbText();
    }

    public void ReceiveCoin()
    {
        count += 100;
    }

    public void ReceiveOrb()
    {
        orbs += 1;
    }

    public void SetCountText()
    {
        countText.text = "Amount earned: $" + count.ToString();
    }

    public void SetOrbText()
    {
        orbText.text = "Orbs: " + orbs.ToString();
    }
}