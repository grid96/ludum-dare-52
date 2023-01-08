using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FencesManager : MonoBehaviour
{
    public static FencesManager Instance { get; private set; }

    private static readonly Vector3[] fenceOffsets = { Vector3.up / 2, Vector3.right, Vector3.down / 2, Vector3.left };

    [SerializeField] private SpriteRenderer[] fences;
    [SerializeField] private SpriteRenderer[] corners;

    private void Awake() => Instance = this;

    public void SetSize(int size)
    {
        for (int i = 0; i < fences.Length; i++)
        {
            fences[i].transform.position = fenceOffsets[i % 4] * size;
            fences[i].size = new Vector2(size * 3.2f * (i % 2 == 0 ? 1 : 0.5f), 0.8f);
        }

        for (int i = 0; i < corners.Length; i++)
        {
            corners[i].transform.position = (fenceOffsets[i % 4] + fenceOffsets[(i + 3) % 4]) * size;
        }
    }
}