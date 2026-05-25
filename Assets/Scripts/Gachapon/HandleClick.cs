using UnityEngine;

public class HandleClick : MonoBehaviour
{
    void OnMouseDown()
    {
        GachaponManager manager = FindObjectOfType<GachaponManager>();

        if (manager != null)
        {
            Debug.Log("Clic sur la manivelle détecté !");
            manager.OnClickSpin();
        }
        else
        {
            Debug.LogError("GachaponManager introuvable dans la scène !");
        }
    }
}