using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class PauseMenuUI : MonoBehaviour
{
    bool isPaused=false;
    bool UIjumpOut = false;
    bool UIjumpIn = false;
    bool bounce = false;
    bool optionMenuOpen =false;
    [Header("UI Setup")]
    [SerializeField] Image leafCounterUI;
    [SerializeField] Image healthUI;
    [SerializeField] GameObject pauseMenu;
    [SerializeField] IntVar leafAmount;
    [SerializeField] IntVar berryCount;
    [SerializeField] IntVar isOpen;
    [SerializeField] IntVar movingOpen;
    [Header("Button Setup")]
    [SerializeField] float reloadTimer = 2f;
    [Header("Animation Setup")]
    [SerializeField] float lerpDuration = 1f;
    [SerializeField] Vector3 startOffset;
    [SerializeField] AnimationCurve jumpInAnimation;
    [SerializeField] AnimationCurve jumpOutAnimation;
    [SerializeField] float animationDuration = 1f;
    bool animationIsRunning = false;
    [Header("Audio Snapshots")]
    [SerializeField] AudioMixerSnapshot paused;
    [SerializeField] AudioMixerSnapshot unpaused;
    [SerializeField] float transitionTime = 0.01f;
    private Vector3 targetPosition;
    private RectTransform pauseMenuRect;

    // Menu navigation
    [Header("Gamepad Navigation")]
    [SerializeField] private Selectable _selectable;
    
    
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        isPaused = false;
        
        pauseMenuRect = pauseMenu.GetComponent<RectTransform>();//get pauseMenu recttransform
        targetPosition = pauseMenuRect.anchoredPosition3D; //save target position.
        pauseMenuRect.anchoredPosition3D = startOffset; //congurate the initial position.
    }

    // Update is called once per frame
    void Update()
    {
        optionMenuOpen = GetComponent<OptionMenuUI>().optionMenuIsOpen;
        if(movingOpen.Value !=0) { return; }
        if(animationIsRunning == true) { return; }
        if(optionMenuOpen == true) { return; }
        if (Input.GetButtonDown("Pause"))// && movingOpen.Value == 0)
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                ShowPauseMenu();
            } 
            else
            {
                ClosePauseMenu();
            }
        }

    }




    public void ShowPauseMenu()
    {
        PauseGame();
        
        // display the UI
        healthUI.GetComponent<HealthCounterUI>().HealthUIFadeOut();
        //gameObject.GetComponent<Image>().enabled = true;
        pauseMenu.SetActive(true);
        //Start Animation
        StartCoroutine(UIAnimation(jumpInAnimation));
        // update the leaf number
        GameObject.Find("LeafNumber1").GetComponent<Text>().text = leafAmount.Value.ToString();
        // update the berry number
        GameObject.Find("SecretNumber").GetComponent<Text>().text = berryCount.Value.ToString();

        UIjumpOut = false;
        UIjumpIn = true;
    }

    private void PauseGame()
    {
        // Enables pause navigation
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_selectable.gameObject);

        isOpen.Value = 1;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        // pause the time
        Time.timeScale = 0;
        // Disable player input
        if (GameManager.Instance.GetPlayerEntity() != null)
        {
            GameManager.Instance.SetEntityInput(false);
        }
        // pause audio listener;
        AudioListener.pause = true;
        paused.TransitionTo(transitionTime);
    }

    public void ClosePauseMenu()
    {
        if(optionMenuOpen==false)
        {
            UnpauseGame();
        }

        //start animation
        StartCoroutine(UIAnimation(jumpOutAnimation));
        leafCounterUI.enabled = true;
        healthUI.GetComponent<HealthCounterUI>().HealthUIFadeIn();
    }

    private void UnpauseGame()
    {
        // Clears navigation event
        EventSystem.current.SetSelectedGameObject(null);
        
        isOpen.Value = 0;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        // resume time
        Time.timeScale = 1;
        // Enable player input
        if (GameManager.Instance.GetPlayerEntity() != null)
        {
            GameManager.Instance.SetEntityInput(true);
        }
        // resume audio listener
        AudioListener.pause = false;
        unpaused.TransitionTo(transitionTime);
    }

    //All button functions start from here.  

    public void ResumeButton()
    {
        
        isPaused = false;
        UnpauseGame();
        StartCoroutine(UIAnimation(jumpOutAnimation));
        //isOpen.Value = 0;
        //Cursor.lockState = CursorLockMode.Locked;
        //Cursor.visible = false;
        //// pause the time
        //Time.timeScale = 1;
        //AudioListener.pause = false; 
        //unpaused.TransitionTo(transitionTime);

        leafCounterUI.enabled = true;
        healthUI.GetComponent<HealthCounterUI>().HealthUIFadeIn();
        //UIjumpOut = true;
        //UIjumpIn = false;
        //bounce = false;
    }

    public void RestartButton()
    {
        Time.timeScale = 1f;
        
        //StartCoroutine(WaitForSeconds(reloadTimer));
        unpaused.TransitionTo(transitionTime);
        AudioListener.pause = false;
        string currentScene = SceneManager.GetActiveScene().name;
        leafAmount.Value = 0;
        berryCount.Value = 0;
        SceneManager.LoadScene(currentScene);


    }

    public void SendBackPauseMenu()
    {
        StartCoroutine(UIAnimation(jumpOutAnimation));
    }
    //IEnumerator WaitForSeconds(float waitTime)
    //{
    //    yield return new WaitForSeconds(waitTime);
    //}

    //IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    //{
    //    float time = 0;
    //    Vector3 currentPosition = pauseMenuRect.anchoredPosition3D;
    //    while(time<duration)
    //    {
    //        float t = time / duration;
    //        float lerpSmooth = t * t * (3f - 2f * t);
    //        pauseMenuRect.anchoredPosition3D = Vector3.Lerp(currentPosition, targetPosition, lerpSmooth);
    //        time += Time.unscaledDeltaTime;
    //        yield return null;
    //    }
    //    pauseMenuRect.anchoredPosition3D = targetPosition;
    //}

    IEnumerator UIAnimation(AnimationCurve thisAnimation)
    {
        float time = 0f;
        while (time < animationDuration)
        {
            animationIsRunning = true;
            Vector3 currentRect;
            currentRect.x = pauseMenuRect.anchoredPosition3D.x;
            currentRect.z = pauseMenuRect.anchoredPosition3D.z;
            currentRect.y = thisAnimation.Evaluate(time);
            pauseMenuRect.anchoredPosition3D = currentRect;
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        animationIsRunning = false;
    }

    public void Quit( string sceneName)
    {
        UnpauseGame();
        SceneManager.LoadScene(sceneName);
    }
}
