using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CornItemController : MonoBehaviour
{
    private float timer;
    private float speed;
    private float acceleration = 2;

    private void Update()
    {
        if (timer < 0.5f)
        {
            transform.position += Vector3.up * Time.deltaTime / 5;
        }
        else if ((ProgressManager.Instance.GetHarvestTarget() - transform.position).magnitude > speed * Time.deltaTime)
        {
            transform.position += (ProgressManager.Instance.GetHarvestTarget() - transform.position).normalized * speed * Time.deltaTime;
            speed += acceleration * Time.deltaTime;
        }
        else
        {
            PlantsManager.Instance.WaitingForHarvest--;
            ProgressManager.Instance.Score += 10;
            Destroy(gameObject);
        }

        timer += Time.deltaTime;
    }
}