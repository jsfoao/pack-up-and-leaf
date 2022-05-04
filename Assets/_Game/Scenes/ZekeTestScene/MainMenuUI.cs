using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] GameObject[] childs;
    [SerializeField] SceneSwapper sceneSwaper;
    [SerializeField] float waitToStart=1f;
    [SerializeField] GameObject hedgeHog;
    [SerializeField] string gameSceneName;

    [SerializeField] AnimationCurve jumpInAnimation;
    [SerializeField] AnimationCurve jumpOutAnimation;
    [SerializeField] float animationDuration = 1f;
    [SerializeField] float startRectY;
    RectTransform thisRect;

    [SerializeField] private Selectable _selectable;
    private GlobalAudioManager audioManager;
    
    // Start is called before the first frame update
    void Start()
    {
        
        audioManager = FindObjectOfType<GlobalAudioManager>();
        thisRect = this.GetComponent<RectTransform>();
        hedgeHog.GetComponent<Animator>().ResetTrigger("StartButton");
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // resume time
        Time.timeScale = 1;
        // resume audio listener
        AudioListener.pause = false;
        
        EventSystem.current.SetSelectedGameObject(_selectable.gameObject);
    }

    public void PlayButton(string sceneName)
    {
        audioManager.StopSoundTrack("MainMenu");
        StartCoroutine(PlayButtonPressed(waitToStart));
        //sceneSwaper.LoadScene(sceneName);
    }

    public void CreditsButton()
    {

    }

    public void OptionsButton()
    {

    }

    // Exit the game when click the exit button on the main menu
    public void ExitButton()
    {
        Application.Quit();
    }

    IEnumerator PlayButtonPressed(float time)
    {
        hedgeHog.GetComponent<Animator>().SetTrigger("StartButton");
        yield return new WaitForSeconds(time);
        sceneSwaper.LoadScene(gameSceneName);
    }

    IEnumerator MainMenuAnimation(AnimationCurve thisAnimation)
    {
        float time = 0f;
        while (time < animationDuration)
        {
            Vector3 currentRect;
            currentRect.x = thisRect.anchoredPosition3D.x;
            currentRect.z = thisRect.anchoredPosition3D.z;
            currentRect.y = thisAnimation.Evaluate(time);
            thisRect.anchoredPosition3D = currentRect;
            time += Time.unscaledDeltaTime;
            yield return null;
        }
    }

    public void CloseMainMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        StartCoroutine(MainMenuAnimation(jumpOutAnimation));
    }

    public void OpenMainMenu()
    {
        EventSystem.current.SetSelectedGameObject(_selectable.gameObject);
        StartCoroutine(MainMenuAnimation(jumpInAnimation));
    }


}
