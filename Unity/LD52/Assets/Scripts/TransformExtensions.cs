using UnityEngine;

public static class TransformExtensions
{
    public static Transform DestroyAllChildren(this Transform transform)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(false);
            Object.Destroy(child.gameObject);
        }
        return transform;
    }

    public static Transform DestroyAllChildren<T>(this Transform transform) where T : Component
    {
        foreach (T child in transform.GetComponentsInChildren<T>())
        {
            if (child.gameObject == transform.gameObject)
                continue;
            child.gameObject.SetActive(false);
            Object.Destroy(child.gameObject);
        }
        return transform;
    }
}
