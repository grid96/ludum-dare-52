using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }

    [SerializeField] private Camera mainCamera;
    [SerializeField] private CornController cornPrefab;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private SpriteRenderer shadowSpriteRenderer;
    [SerializeField] private Sprite[] shadowSprites;

    public float Speed { get; private set; } = 1f;
    public float TimeBetweenShots { get; private set; } = 0.2f;
    public float Cooldown { get; private set; }
    public bool AutoFire { get; private set; }

    private void Awake() => Instance = this;

    private void Update()
    {
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        Vector3 heading = worldPosition - transform.position;
        heading.z = 0;
        if (heading.magnitude > Speed / 10f)
        {
            heading = heading.normalized;
            transform.position += heading * Speed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(Vector3.forward, heading);
        }

        transform.position = new Vector3(Math.Clamp(transform.position.x, -FarmManager.Instance.Width / 2 + .5f, FarmManager.Instance.Width / 2 - .5f),
            Math.Clamp(transform.position.y, -FarmManager.Instance.Height / 2 + .5f, FarmManager.Instance.Height / 2 - .5f), 0);

        Cooldown -= Time.deltaTime;
        if (Input.GetKeyDown(KeyCode.Return))
            AutoFire = !AutoFire;
        if (AutoFire || Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
            Shoot();

        spriteRenderer.sprite = sprites[Cooldown >= TimeBetweenShots * 0.625f ? 1 : 0];
        shadowSpriteRenderer.sprite = shadowSprites[Cooldown >= TimeBetweenShots * 0.625f ? 1 : 0];
    }

    public void Shoot()
    {
        if (Cooldown > 0)
            return;
        Cooldown = TimeBetweenShots;
        CornController corn = Instantiate(cornPrefab, transform.position + transform.up * .5f, transform.rotation);
        corn.Init(4, 2);
    }
}