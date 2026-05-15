using UnityEngine;
using UnityEngine.UI; 
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class StoryManager : MonoBehaviour
{
    [Header("UI Elements")]
    public Image mermaidImage; 
    public TextMeshProUGUI dialogText;

    [Header("Sprites for Mermaid")]
    public Sprite mermaidHappy;   
    public Sprite mermaidPanic;  
    public Sprite mermaidSad;    

    [Header("Objects (Drag your 3 items here)")]
    public GameObject[] fish;
    public GameObject[] trash;
    public GameObject fisherman;

    private int step = 0;

    void Start()
    {
        mermaidImage.sprite = mermaidHappy;

        foreach (GameObject t in trash) t.SetActive(false);
        fisherman.SetActive(false);

        dialogText.text = "Our home is beautiful and peaceful...";
    }

    public void NextStep()
    {
        step++;

        switch (step)
        {
            case 1:
                mermaidImage.sprite = mermaidPanic;

                foreach (GameObject t in trash) t.SetActive(true);
                dialogText.text = "But suddenly... trash is invading our water!";
                break;

            case 2:
                fisherman.SetActive(true);
                dialogText.text = "Fishermen are taking my friends away...";
                break;

            case 3:
                mermaidImage.sprite = mermaidSad;
                StartCoroutine(RemoveFishOneByOne());
                break;

            case 4:
                dialogText.text = "Please, traveler, you must help us clean and save the ocean!";
                break;

            case 5:
                SceneManager.LoadScene("ChoiceScene");
                break;
        }
    }

    IEnumerator RemoveFishOneByOne()
    {
        foreach (GameObject f in fish)
        {
            f.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
        dialogText.text = "I am all alone now...";
    }
}