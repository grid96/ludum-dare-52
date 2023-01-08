using UnityEngine;

public class Pulsate : MonoBehaviour
{
    [SerializeField] private float amplitude = 1.25f;
    [SerializeField] private float frequency = 0.5f;

    private Vector3 scale;

    private void Update()
    {
        transform.localScale = scale * (1 + (amplitude - 1) / 2 + Mathf.Sin(2 * Mathf.PI * Time.time * frequency) * ((amplitude - 1) / 2));
    }

    private void OnEnable()
    {
        scale = transform.localScale;
    }

    private void OnDisable()
    {
        transform.localScale = scale;
    }
}