using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class DataManager : MonoBehaviour
{
    public static DataManager Instance;
    public TextMeshProUGUI totalShellText;
    private int totalShells;

    void Awake()
    {
        if (Instance == null) { Instance = this; }
        else { Destroy(gameObject); }
    }

    void Start()
    {
        Time.timeScale = 1f;

        if (SceneManager.GetActiveScene().name == "LinkScene")
        {
            RefreshData();
        }
    }

    public void RefreshData()
    {
        Debug.Log("Score chargé : " + totalShells);
        totalShells = PlayerPrefs.GetInt("TotalShells", 0);
        UpdateShellDisplay();
    }

    public void UpdateShellDisplay()
    {
        if (totalShellText == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("ScoreText");
            if (go != null) totalShellText = go.GetComponent<TextMeshProUGUI>();
        }

        if (totalShellText != null)
        {
            totalShellText.text = totalShells.ToString();
        }
    }

    public void AddShellsToTotal(int shellsFromMiniGame)
    {
        int currentTotal = PlayerPrefs.GetInt("TotalShells", 0);
        currentTotal += shellsFromMiniGame;
        PlayerPrefs.SetInt("TotalShells", currentTotal);
        PlayerPrefs.Save();

        totalShells = currentTotal;

        UpdateShellDisplay();
    }

    public bool SpendShells(int amount)
    {
        totalShells = PlayerPrefs.GetInt("TotalShells", 0);

        if (totalShells >= amount)
        {
            totalShells -= amount;
            PlayerPrefs.SetInt("TotalShells", totalShells);
            PlayerPrefs.Save();
            UpdateShellDisplay();
            return true;
        }

        return false;
    }
}