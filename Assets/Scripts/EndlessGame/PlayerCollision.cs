using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class PlayerCollision : MonoBehaviour
{
    public int score = 0;
    private System.Collections.Generic.HashSet<int> collectedShells = new System.Collections.Generic.HashSet<int>();
    public TextMeshProUGUI scoreText;
    public GameObject loosePanel;
    public GameObject collectAnimPrefab;
    public RectTransform scoreIcon;
    private int lastFrameChecked = -1;
    private float lastCollectTime = 0f;
    private float collectDelay = 0.1f;

    void Awake()
    {
        score = 0;
    }
    void Start()
    {
        Time.timeScale = 1f;
        if (scoreText != null)
        {
            scoreText.text = "0";
        }
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Obstacle"))
        {
            Debug.Log("COLLISION OBSTACLE !");

            if (DataManager.Instance != null)
            {
                DataManager.Instance.AddShellsToTotal(score);
            }

            loosePanel.SetActive(true);
            Time.timeScale = 0f; // Arrête le jeu
            return; // On sort de la fonction pour ne rien faire d'autre
        }
        else if (other.CompareTag("Shell"))
        {
            if (Time.time < lastCollectTime + collectDelay) return;

            lastCollectTime = Time.time;
            int shellID = other.gameObject.GetInstanceID();

            // SÉCURITÉ ABSOLUE : Si on a déjà traité cet ID unique, on ignore
            if (collectedShells.Contains(shellID)) return;

            // On ajoute l'ID à la liste des objets déjà ramassés
            collectedShells.Add(shellID);

            // --- Ton code de ramassage ---
            other.tag = "Untagged";
            Collider2D shellCollider = other.GetComponent<Collider2D>();
            if (shellCollider != null) shellCollider.enabled = false;

            GameObject anim = Instantiate(collectAnimPrefab, other.transform.position, Quaternion.identity);
            if (anim != null)
            {
                anim.GetComponent<EndlessCollectAnimation>().StartAnimation(scoreIcon);
            }

            score++;
            if (scoreText != null) scoreText.text = score.ToString();

            Destroy(other.gameObject);
        }
    }
    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("LinkScene");
    }
}