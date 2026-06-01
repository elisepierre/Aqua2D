using UnityEngine;
using TMPro;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class LanguageManager : MonoBehaviour
{
    public static LanguageManager instance;

    [Header("Font Assets")]
    public TMP_FontAsset englishFontAsset;
    public TMP_FontAsset chineseFontAsset;

    private void Awake()
    {
        // LE VIDEUR : Empêche les doublons et garde les réglages
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {
        RefreshSceneText();
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Time.timeScale = 1f;
        RefreshSceneText();
        ReconnectLanguageButtons(); // <-- AJOUTE CETTE LIGNE
    }

    private void ReconnectLanguageButtons()
    {
        // On récupère TOUS les boutons de la scène, même ceux cachés dans des panels
        UnityEngine.UI.Button[] allButtons = Resources.FindObjectsOfTypeAll<UnityEngine.UI.Button>();

        foreach (var btn in allButtons)
        {
            // On vérifie le nom de l'objet (assure-toi que c'est bien le nom dans la hiérarchie)
            if (btn.gameObject.name == "EnglishButton")
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => SetLanguage(0));
                Debug.Log("Lien rétabli pour Anglais");
            }
            else if (btn.gameObject.name == "ChineseButton")
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => SetLanguage(1));
                Debug.Log("Lien rétabli pour Chinois");
            }
            else if (btn.gameObject.name == "FrenchButton")
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(() => SetLanguage(2));
                Debug.Log("Lien rétabli pour Français");
            }
        }
    }

    public void SetLanguage(int index)
    {
        // Sauvegarde immédiate dans la mémoire du téléphone/PC
        PlayerPrefs.SetInt("SelectedLanguage", index);
        PlayerPrefs.Save();

        RefreshSceneText();

        if (AudioManager.Instance != null)
            AudioManager.Instance.PlaySFX(AudioManager.Instance.pauseClip);
    }

    public void RefreshSceneText()
    {
        int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);
        TextMeshProUGUI[] allTexts = Object.FindObjectsByType<TextMeshProUGUI>(FindObjectsSortMode.None);

        foreach (var txt in allTexts)
        {
            if (txt == null) continue;
            txt.font = (lang == 1 && txt.gameObject.name.StartsWith("TXT_")) ? chineseFontAsset : englishFontAsset;
            txt.UpdateFontAsset();
        }

        // --- SCENE : MAIN MENU ---
        TranslateByName("TXT_Play", lang == 0 ? "PLAY" : lang == 1 ? "開始遊戲" : "JOUER");
        TranslateByName("TXT_Continue", lang == 0 ? "CONTINUE" : lang == 1 ? "繼續" : "CONTINUER");
        TranslateByName("TXT_Settings", lang == 0 ? "LANGUAGE" : lang == 1 ? "語言" : "LANGUE");
        TranslateByName("TXT_French", lang == 0 ? "FRENCH" : lang == 1 ? "法文" : "FRANÇAIS");
        TranslateByName("TXT_English", lang == 0 ? "ENGLISH" : lang == 1 ? "英語" : "ANGLAIS");
        TranslateByName("TXT_Chinese", lang == 0 ? "CHINESE" : lang == 1 ? "繁體中文" : "CHINOIS TRAD.");

        // --- SCENE : LINK SCENE ---
        TranslateByName("TXT_ChoiceTitle", lang == 0 ? "CHOOSE A MINI-GAME" : lang == 1 ? "選擇小遊戲" : "CHOISIS UN JEU");
        TranslateByName("TXT_Game1", lang == 0 ? "CLEAN OCEAN" : lang == 1 ? "清潔海洋" : "LAVER L'OCÉAN");
        TranslateByName("TXT_Game2", lang == 0 ? "SWIM RUSH" : lang == 1 ? "極速游泳" : "COURSE DE NAGE");
        TranslateByName("TXT_Game3", lang == 0 ? "FISHER WACK" : lang == 1 ? "敲打漁夫" : "TAPE-PÊCHEURS");
        TranslateByName("TXT_Minigames", lang == 0 ? "MINI-GAMES" : lang == 1 ? "小遊戲" : "MINI-JEUX");
        TranslateByName("TXT_Gachapon", lang == 0 ? "GACHAPON" : lang == 1 ? "扭蛋機" : "GACHAPON");
        TranslateByName("TXT_Aquarium", lang == 0 ? "AQUARIUM" : lang == 1 ? "水族箱" : "AQUARIUM");

        // --- SCENE : GACHAPON SCENE ---
        TranslateByName("TXT_Spin", lang == 0 ? "SPIN FOR 5 SHELLS" : lang == 1 ? "花費 5 個貝殼旋轉" : "LANCER (5 COQUILLAGES)");
        TranslateByName("TXT_PrizeTitle", lang == 0 ? "NEW ITEM!" : lang == 1 ? "獲得新物品！" : "NOUVEL OBJET !");

        // --- SCENE : COLLECTION SCENE ---
        TranslateByName("TXT_Collection", lang == 0 ? "COLLECTION" : lang == 1 ? "收藏展覽" : "COLLECTION");

        // --- NOMS DES ITEMS (POUR GACHA & COLLECTION) ---
        // Utilise l'ID de l'item défini dans ton ScriptableObject (ex: "fish_01")
        TranslateItemName("fish_blue", lang);
        TranslateItemName("fish_clown", lang);
        TranslateItemName("fish_diodon", lang);
        TranslateItemName("fish_mask", lang);
        TranslateItemName("fish_yellow", lang);
        TranslateItemName("plant_anemone", lang);
        TranslateItemName("plant_coral", lang);
        TranslateItemName("plant_seaweed", lang);
        TranslateItemName("plant_rock", lang);
        // Ajoute ici tous tes IDs d'items...
    }

    private void TranslateByName(string objName, string translation)
    {
        // On cherche partout, même dans les objets désactivés
        TextMeshProUGUI[] allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();

        foreach (var t in allTexts)
        {
            // On vérifie si c'est le bon objet ET s'il appartient à la scène active
            // (pour éviter de modifier des objets d'autres scènes en mémoire)
            if (t.gameObject.name == objName && t.gameObject.scene.isLoaded)
            {
                t.text = translation;
                return; // On a trouvé, on s'arrête
            }
        }
    }

    // Vérifie bien le mot "public" au début !
    public string GetTranslatedItemName(string itemID)
    {
        int lang = PlayerPrefs.GetInt("SelectedLanguage", 0);
        switch (itemID)
        {
            case "fish_blue": return lang == 0 ? "Blue Tang" : lang == 1 ? "擬刺尾鯛" : "Chirurgien Bleu";
            case "fish_clown": return lang == 0 ? "Clownfish" : lang == 1 ? "小丑魚" : "Poisson-Clown";
            case "fish_diodon": return lang == 0 ? "Diodon" : lang == 1 ? "刺河豚" : "Poisson-Globe";
            case "fish_mask": return lang == 0 ? "Masked Bannerfish" : lang == 1 ? "馬夫魚" : "Poisson-Cocher";
            case "fish_yellow": return lang == 0 ? "Yellow Tang" : lang == 1 ? "黃高鰭刺尾魚" : "Chirurgien Jaune";
            case "plant_anemone": return lang == 0 ? "Sea Anemone" : lang == 1 ? "海葵" : "Anémone de Mer";
            case "plant_coral": return lang == 0 ? "Coral" : lang == 1 ? "珊瑚" : "Corail";
            case "plant_seaweed": return lang == 0 ? "Seaweed" : lang == 1 ? "海藻" : "Algue";
            case "plant_rock": return lang == 0 ? "Rock" : lang == 1 ? "岩石" : "Rocher";
            default: return itemID;
        }
    }

    private void TranslateItemName(string itemID, int lang)
    {
        string translation = "";
        switch (itemID)
        {
            case "fish_blue": translation = (lang == 0 ? "Blue Tang" : lang == 1 ? "擬刺尾鯛" : "Chirurgien Bleu"); break;
            case "fish_clown": translation = (lang == 0 ? "Clownfish" : lang == 1 ? "小丑魚" : "Poisson-Clown"); break;
            case "fish_diodon": translation = (lang == 0 ? "Diodon" : lang == 1 ? "刺河豚" : "Poisson-Globe"); break;
            case "fish_mask": translation = (lang == 0 ? "Masked Bannerfish" : lang == 1 ? "馬夫魚" : "Poisson-Cocher"); break;
            case "fish_yellow": translation = (lang == 0 ? "Yellow Tang" : lang == 1 ? "黃高鰭刺尾魚" : "Chirurgien Jaune"); break;
            case "plant_anemone": translation = (lang == 0 ? "Sea Anemone" : lang == 1 ? "海葵" : "Anémone de Mer"); break;
            case "plant_coral": translation = (lang == 0 ? "Coral" : lang == 1 ? "珊瑚" : "Corail"); break;
            case "plant_seaweed": translation = (lang == 0 ? "Seaweed" : lang == 1 ? "海藻" : "Algue"); break;
            case "plant_rock": translation = (lang == 0 ? "Rock" : lang == 1 ? "岩石" : "Rocher"); break;
            default: return; // Si l'ID est inconnu, on sort
        }

        // ON CHERCHE PARTOUT (Même les objets désactivés)
        TextMeshProUGUI[] allTexts = Resources.FindObjectsOfTypeAll<TextMeshProUGUI>();
        bool found = false;

        foreach (var txt in allTexts)
        {
            // ATTENTION : On vérifie si le nom de l'objet est EXACTEMENT TXT_itemID
            if (txt.gameObject.name == "TXT_" + itemID)
            {
                txt.text = translation;
                txt.font = (lang == 1) ? chineseFontAsset : englishFontAsset;
                txt.UpdateFontAsset();
                found = true;
            }
        }

        if (!found)
        {
            Debug.LogWarning("LanguageManager : Aucun objet texte nommé TXT_" + itemID + " n'a été trouvé dans la scène !");
        }
    }

    public string GetStoryDialog(int step, int lang)
    {
        if (lang == 0) // English
        {
            switch (step)
            {
                case 0: return "Our home is beautiful and peaceful...";
                case 1: return "But suddenly... trash is invading our water!";
                case 2: return "Fishermen are taking my friends away...";
                case 3: return "I am all alone now...";
                case 4: return "Please, traveler, you must help us clean and save the ocean!";
                default: return "";
            }
        }
        else if (lang == 1) // Chinese
        {
            switch (step)
            {
                case 0: return "我們的家園原本既美麗又和平...";
                case 1: return "但突然間... 垃圾正在入侵我們的水域！";
                case 2: return "漁夫正在抓走我的朋友們...";
                case 3: return "現在只剩下我一個人了...";
                case 4: return "旅人，拜託你，請救救我們，清理並守護這片海洋！";
                default: return "";
            }
        }
        else // lang == 2 : French
        {
            switch (step)
            {
                case 0: return "Notre foyer était si beau et paisible...";
                case 1: return "Mais soudain... les déchets ont envahi nos eaux !";
                case 2: return "Les pêcheurs emportent tous mes amis...";
                case 3: return "Je suis toute seule maintenant...";
                case 4: return "S'il te plaît, voyageur, aide-nous à nettoyer et sauver l'océan !";
                default: return "";
            }
        }
    }
}