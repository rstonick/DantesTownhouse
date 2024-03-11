using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class TimeReset : MonoBehaviour
{
    public float timeLimit = 180f;
    private float currentTime;
    public TextMeshProUGUI timeText;

    void Start()
    {
        currentTime = timeLimit;
        SetTimeText();
    }

    void Update()
    {
        if (currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            SetTimeText();
        }
        else
        {
            ResetScene();
        }
    }

    void ResetScene()
    {
        Scene currentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
        UnityEngine.SceneManagement.SceneManager.LoadScene(currentScene.name);
    }

    public void SetTimeText()
    {
        timeText.text = "Time: " + currentTime.ToString("0") + "s";
    }
}
