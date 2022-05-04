using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsUI : MonoBehaviour
{

    public bool creditsMenuIsOpen = false;
    RectTransform thisRect;
    //[SerializeField] GameObject[] childs;
    [SerializeField] GameObject creditsMenu;

    [SerializeField] AnimationCurve jumpInAnimation;
    [SerializeField] AnimationCurve jumpOutAnimation;
    [SerializeField] float animationDuration = 1f;
    [SerializeField] float startRectY;

    [SerializeField] private MainMenuUI _mainMenuUI;
    

    // Start is called before the first frame update
    void Start()
    {
        thisRect = creditsMenu.GetComponent<RectTransform>();
        creditsMenu.SetActive(false);
    }

    
    IEnumerator OptionAnimation(AnimationCurve thisAnimation)
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

    public void OpenCreditsUI()
    {
        //isPaused = true;
        //CallOptionMenu();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        creditsMenuIsOpen = true;
        creditsMenu.SetActive(true);
        StartCoroutine(OptionAnimation(jumpInAnimation));

    }

    public void CloseCreditsUI()
    {
        //Cursor.visible = false;
        // pause the time
        //Time.timeScale = 1;

        //UIjumpOut = true;
        //UIjumpIn = false;
        //bounce = false;
        creditsMenuIsOpen = false;
        creditsMenu.SetActive(false);
        StartCoroutine(OptionAnimation(jumpOutAnimation));
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Roll") && creditsMenuIsOpen)
        {
            CloseCreditsUI();
            _mainMenuUI.OpenMainMenu();
        }
    }
}
