using UnityEngine;

public class HorizontalBackgroundScroll : MonoBehaviour
{
    [SerializeField] private float scrollSpeed = 0.1f;
    private Material material;
    private Vector2 offset;

    void Start()
    {
        material = GetComponent<Renderer>().material;
    }

    void Update()
    {
        offset.x += scrollSpeed * Time.deltaTime;

        material.mainTextureOffset = offset;
    }
}
