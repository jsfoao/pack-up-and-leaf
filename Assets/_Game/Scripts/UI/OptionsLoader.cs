using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsLoader : MonoBehaviour
{
    private float masterSettings;
    private float musicSettings;
    private float sfxSettings;
    private float mouseSensSettings;
    private bool axisSettings;

    private OptionMenuUI optionMenuUI;
    public static OptionsLoader Instance;
    
    public void SaveSettings()
    {
        Debug.Log("Settings saved");
        masterSettings = optionMenuUI.masterSlider.value;
        musicSettings = optionMenuUI.musicSlider.value;
        sfxSettings = optionMenuUI.soundSlider.value;
        mouseSensSettings = optionMenuUI.mouseSlider.value;
        axisSettings = optionMenuUI.mouseToggle.isOn;
    }

    public void LoadSettings()
    {
        Debug.Log("Settings loaded");
        optionMenuUI.masterSlider.value = masterSettings;
        optionMenuUI.musicSlider.value = musicSettings;
        optionMenuUI.soundSlider.value = sfxSettings;
        optionMenuUI.mouseSlider.value = mouseSensSettings;
        optionMenuUI.mouseToggle.isOn = axisSettings;
    }

    public void LoadDefaultSettings()
    {
        masterSettings = 1;
        musicSettings = 1;
        sfxSettings = 1;
        mouseSensSettings = 1;
        axisSettings = false;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Look for option menu UI when scene loads
        optionMenuUI = FindObjectOfType<OptionMenuUI>();
        if (optionMenuUI != null)
        {
            Debug.Log("Option Menu UI found!");
        }
        else
        {
            Debug.LogWarning("Missing Option Menu UI");
        }
    }

    private void Awake()
    {
        DontDestroyOnLoad(this);
        
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        LoadDefaultSettings();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

}
