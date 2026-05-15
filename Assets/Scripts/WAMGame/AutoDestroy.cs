using System.Collections;
using UnityEngine;

public class AutoDestroy : MonoBehaviour
{
    [Header("Réglages de l'impact")]
    public float rotationSpeed = 300f;
    public float targetRotation = -45f; 
    public float lifetime = 0.5f;

    void Start()
    {
        transform.rotation = Quaternion.Euler(0, 0, 30f);

        StartCoroutine(SwingHammer());

        Destroy(gameObject, lifetime);
    }

    IEnumerator SwingHammer()
    {
        float elapsed = 0f;
        Quaternion startRot = transform.rotation;
        Quaternion endRot = Quaternion.Euler(0, 0, targetRotation);

        while (elapsed < 0.15f)
        {
            transform.rotation = Quaternion.Slerp(startRot, endRot, elapsed / 0.15f);
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.rotation = endRot;
    }
}