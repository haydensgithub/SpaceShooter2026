using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class DeathTimer : MonoBehaviour
{
    public static DeathTimer Instance;
    public float startingTime = 20f;
    public float currentTime;
    public TMP_Text timerText;
    public bool gameOver = false;
    public UI ui;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        currentTime = startingTime;
        UpdateTimerUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameOver) return;

        if (ui != null && !ui.IsReady) return;

        currentTime -= Time.deltaTime;

        if (currentTime <= 0)
        {
            currentTime = 0;
            UpdateTimerUI();
            EndGame();
            return;
        }

        UpdateTimerUI();
        
    }

    public void AddTime(float amount)
    {
        if (gameOver) return;

        currentTime += amount;
        UpdateTimerUI();
    }

    void UpdateTimerUI()
    {
        if (timerText != null)
        {
            timerText.text = "Time: " + currentTime.ToString("F1");
        }
    }

    void EndGame()
    {
        gameOver = true;
        ui.ShowGameOver();
    }
}
