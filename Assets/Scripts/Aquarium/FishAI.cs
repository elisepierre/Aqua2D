using UnityEngine;

public class FishAI : MonoBehaviour
{
    public float speed = 1.5f;
    private Vector2 targetPosition;
    private float minX = -8f, maxX = 8f, minY = -4f, maxY = 4f; // À ajuster selon ta caméra

    void Start()
    {
        SetNewTarget();
    }

    void Update()
    {
        // Déplacement vers la cible
        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);

        // Rotation (regarder vers la gauche ou la droite)
        // Ce code force le scale à 1 ou -1 à chaque frame !
        // Modifie les 1 par la taille souhaitée (ex: 3)
        if (targetPosition.x > transform.position.x)
            transform.localScale = new Vector3(-0.6f, 0.6f, 1f); // Regarde à droite en grand
        else
            transform.localScale = new Vector3(0.6f, 0.6f, 1f);  // Regarde à gauche en grand

        // Si on est arrivé, on choisit une nouvelle cible
        if (Vector2.Distance(transform.position, targetPosition) < 0.2f)
        {
            SetNewTarget();
        }
    }

    void SetNewTarget()
    {
        // On récupère les limites de la caméra en unités Unity
        Camera cam = Camera.main;
        float height = 2f * cam.orthographicSize;
        float width = height * cam.aspect;

        // On définit une marge (ex: 0.8f) pour que le poisson ne touche pas les bords
        float padding = 1.0f;

        float xLimit = (width / 1.5f) - padding;
        float yLimit = (height / 2f) - padding;

        // Nouvelle position aléatoire limitée à la vue de la caméra
        targetPosition = new Vector2(
            Random.Range(-xLimit, xLimit),
            Random.Range(-yLimit, yLimit)
        );
    }
}