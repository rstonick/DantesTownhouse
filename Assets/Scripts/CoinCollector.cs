using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinCollector : MonoBehaviour
{
    public int count = 0;
    public TextMeshProUGUI countText;
    public string tableTag;

    public float lowRiskChance = 0.2f;
    public float mediumRiskChance = 0.5f;
    public float highRiskChance = 0.8f;
    public int lowRewardMultiplier = 2;
    public int mediumRewardMultiplier = 3;
    public int highRewardMultiplier = 5;

    void Start()
    {
        SetCountText();
    }

    public void ReceiveCoin()
    {
        count += 100;
    }

    public void SetCountText()
    {
        countText.text = "Amount earned: $" + count.ToString();
    }

    public void SetTableTag(string tag)
    {
        tableTag = tag;
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("Setting tag");
        SetTableTag(other.tag);
    }

    void Update()
    {
        if (DialogueManager.instance != null)
        {
            if (DialogueManager.instance.IsDialogueActive())
            {
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
                {
                    if (tableTag == "Table1")
                    {
                        count *= lowRewardMultiplier;
                    }
                    else if (tableTag == "Table2")
                    {
                        count *= mediumRewardMultiplier;
                    }
                    else if (tableTag == "Table3")
                    {
                        count *= highRewardMultiplier;
                    }
                    SetCountText();
                }
            }

        }
    }
}