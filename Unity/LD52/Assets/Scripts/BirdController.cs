using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdController : MonoBehaviour
{
    public enum State
    {
        Flying,
        Eating,
        Dying
    }

    [SerializeField] private PlantType type;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] flyingSprites;
    [SerializeField] private Sprite[] eatingSprites;
    [SerializeField] private Sprite[] dyingSprites;
    [SerializeField] private SpriteRenderer shadowSpriteRenderer;
    [SerializeField] private Sprite[] flyingShadowSprites;
    [SerializeField] private Sprite[] eatingShadowSprites;
    [SerializeField] private Sprite[] dyingShadowSprites;
    [SerializeField] private Color[] colors;
    [SerializeField] private Collider2D collider2d;
    [SerializeField] private float health;
    [SerializeField] private float speed;
    [SerializeField] private float eatingDuration;
    [SerializeField] private float dyingDuration;
    [SerializeField] private float scareDistance;

    public PlantType Type => type;
    public State state { get; private set; }
    public PlantController target { get; set; }
    public Vector3 heading { get; set; }

    private float stateTimer;
    private float lastDamage;
    private bool scared;

    public void Init(Vector3 position)
    {
        transform.position = position;
        speed *= Random.Range(0.875f, 1.125f);
        SelectTarget();
    }

    public void SelectTarget()
    {
        if (scared)
            return;
        if (target != null)
            target.RemoveTargetOf(this);
        target = PlantsManager.Instance.GetClosestPlant(PlantType.Corn, transform.position);
        SetState(State.Flying);
        if (target != null)
        {
            heading = (target.transform.position - transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, heading) * Quaternion.Euler(0, 0, 90);
            target.AddTargetOf(this);
        }
    }

    private void Update()
    {
        float distanceFromPlayer = (transform.position - PlayerManager.Instance.transform.position).magnitude;
        if (distanceFromPlayer < scareDistance && state != State.Dying)
        {
            scared = true;
            if (target != null)
                target.RemoveTargetOf(this);
            target = null;
            heading = (transform.position - PlayerManager.Instance.transform.position).normalized;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, heading) * Quaternion.Euler(0, 0, 90);
            SetState(State.Flying);
        }

        if (scared && distanceFromPlayer > scareDistance * 3 && state != State.Dying)
        {
            scared = false;
            SelectTarget();
        }

        if ((target == null || target.Eaten) && state != State.Dying)
            SelectTarget();

        if (state == State.Flying)
            if (target == null || (target.transform.position - transform.position).magnitude > speed * Time.deltaTime)
                transform.position += heading * speed * Time.deltaTime * (scared ? 1.5f : 1);
            else
            {
                transform.position = target.transform.position;
                SetState(State.Eating);
            }

        if (state == State.Eating && stateTimer >= eatingDuration)
        {
            target.Eat();
            SelectTarget();
        }

        if (state == State.Dying && stateTimer >= dyingDuration)
        {
            if (target != null)
                target.RemoveTargetOf(this);
            Destroy(gameObject);
            return;
        }

        switch (state)
        {
            case State.Flying:
                spriteRenderer.sprite = flyingSprites[(int)((stateTimer * 4) % flyingSprites.Length)];
                shadowSpriteRenderer.sprite = flyingShadowSprites[(int)((stateTimer * 4) % flyingShadowSprites.Length)];
                break;
            case State.Eating:
                spriteRenderer.sprite = eatingSprites[(int)((stateTimer) % eatingSprites.Length)];
                shadowSpriteRenderer.sprite = eatingShadowSprites[(int)((stateTimer) % eatingShadowSprites.Length)];
                break;
            case State.Dying:
                spriteRenderer.sprite = dyingSprites[(int)((stateTimer * 4) % dyingSprites.Length)];
                shadowSpriteRenderer.sprite = dyingShadowSprites[(int)((stateTimer * 4) % dyingShadowSprites.Length)];
                break;
        }

        stateTimer += Time.deltaTime;

        lastDamage += Time.deltaTime;
        spriteRenderer.color = colors[state != State.Dying && lastDamage < 0.1f ? 1 : 0];
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        if (health <= 0)
            SetState(State.Dying);
        lastDamage = 0;
    }

    public void SetState(State state)
    {
        if (state == this.state)
            return;
        if (state == State.Dying)
            collider2d.enabled = false;
        this.state = state;
        stateTimer = 0;
    }
}