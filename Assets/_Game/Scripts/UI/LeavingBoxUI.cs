using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ScriptableEvents;

public class LeavingBoxUI : MonoBehaviour
{
    [SerializeField] GameObject panelObj;
    [SerializeField] Text leafCountText;
    [SerializeField] Text leafRequiredText;
    [SerializeField] Text rdyText;

    [SerializeField] IntVar leafCount;
    [SerializeField] IntVar leafRequired;

    [SerializeField] string notRDYstring;
    [SerializeField] string isRDYstring;
    [SerializeField] ScriptableEventVoid b;

    

    public void OnBoxClose()
    {
        panelObj.SetActive(false);
    }

    public void OnBoxLeave()
    {
        panelObj.SetActive(true);
    }

    public void OnLeafUpdate()
    {
        
        leafCountText.text = leafCount.Value.ToString();
        if (leafCount.Value >= leafRequired.Value)
        {
            rdyText.text = isRDYstring;
        }
        
    }

    private void Start()
    {
        leafRequiredText.text = leafRequired.Value.ToString();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            leafCount.Value = leafCount.Value + 1;
            b.Raise();

        }
    }
}
