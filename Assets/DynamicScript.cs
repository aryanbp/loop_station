using UnityEngine;

public class DynamicSprite : MonoBehaviour
{
    public Sprite[] frames;
    public float frameRate = 0.2f;

    private SpriteRenderer spriteRenderer;
    private int currentFrame = 0;
    private float timer = 0f;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = frames[0];
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= frameRate)
        {
            currentFrame = (currentFrame + 1) % frames.Length;
            spriteRenderer.sprite = frames[currentFrame];
            timer = 0f;
        }
    }
}
