using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vegetable : MonoBehaviour
{
    private const float GRAVITY = 2.0f;
    public bool IsActive { set; get; }
    public SpriteRenderer spriteRenderer;

    private float verticalVelocity;
    private float speed;
    private bool isSliced;
    
    public Sprite[] sprites;
    private int spriteIndex;
    private float lastSpriteUpdate;
    private float spriteUpdateDelta = 0.125f;
    private float rotationSpeed;

    public void LaunchVegetable(float verticalVelocity, float xSpeed, float xStart)
    {
        IsActive = true;
        speed = xSpeed;
        this.verticalVelocity = verticalVelocity;
        transform.position = new Vector3(xStart, 0, 0);
        rotationSpeed = Random.Range(-180, 180);
        isSliced = false;
        spriteIndex = 0;
        spriteRenderer.sprite = sprites[spriteIndex];
    }

    private void Update()
    {
        if (!IsActive)
            return;

        verticalVelocity -= GRAVITY * Time.deltaTime;
        transform.position += new Vector3(speed, verticalVelocity, 0) * Time.deltaTime;
        transform.Rotate(new Vector3(0, 0, rotationSpeed) * Time.deltaTime);

        if (isSliced)
        {
            if (spriteIndex != sprites.Length - 1 && Time.time - lastSpriteUpdate > spriteUpdateDelta)
            {
                lastSpriteUpdate = Time.time;
                spriteIndex++;
                spriteRenderer.sprite = sprites[spriteIndex];
            }
        }

        if (transform.position.y < -1)
        {
            IsActive = false;
            if (!isSliced)
                GameManager.Instance.LoseLifePoint();
        }
            
    }

    public void Slice()
    {
        if(isSliced)
            return;
        
        if (verticalVelocity < 0.5f)
            verticalVelocity = 0.5f;

        speed = speed * 0.5f;
        isSliced = true;
        
        SoundManager.Instance.PlaySound(0);

        GameManager.Instance.IncrementScore(1);
    }
}
