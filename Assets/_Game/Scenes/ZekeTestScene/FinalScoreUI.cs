using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreUI : MonoBehaviour
{

    [SerializeField] IntVar leafAmount;
    [SerializeField] IntRef leafToWinTreshold;
    [SerializeField] GameObject[] childs;
    [SerializeField] IntVar leafToGetS;
    [SerializeField] IntVar leafToGetA;
    [SerializeField] Image rankA;
    [SerializeField] Image rankB;
    [SerializeField] Image rankS;


    // Start is called before the first frame update
    void Start()
    {
        gameObject.GetComponent<Image>().enabled = false;
        foreach (GameObject child in childs)
        {
            child.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // show the final score panel
    public void ShowFinalScore()
    {
        if (leafAmount.Value <= leafToWinTreshold.Value)
        {
            return;
        }

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // make the UI visible 
        gameObject.GetComponent<Image>().enabled = true;
        foreach (GameObject child in childs)
        {
            child.SetActive(true);
        }

        // update the numebr of leaf and display the rank
        GameObject.Find("LeafNumber5").GetComponent<Text>().text = leafAmount.Value.ToString();

        rankA.enabled = false;
        rankB.enabled = false;
        rankS.enabled = false;


        if (leafAmount.Value > leafToGetS.Value)
        {
            rankS.enabled = true;
        } else if (leafAmount.Value > leafToGetA.Value)
        {
            rankA.enabled = true;
        }
        else if (leafAmount.Value > leafToWinTreshold.Value)
        {
            rankB.enabled = true;
        }

    }

}
