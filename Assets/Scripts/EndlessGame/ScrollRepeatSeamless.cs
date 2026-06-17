using UnityEngine;

public class ScrollRepeatSeamless : MonoBehaviour

{

    public float scrollSpeed = 0.3f;

    private Material material;



    void Start()

    {

        material = GetComponent<Renderer>().material;

    }



    void Update()

    {

        float offsetY = Time.time * scrollSpeed;

        material.SetTextureOffset("_MainTex", new Vector2(0, offsetY));

    }

}

