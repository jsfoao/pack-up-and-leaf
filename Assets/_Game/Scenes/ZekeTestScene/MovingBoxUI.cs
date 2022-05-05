using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MovingBoxUI : MonoBehaviour
{

    bool isPaused;
    bool isApproach;
    bool UIjumpOut = false;
    bool UIjumpIn = false;
    bool bounce = false;
    [SerializeField] Transform movingBoxPos;
    [SerializeField] IntVar leafAmount;
    [SerializeField] IntVar berryCount;
    [SerializeField] float activateUIDist;
    //[SerializeField] GameObject[] childs;
    float originY;
    [SerializeField] IntVar isOpen;
    [SerializeField] IntRef leafToWinTreshold;
    [SerializeField] IntVar leafToGetS;
    [SerializeField] IntVar leafToGetA;
    [SerializeField] IntVar pauseOpen;
    [SerializeField] Vector3Var playerPosition;
    [SerializeField] Image pass;
    [SerializeField] Image notPass;
    [SerializeField] Image movingBoxMsg;
    [SerializeField] Image openChestMsg;
    [SerializeField] Image skipMsg;

    [SerializeField] float animationDuration;
    [SerializeField] bool animationIsRunning;
    [SerializeField] AnimationCurve jumpInAnimation;
    [SerializeField] AnimationCurve jumpOutAnimation;
    [SerializeField] GameObject movingBoxUI;

    [SerializeField] GameObject bigHouse;
    [SerializeField] GameObject middleHouse;
    [SerializeField] GameObject smallHouse;

    RectTransform movingBoxRect;

    [SerializeField] private HUDManager hudRef;
    [Header("Gamepad Navigation")] [SerializeField]
    private Selectable _selectable;

    Vector3 PlayerPosition
    {
        get => playerPosition.Value;
    }


    void Start()
    {
        isPaused = false;
        movingBoxRect = movingBoxUI.GetComponent<RectTransform>();
        //gameObject.GetComponent<Image>().enabled = false;
        //originY = transform.position.y;
        //transform.Translate(0f, -800f, 0f);
        //foreach (GameObject child in childs)
        //{
        //    child.SetActive(false);
        //}
    }

    private void Update()
    {
        if (Input.GetButtonDown("Roll"))
        {
            if (isPaused)
            {
                KeepPlayButton();
            }
        }
    }


    // Call or Exit pause menu every time you press ESC
    void CallMovingBoxMenu()
    {
        // Show the pause menu
        if (isPaused)
        {
            isOpen.Value = 1;
            
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(_selectable.gameObject);
            
            // Pause cursor buffer
            hudRef.SetPaused();

            // Disable player input
            GameManager.Instance.SetEntityInput(false);
            
            // pause the time
            Time.timeScale = 0;
            AudioListener.pause = true;

            // display the UI
            //gameObject.GetComponent<Image>().enabled = true;
            //foreach (GameObject child in childs)
            //{
            //    child.SetActive(true);
            //}
            StartCoroutine(UIAnimation(jumpInAnimation));
            // update the leaf number
            GameObject.Find("LeafNumber3").GetComponent<Text>().text = leafAmount.Value.ToString();

            // update the berry number
            GameObject.Find("BerryNumber").GetComponent<Text>().text = berryCount.Value.ToString();

            UIjumpOut = false;
            UIjumpIn = true;

            // Show if the player have reached the number
            if (leafAmount.Value >= leafToWinTreshold.Value)
            {
                pass.enabled = true;
                notPass.enabled = false;
            } else
            {
                pass.enabled = false;
                notPass.enabled = true;
            }

        }

        // Exit the pause menu
        else
        {
            isOpen.Value = 0;
            
            EventSystem.current.SetSelectedGameObject(null);
            
            // Pause cursor buffer
            hudRef.SetUnpaused();
            
            // Enable player input
            GameManager.Instance.SetEntityInput(true);
            
            // pause the time
            Time.timeScale = 1;
            AudioListener.pause = false;
            StartCoroutine(UIAnimation(jumpOutAnimation));

            UIjumpOut = true;
            UIjumpIn = false;
            bounce = false;
        }
    }

    public void KeepPlayButton()
    {
        isPaused = !isPaused;
        CallMovingBoxMenu();
        ShowMovingBoxMsg();
    }

    public void RebuildHouseButton()
    {
        // Close the UI of moving box
        if (leafAmount.Value >= leafToWinTreshold.Value) {
            isPaused = !isPaused;
            CallMovingBoxMenu();
        }
    }

    public void ShowMovingBoxMsg()
    {
        movingBoxMsg.enabled = true;
    }

    public void CloseMovingBoxMsg()
    {
        movingBoxMsg.enabled = false;
    }

    public void ShowChestMsg()
    {
        bool usingMouse = GameManager.Instance.GetPlayerEntity().GetComponent<InputManager>().usingMouse;
        if (usingMouse)
        {
            openChestMsg.GetComponent<ImageManagerUI>().SetKeyboardSprite();
        }
        else
        {
            openChestMsg.GetComponent<ImageManagerUI>().SetGamepadSprite();
        }

        openChestMsg.enabled = true;
    }

    public void CloseChestMsg()
    {
        openChestMsg.enabled = false;
    }


    public void ShowSKipMsg()
    {
        Debug.Log("Show skip message");
        bool usingMouse = GameManager.Instance.GetPlayerEntity().GetComponent<InputManager>().usingMouse;
        if (usingMouse)
        {
            skipMsg.GetComponent<ImageManagerUI>().SetKeyboardSprite();
        }
        else
        {
            skipMsg.GetComponent<ImageManagerUI>().SetGamepadSprite();
        }

        skipMsg.enabled = true;
    }

    public void CloseSKipMsg()
    {
        skipMsg.enabled = false;
    }

    public void CallMovingBox()
    {
        isPaused = !isPaused;
        CallMovingBoxMenu();
    }

    public void CloseMovingBoxUI()
    {
        StartCoroutine(UIAnimation(jumpOutAnimation));
    }

    IEnumerator UIAnimation(AnimationCurve thisAnimation)
    {
        float time = 0f;
        while (time < animationDuration)
        {
            animationIsRunning = true;
            Vector3 currentRect;
            currentRect.x = movingBoxRect.anchoredPosition3D.x;
            currentRect.z = movingBoxRect.anchoredPosition3D.z;
            currentRect.y = thisAnimation.Evaluate(time);
            movingBoxRect.anchoredPosition3D = currentRect;
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        animationIsRunning = false;
    }

    public void ShowHouseModel()
    {
        bigHouse.SetActive(false);
        middleHouse.SetActive(false);
        smallHouse.SetActive(false);

        if (leafAmount.Value > leafToGetS.Value)
        {
            GameManager.Instance.SetEntityInput(false);
            bigHouse.SetActive(true);
        }
        else if (leafAmount.Value > leafToGetA.Value)
        {
            GameManager.Instance.SetEntityInput(false);
            middleHouse.SetActive(true);
        }
        else if (leafAmount.Value > leafToWinTreshold.Value)
        {
            GameManager.Instance.SetEntityInput(false);
            smallHouse.SetActive(true);
        }
    }
}

