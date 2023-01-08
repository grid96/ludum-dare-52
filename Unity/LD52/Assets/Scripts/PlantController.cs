using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantController : MonoBehaviour
{
    [SerializeField] private PlantType type;
    [SerializeField] private float size;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer headRenderer;
    [SerializeField] private CornItemController cornItemPrefab;

    private List<BirdController> targetOf = new List<BirdController>();

    public PlantType Type => type;
    public float Size => size;
    public Vector3 Position => transform.position;
    public bool Eaten { get; private set; }
    public bool Harvested { get; private set; }
    public int targetOfCount => targetOf.Count;

    public void Init(Vector3 position)
    {
        transform.position = position;
        transform.rotation = Quaternion.Euler(0, 0, Random.Range(-45, 45));
    }

    public void Grow()
    {
        Eaten = false;
        Harvested = false;
        spriteRenderer.sprite = sprites[0];
        headRenderer.gameObject.SetActive(true);
        targetOf.Clear();
    }

    public void AddTargetOf(BirdController bird)
    {
        if (targetOf.Contains(bird))
            return;
        targetOf.Add(bird);
    }

    public void RemoveTargetOf(BirdController bird)
    {
        if (!targetOf.Contains(bird))
            return;
        targetOf.Remove(bird);
    }

    public void Eat()
    {
        Eaten = true;
        spriteRenderer.sprite = sprites[1];
        headRenderer.gameObject.SetActive(false);
    }

    public void Harvest()
    {
        Harvested = true;
        spriteRenderer.sprite = sprites[2];
        headRenderer.gameObject.SetActive(false);
        Instantiate(cornItemPrefab, transform.position, Quaternion.identity);
    }
 
    public IEnumerator HarvestAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Harvest();
    }
}