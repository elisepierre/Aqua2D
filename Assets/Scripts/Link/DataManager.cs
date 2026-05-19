using UnityEngine;
using TMPro;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;

    [Header("UI Elements")]
    public TextMeshProUGUI totalShellText;

    private int totalShells;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene().name == "LinkScene")
        {
            RefreshData();
        }  
    }
    public void RefreshData()
    {
        totalShells = PlayerPrefs.GetInt("TotalShells", 0);
        UpdateShellDisplay();
    }

    public void UpdateShellDisplay()
    {
        if (totalShellText != null)
        {
            totalShellText.text = totalShells.ToString();
        }
    }

    public void AddShellsToTotal(int shellsFromMiniGame)
    {
        totalShells = PlayerPrefs.GetInt("TotalShells", 0);

        totalShells += shellsFromMiniGame;

        PlayerPrefs.SetInt("TotalShells", totalShells);
        PlayerPrefs.Save();

        UpdateShellDisplay();
    }
}