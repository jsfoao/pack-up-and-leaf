using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageManagerUI : MonoBehaviour
{
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;
    [SerializeField] private Sprite keyboardSprite;
    [SerializeField] private Sprite gamepadSprite;

    [SerializeField] private bool simpleMessage;
    
    

    private void Start()
    {
        if (simpleMessage)
        {
            return;
        }
        SetDeselectedColor();
    }

    public void SetImageColor(Color color)
    {
        GetComponent<Image>().color = color;
    }

    public void SetSelectedColor()
    {
        SetImageColor(selectedColor);
    }

    public void SetDeselectedColor()
    {
        SetImageColor(deselectedColor);
    }

    public void SetGamepadSprite()
    {
        GetComponent<Image>().sprite = gamepadSprite;
    }

    public void SetKeyboardSprite()
    {
        GetComponent<Image>().sprite = keyboardSprite;
    }
}
