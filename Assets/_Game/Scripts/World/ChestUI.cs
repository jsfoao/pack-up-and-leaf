using UnityEngine;
using UnityEngine.Events;

public class ChestUI : MonoBehaviour
{
    [SerializeField] private UnityEvent onEnterActive;
    [SerializeField] private UnityEvent onExitActive;

    public void OnEnter()
    {
        if (GetComponent<Interactable>().active)
        {
            onEnterActive.Invoke();
        }
    }

    public void OnExit()
    {
        if (GetComponent<Interactable>().active)
        {
            onExitActive.Invoke();
        }
    }
}
