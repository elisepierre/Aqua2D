using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float speed = 1.5f;
    private Vector2 targetPosition;
    private float minX = -8f, maxX = 8f, minY = -4f, maxY = 4f;

    void Start()
    {
        SetNewTarget();
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        
        if (targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(-0.6f, 0.6f, 1f);
        else
            transform.localScale = new Vector3(0.6f, 0.6f, 1f);
            
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            SetNewTarget();
        }
    }

    void SetNewTarget()
    {
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        float padding = 1.0f;

        float xLimit = (width / 1.5f) - padding;
        float yLimit = (height / 2f) - padding;

        targetPosition = new Vector2(
            Random.Range(-xLimit, xLimit),
            Random.Range(-yLimit, yLimit)
        );
    }
}
