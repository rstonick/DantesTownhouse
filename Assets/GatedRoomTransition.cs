using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GatedRoomTransition : MonoBehaviour
{
    public string targetScene;
    public CoinCollector coinCollector;
    public bool floorComplete = false;

    void Update()
    {
        if (coinCollector != null)
        {
            if (coinCollector.count >= 4000)
            {
                floorComplete = true;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && floorComplete == true)
        {
            SceneManager.LoadScene(targetScene);
        }
    }
}
