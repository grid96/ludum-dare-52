using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlantsManager : MonoBehaviour
{
    public static PlantsManager Instance { get; private set; }

    private static readonly float newGroupChanceIncrease = 0.05f;
    private static readonly float newGroupChanceDecrease = 0.5f;
    private static readonly float minGroupDistance = 5;
    private static readonly float maxNeighborDistance = 0.5f;

    [SerializeField] private PlantController[] plantPrefabs;

    private List<PlantController> plants = new List<PlantController>();
    private float newGroupChance;

    private void Awake() => Instance = this;

    public void SpawnPlants(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
            PlantType type = PlantType.Corn;
            bool newGroup = Random.value < newGroupChance;
            if (plants.Count == 0)
                newGroup = true;

            Vector3 position = Vector3.zero;
            bool valid = false;
            int attempts = 0;
            do
            {
                if (attempts == 100)
                    newGroup = false;
                if (attempts == 1000)
                    break;
                position = new Vector3(Random.Range(-(FarmManager.Instance.Width - plantPrefabs[(int)type].Size) / 2, (FarmManager.Instance.Width - plantPrefabs[(int)type].Size) / 2),
                    Random.Range(-(FarmManager.Instance.Height - plantPrefabs[(int)type].Size) / 2, (FarmManager.Instance.Height - plantPrefabs[(int)type].Size) / 2), 0);
                bool groupDistanceValid = true;
                bool neighborDistanceValid = false;
                bool plantDistanceValid = true;
                foreach (PlantController plant in plants)
                {
                    float minPlantDistance = (plantPrefabs[(int)type].Size + plant.Size) / 2;
                    if (newGroup && Vector3.Distance(plant.Position, position) < minGroupDistance + minPlantDistance)
                        groupDistanceValid = false;
                    if (!newGroup && Vector3.Distance(plant.Position, position) < maxNeighborDistance + minPlantDistance)
                        neighborDistanceValid = true;
                    if (Vector3.Distance(plant.Position, position) < minPlantDistance)
                        plantDistanceValid = false;
                    if (!groupDistanceValid || !plantDistanceValid)
                        break;
                }

                valid = ((newGroup && groupDistanceValid) || (!newGroup && neighborDistanceValid)) && plantDistanceValid;
                attempts++;
            } while (!valid);

            if (!valid)
            {
                Debug.Log($"Could not find a valid position for {type} plant.");
                return;
            }

            if (newGroup)
                newGroupChance -= newGroupChanceDecrease;
            newGroupChance += newGroupChanceIncrease;
            SpawnPlant(type, position);
        }
    }

    public void SpawnPlant(PlantType type, Vector3 position)
    {
        PlantController plant = Instantiate(plantPrefabs[(int)type], transform);
        plant.Init(position);
        plants.Add(plant.GetComponent<PlantController>());
    }

    public PlantController GetClosestPlant(PlantType type, Vector3 position)
    {
        PlantController closestPlant = null;
        float smallestDistance = float.MaxValue;
        int leastTargetOf = int.MaxValue;
        foreach (PlantController plant in plants)
            if (plant.Type == type && !plant.Eaten)
            {
                float distance = Vector3.Distance(plant.Position, position);
                if ((plant.targetOfCount < leastTargetOf || (plant.targetOfCount == leastTargetOf && distance < smallestDistance)) && plant.targetOfCount < 3)
                {
                    closestPlant = plant;
                    smallestDistance = distance;
                    leastTargetOf = plant.targetOfCount;
                }
            }

        return closestPlant;
    }

    public void Grow()
    {
        foreach (PlantController plant in plants)
            plant.Grow();
    }

    public void Clear()
    {
        transform.DestroyAllChildren<PlantController>();
        plants.Clear();
    }
}