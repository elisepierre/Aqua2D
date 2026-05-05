using UnityEngine;
using System.Collections;

public class FishLaneMovement : MonoBehaviour
{
    [Header("Réglages des Voies")]
    public float[] lanePositions = new float[] { -2.0f, 0f, 2.0f }; // Positions X : Gauche, Centre, Droite
    private int currentLane = 1; // Commence au milieu (index 1)
    public float moveSpeed = 10f;

    [Header("Sprites de Rotation")]
    public Sprite spriteBack;      // Ton sprite de dos (image_39-1)
    public Sprite spriteTurnLeft;  // Ton sprite tourné vers la gauche (image_39-2)
    public Sprite spriteTurnRight; // Ton sprite tourné vers la droite (image_39-3)

    private SpriteRenderer sr;
    private bool isMoving = false;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;

    void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        // Place le poisson sur la voie de départ
        transform.position = new Vector3(lanePositions[currentLane], transform.position.y, transform.position.z);
    }

    void Update()
    {
        HandleSwipe();
    }

    void HandleSwipe()
    {
        if (isMoving) return;

        // Détection du Swipe (Souris ou Tactile)
        if (Input.GetMouseButtonDown(0)) startTouchPosition = Input.mousePosition;

        if (Input.GetMouseButtonUp(0))
        {
            endTouchPosition = Input.mousePosition;
            float swipeDist = endTouchPosition.x - startTouchPosition.x;

            if (Mathf.Abs(swipeDist) > 50) // Seuil pour éviter les petits clics
            {
                if (swipeDist < 0 && currentLane > 0) // Vers la gauche
                    StartCoroutine(MoveToLane(currentLane - 1, spriteTurnLeft));
                else if (swipeDist > 0 && currentLane < 2) // Vers la droite
                    StartCoroutine(MoveToLane(currentLane + 1, spriteTurnRight));
            }
        }
    }

    IEnumerator MoveToLane(int targetLane, Sprite turnSprite)
    {
        isMoving = true;
        currentLane = targetLane;
        float targetX = lanePositions[currentLane];

        // 1. On change le sprite pour la rotation
        sr.sprite = turnSprite;

        // 2. On bouge le poisson de façon fluide
        while (Mathf.Abs(transform.position.x - targetX) > 0.05f)
        {
            float newX = Mathf.MoveTowards(transform.position.x, targetX, moveSpeed * Time.deltaTime);
            transform.position = new Vector3(newX, transform.position.y, transform.position.z);
            yield return null;
        }

        // 3. Arrivé sur la voie, on remet le sprite de dos
        transform.position = new Vector3(targetX, transform.position.y, transform.position.z);
        sr.sprite = spriteBack;
        isMoving = false;
    }
}