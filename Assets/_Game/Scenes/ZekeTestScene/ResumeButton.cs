using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class ResumeButton : MonoBehaviour
{
    public Image resumeImg;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ColorBlock colors = this.GetComponent<Button>().colors;
        colors.normalColor = Color.white;
    }

}
