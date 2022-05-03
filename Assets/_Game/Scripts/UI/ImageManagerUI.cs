using System;
using UnityEngine;
using UnityEngine.UI;

public class ImageManagerUI : MonoBehaviour
{
    [SerializeField] private Color selectedColor;
    [SerializeField] private Color deselectedColor;

    private void Start()
    {
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
}
