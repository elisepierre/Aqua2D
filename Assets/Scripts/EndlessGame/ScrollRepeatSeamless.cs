using UnityEngine;

// À mettre sur l'objet qui contient votre fond d'eau
public class ScrollRepeatSeamless : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private Material material;

    void Start()
    {
        material = GetComponent<Renderer>().material;
        // Assurez-vous que l'image est bien en Repeat dans Unity (Asset Settings)
    }

    void Update()
    {
        // On change l'offset vertical
        float offsetY = Time.time * scrollSpeed;

        // Applique l'offset sur la texture principale (_MainTex)
        // La texture va boucler d'elle-même grâce au raccordement parfait de l'image 37
        material.SetTextureOffset("_MainTex", new Vector2(0, offsetY));
    }
}