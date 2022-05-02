using UnityEngine;
using UnityEngine.UI;

public class ImageManagerUI : MonoBehaviour
{
    public void SetImageColor(Color color)
    {
        GetComponent<Image>().color = color;
    }

    public void SetSelectedColor()
    {
        SetImageColor(Color.red);
    }

    public void SetDeselectedColor()
    {
        SetImageColor(Color.white);
    }
}
