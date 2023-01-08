using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornController : MonoBehaviour
{
    public float speed;
    public float lifetime;

    public void Init(float speed, float lifetime)
    {
        this.speed = speed;
        this.lifetime = lifetime;
    }

    private void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0)
            Destroy(gameObject);
        transform.position += transform.up * Time.deltaTime * speed;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        BirdController bird = other.GetComponent<BirdController>();
        if (bird == null)
            return;
        bird.TakeDamage(1);
        Destroy(gameObject);
    }
}