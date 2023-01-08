using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager Instance { get; private set; }

    [SerializeField] private Slider slider;
    [SerializeField] private Button harvestButton;
    [SerializeField] private TMP_Text stateText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text healthText;
    [SerializeField] private TMP_Text gameOverScoreText;
    [SerializeField] private TMP_Text gameOverHighscoreText;
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private Animator progressAnimator;
    [SerializeField] private Animator scoreAnimator;
    [SerializeField] private Animator healthAnimator;
    [SerializeField] private Animator instructionAnimator;
    [SerializeField] private Animator harvestAnimator;
    [SerializeField] private Animator gameOverAnimator;
    [SerializeField] private Animator levelAnimator;
    [SerializeField] private CanvasGroup instructionCanvasGroup;
    [SerializeField] private CanvasGroup harvestCanvasGroup;
    [SerializeField] private CanvasGroup gameOverCanvasGroup;

    public int Score;
    public bool Completed { get; private set; }
    public bool IsGameOver { get; private set; }
    public bool Shown { get; private set; }

    private float duration = float.MaxValue;

    private void Awake() => Instance = this;

    private void Update()
    {
        if (!Shown && Time.timeSinceLevelLoad >= 16)
        {
            progressAnimator.enabled = true;
            scoreAnimator.enabled = true;
            healthAnimator.enabled = true;
            instructionAnimator.enabled = true;
            Shown = true;
        }

        if (!Completed && !IsGameOver)
        {
            slider.value += Time.deltaTime / duration;
            if (slider.value >= 1)
            {
                Complete();
                Completed = true;
            }
            else if (PlantsManager.Instance.GetEaten() > 0.5f)
            {
                GameOver();
                IsGameOver = true;
            }
        }

        scoreText.text = Score + "$";
        healthText.text = 100 - (int)(PlantsManager.Instance.GetEaten() * 100) + "%";
        healthText.color = Color.Lerp(Color.HSVToRGB(1 / 3f, 0.5f, 1f).Alpha(0.75f), Color.HSVToRGB(0f, 0.5f, 1f).Alpha(0.75f), PlantsManager.Instance.GetEaten() * 2);

        if (Completed && !harvestButton.interactable && PlantsManager.Instance.WaitingForHarvest == 0)
            StartCoroutine(FarmManager.Instance.NextLevelAfterDelay(1));
    }

    public void ResetProgress(float duration)
    {
        this.duration = duration;
        Completed = false;
        IsGameOver = false;
        slider.value = 0;
        harvestAnimator.enabled = false;
        gameOverAnimator.enabled = false;
        instructionCanvasGroup.alpha = 0;
        harvestCanvasGroup.alpha = 0;
        gameOverCanvasGroup.alpha = 0;
        instructionCanvasGroup.blocksRaycasts = false;
        harvestCanvasGroup.blocksRaycasts = false;
        gameOverCanvasGroup.blocksRaycasts = false;
        stateText.text = $"Level {FarmManager.Instance.Level} - Progress";
        if (Shown)
        {
            instructionAnimator.enabled = true;
            levelText.text = $"Level {FarmManager.Instance.Level}";
            levelAnimator.enabled = true;
            levelAnimator.Play("Level", 0, 0);
        }
    }

    public void Complete()
    {
        harvestButton.interactable = true;
        instructionAnimator.enabled = false;
        instructionCanvasGroup.alpha = 0;
        instructionCanvasGroup.blocksRaycasts = false;
        harvestAnimator.enabled = true;
        stateText.text = $"Level {FarmManager.Instance.Level} - Complete!";
        levelAnimator.enabled = false;
    }

    public void Harvest()
    {
        harvestButton.interactable = false;
        PlantsManager.Instance.Harvest();
    }

    public void GameOver()
    {
        gameOverAnimator.enabled = true;
        gameOverScoreText.text = $"Score: {Score}$";
        gameOverHighscoreText.text = Score > PlayerPrefs.GetInt("Highscore", 0) ? "New Highscore!" : $"Highscore: {PlayerPrefs.GetInt("Highscore", 0)}$";
        PlayerPrefs.SetInt("Highscore", Score);
    }

    public void Restart()
    {
        Score = 0;
        FarmManager.Instance.Init();
    }

    public Vector3 GetHarvestTarget()
    {
        return harvestButton.transform.position;
    }
}