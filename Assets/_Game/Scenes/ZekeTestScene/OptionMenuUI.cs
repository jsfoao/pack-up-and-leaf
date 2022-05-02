using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class OptionMenuUI : MonoBehaviour
{

    bool isPaused;
    public bool optionMenuIsOpen = false;
    //bool UIjumpOut = false;
    //bool UIjumpIn = false;
    //bool bounce = false;
    //float originY;
    RectTransform thisRect;
    //[SerializeField] GameObject[] childs;
    [SerializeField] GameObject optionMenu;

    [SerializeField] AnimationCurve jumpInAnimation;
    [SerializeField] AnimationCurve jumpOutAnimation;
    [SerializeField] float animationDuration=1f;
    [SerializeField] float startRectY;

    [Header("UI sliders")]
    [SerializeField] Slider masterSlider;
    [SerializeField] Slider musicSlider;
    [SerializeField] Slider soundSlider;
    [SerializeField] Slider mouseSlider;
    [SerializeField] Toggle mouseToggle;


    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float sliderMultiplier;
    float mouseSensitivity;
    bool mouseBool;

    [Header("Gamepad Navigation")]
    [SerializeField] private Selectable _selectable;
    [SerializeField] private Selectable _pauseMenuSelectable;

    void Start()
    {
        //isPaused = false;
        //gameObject.GetComponent<Image>().enabled = false;
        //originY = transform.position.y;
        ////transform.Translate(0f, -800f, 0f);
        //foreach (GameObject child in childs)
        //{
        //    child.SetActive(false);
        //}
        thisRect = optionMenu.GetComponent<RectTransform>();
        optionMenu.SetActive(false);
        masterSlider.onValueChanged.AddListener(ChangeMasterSlider);
        musicSlider.onValueChanged.AddListener(ChangeMusicSlider);
        soundSlider.onValueChanged.AddListener(ChangeSoundSlider);
        mouseSlider.onValueChanged.AddListener(ChangeMouseSensitivity);
    }

    // Update is called once per frame
    void Update()
    {

        //if (UIjumpOut)
        //{
        //    transform.Translate(0f, -12f, 0f);
        //    if (transform.position.y < -1000)
        //    {
        //        UIjumpOut = false;
        //    }
        //}

        //if (UIjumpIn)
        //{

        //    if (bounce == false)
        //    {
        //        transform.Translate(0f, 12f, 0f);
        //    }
        //    else
        //    {
        //        transform.Translate(0f, -0.9f, 0f);
        //    }

        //    if (transform.position.y > originY + 50)
        //    {
        //        bounce = true;
        //    }

        //    if (transform.position.y < originY && bounce)
        //    {
        //        UIjumpIn = false;
        //    }
        //}



    }


    // Call or Exit option menu every time you press ESC
    void CallOptionMenu()
    {
 

        // Show the pause menu
        if (isPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            // pause the time
            Time.timeScale = 0;

            //// display the UI
            //gameObject.GetComponent<Image>().enabled = true;
            //foreach (GameObject child in childs)
            //{
            //    child.SetActive(true);
            //}

            //// Play the animation
            //UIjumpOut = false;
            //UIjumpIn = true;


        }

        // Exit the pause menu
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            // pause the time
            Time.timeScale = 1;

        }
    }



    // Close the UI when click the close icon
    public void CloseOptionUI()
    {
        // Enables options navigation
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_pauseMenuSelectable.gameObject);
        
        //Cursor.visible = false;
        // pause the time
        //Time.timeScale = 1;

        //UIjumpOut = true;
        //UIjumpIn = false;
        //bounce = false;
        optionMenuIsOpen = false;
        optionMenu.SetActive(false);
        StartCoroutine(OptionAnimation(jumpOutAnimation));
    }

    // Close the UI when click the close icon
    public void OpenOptionUI()
    {
        // Enables options navigation
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(_selectable.gameObject);
        
        //isPaused = true;
        //CallOptionMenu();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        optionMenuIsOpen = true;
        optionMenu.SetActive(true);
        StartCoroutine(OptionAnimation(jumpInAnimation));
    }

    IEnumerator OptionAnimation(AnimationCurve thisAnimation)
    {
        float time = 0f;
        while(time<animationDuration)
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

    public void ChangeMasterSlider(float value)
    {

        audioMixer.SetFloat("masterVolume", CalculateVolumeValue(value));
    }

    public void ChangeMusicSlider(float value)
    {
        audioMixer.SetFloat("musicVolume", CalculateVolumeValue(value));
    }

    public void ChangeSoundSlider(float value)
    {
        audioMixer.SetFloat("soundVolume", CalculateVolumeValue(value));
    }

    private float CalculateVolumeValue(float value)
    {
        return Mathf.Log10(Mathf.Clamp(value, 0.001f, Mathf.Infinity)) * sliderMultiplier;
    }

    public void ChangeMouseSensitivity(float value)
    {
        mouseSensitivity = value;
    }

    public void ChangeMouseBool(bool yesOrNo)
    {
        mouseBool = yesOrNo;
    }

    public float ReturnMouseValue()
    {
        return mouseSensitivity;
    }

    public bool ReturnMouseBool()
    {
        return mouseBool;
    }
}



