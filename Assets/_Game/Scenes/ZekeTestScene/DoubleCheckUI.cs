using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class DoubleCheckUI : MonoBehaviour
{
    [SerializeField] IntVar leafAmount;
    [SerializeField] IntRef leafToWinTreshold;
    [SerializeField] GameObject[] childs;
    [SerializeField] IntVar leafToGetS;
    [SerializeField] IntVar leafToGetA;
    [SerializeField] Image markA;
    [SerializeField] Image markB;
    [SerializeField] Image markS;
    [SerializeField] UnityEvent onShowDoubleCheck;

    private bool isShowing = false;
    [SerializeField] private HUDManager hudRef;
    [Header("Gamepad Navigation")] [SerializeField]
    private Selectable _selectable;

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
        if (Input.GetButtonDown("Roll") && isShowing)
        {
            CloseDoubleCheckUI();
        }
    }


    public void ShowDoubleCheckUI()
    {
        if (leafAmount.Value < leafToWinTreshold.Value)
        {
            return;
        }

        isShowing = true;
        hudRef.SetPaused();
        GameManager.Instance.SetEntityInput(false);

        onShowDoubleCheck.Invoke();
        // make the UI visible 
        gameObject.GetComponent<Image>().enabled = true;
        foreach (GameObject child in childs)
        {
            child.SetActive(true);
        }

        // display the right mark

        markA.enabled = false;
        markB.enabled = false;
        markS.enabled = false;

        if (leafAmount.Value > leafToGetS.Value)
        {
            markS.enabled = true;
        }
        else if (leafAmount.Value > leafToGetA.Value)
        {
            markA.enabled = true;
        }
        else if (leafAmount.Value > leafToWinTreshold.Value)
        {
            markB.enabled = true;
        }
        

    }


    public void CloseDoubleCheckUI()
    {
        isShowing = false;
        hudRef.SetUnpaused();
        GameManager.Instance.SetEntityInput(true);

        // make the UI visible 
        gameObject.GetComponent<Image>().enabled = false;
        foreach (GameObject child in childs)
        {
            child.SetActive(false);
        }
        

    }


}
