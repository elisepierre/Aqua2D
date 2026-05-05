using UnityEngine;
using System.Collections; // Nécessaire pour IEnumerator
using System.Collections.Generic; // Pour les listes d'objets

public class GachaSystem : MonoBehaviour
{
    public Animator shellAnimator;
    public GameObject[] prizePool;
    public Transform spawnPoint;

    // Cette fonction se déclenche si tu as un Collider2D sur ton coquillage
    void OnMouseDown()
    {
        StartCoroutine(ExecuteGacha());
    }

    IEnumerator ExecuteGacha()
    {
        // 1. On déclenche le Trigger de l'Animator
        if (shellAnimator != null)
        {
            shellAnimator.SetTrigger("Turn");
        }

        // 2. On attend 0.5 secondes (ajuste selon ton animation)
        yield return new WaitForSeconds(0.5f);

        // 3. On fait apparaître la boule
        DropPrize();
    }

    void DropPrize()
    {
        if (prizePool.Length > 0 && spawnPoint != null)
        {
            int index = Random.Range(0, prizePool.Length);
            Instantiate(prizePool[index], spawnPoint.position, Quaternion.identity);
        }
    }
}