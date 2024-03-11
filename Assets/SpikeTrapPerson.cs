using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeTrapPerson : MonoBehaviour
{
    public bool hitTrap = false;
    public void HitTrap()
    {
        hitTrap = true;
    }
}
