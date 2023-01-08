using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    public static FarmManager Instance { get; private set; }

    [SerializeField] private Camera mainCamera;

    public int Level { get; private set; } = 1;
    public float Width { get; private set; } = 10f;
    public float Height { get; private set; } = 6f;
    
    private bool waitingForNextLevel;

    private void Awake() => Instance = this;
    private void Start() => Init();

    private void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Return))
            NextLevel();
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftControl) && Input.GetKey(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.Backspace))
            Init();
    }

    public void Init()
    {
        Level = 1;
        Width = 8f;
        Height = 4f;
        mainCamera.orthographicSize = 3f;
        FencesManager.Instance.SetSize(4);
        PlantsManager.Instance.Clear();
        PlantsManager.Instance.SpawnPlants(10);
        BirdsManager.Instance.Clear();
        BirdsManager.Instance.SetValues(ProgressManager.Instance.Shown ? 0.2f : -0.2f, 0.03f, 0.1f);
        ProgressManager.Instance.ResetProgress(60);
    }

    public void NextLevel()
    {
        Level++;
        Width += 2f;
        Height += 1f;
        mainCamera.orthographicSize += 0.6f;
        FencesManager.Instance.SetSize(Level + 3);
        PlantsManager.Instance.SpawnPlants(5); // + Level * 2);
        PlantsManager.Instance.Grow();
        BirdsManager.Instance.Clear();
        BirdsManager.Instance.SetValues(0.2f + 0.05f * Level, 0.03f + 0.01f * Level, 0.1f + 0.02f * Level);
        ProgressManager.Instance.ResetProgress(50 + 10 * Level);
        waitingForNextLevel = false;
    }

    public IEnumerator NextLevelAfterDelay(float delay)
    {
        if (waitingForNextLevel)
            yield break;
        waitingForNextLevel = true;
        yield return new WaitForSeconds(delay);
        NextLevel();
    }
}