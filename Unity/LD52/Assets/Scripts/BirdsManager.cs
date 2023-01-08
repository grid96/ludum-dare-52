using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdsManager : MonoBehaviour
{
    public static BirdsManager Instance { get; private set; }

    [SerializeField] private BirdController[] birdPrefabs;

    private List<BirdController> birds = new List<BirdController>();
    private float newBirdChance;
    private float newBirdChanceIncrease = 0.02f;
    private float newBirdChanceDecrease = 0.1f;

    private void Awake() => Instance = this;

    public void SetValues(float newBirdChanceIncrease, float newBirdChanceDecrease)
    {
        newBirdChance = 0;
        this.newBirdChanceIncrease = newBirdChanceIncrease;
        this.newBirdChanceDecrease = newBirdChanceDecrease;
    }

    private void Update()
    {
        if (Random.value < newBirdChance)
        {
            newBirdChance -= newBirdChanceDecrease;
            Vector3 position = Quaternion.Euler(0, 0, 360 * Random.value) * Vector3.up * (6 + 1.2f * FarmManager.Instance.Level);
            SpawnBird(BirdType.Easy, position);
        }

        newBirdChance += newBirdChanceIncrease * Time.deltaTime;
    }

    public void SpawnBird(BirdType type, Vector3 position)
    {
        BirdController bird = Instantiate(birdPrefabs[(int)type], transform);
        bird.Init(position);
        birds.Add(bird);
    }

    public void Clear()
    {
        transform.DestroyAllChildren<BirdController>();
        birds.Clear();
    }
}