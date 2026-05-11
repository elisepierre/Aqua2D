using UnityEngine;
using System.Collections;

public class FishLaneMovement : MonoBehaviour
{
    [Header("Réglages des Voies")]
    public float[] lanePositions = new float[] { -2.0f, 0f, 2.0f };
    private int currentLane = 1;
    public float moveSpeed = 10f;

    [Header("Sprites de Rotation")]
    public Sprite spriteBack;
    public Sprite spriteTurnLeft;
    public Sprite spriteTurnRight;

    private SpriteRenderer sr;
    private bool isMoving = false;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        transform.position = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
    }

    void Update()
    {
        if (Time.timeScale == 0) return;
        HandleSwipe();
    }

    void HandleSwipe()
    {
        if (isMoving) return;

        if (Input.GetMouseButtonDown(0)) startTouchPosition = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            float swipeDist = endTouchPosition.x - startTouchPosition.x;

            if (Mathf.Abs(swipeDist) > 50)
            {
                if (swipeDist < 0 && currentLane > 0)
                    StartCoroutine(MoveToLane(currentLane - 1, spriteTurnLeft));
                else if (swipeDist > 0 && currentLane < 2)
                    StartCoroutine(MoveToLane(currentLane + 1, spriteTurnRight));
            }
        }
    }

    IEnumerator MoveToLane(int targetLane, Sprite turnSprite)
    {
        isMoving = true;
        currentLane = targetLane;
        float targetX = lanePositions[currentLane];

        sr.sprite = turnSprite;

        while (Mathf.Abs(transform.position.x - targetX) > 0.05f)
        {
            float newX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            yield return null;
        }

        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        sr.sprite = spriteBack;
        isMoving = false;
    }
}