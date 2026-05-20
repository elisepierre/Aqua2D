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

    public void UpdateDialog()
    {
        int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);
        dialogText.text = LanguageManager.instance.GetStoryDialog(step, lang);
    }

    void Start()
    {
        mermaidImage.sprite = mermaidHappy;
        foreach (GameObject t in trash) t.SetActive(false);
        fisherman.SetActive(false);

        UpdateDialog();
    }
    public void NextStep()
    {
        step++;

        switch (step)
        {
            case 1:
                mermaidImage.sprite = mermaidPanic;

                foreach (GameObject t in trash) t.SetActive(true);
                UpdateDialog(); 
                break;

            case 2:
                fisherman.SetActive(true);
                UpdateDialog();
                break;

            case 3:
                mermaidImage.sprite = mermaidSad;
                StartCoroutine(RemoveFishOneByOne());
                break;

            case 4:
                UpdateDialog();
                break;

            case 5:
                SceneManager.LoadScene("LinkScene");
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
        UpdateDialog();
    }
}