using UnityEngine;
using UnityEngine.Events;

public class MovingBox : Interactable
{
    [Header("Moving Box Settings:")]
    [SerializeField] IntRef leafToWinTreshold;
    [SerializeField] IntVar leafAmount;

    [SerializeField] private UnityEvent onWin;
    [SerializeField] private UnityEvent onNotEnough;
    

    #region PROPERTIES
    int LeafAmount
    {
        get => leafAmount.Value;
    }
    int LeafToWinTreshold
    {
        get => leafToWinTreshold.Value;
    }

    #endregion
    
    public void CheckWin()
    {
        if (LeafAmount < LeafToWinTreshold)
        {
            onNotEnough.Invoke();
        }
        else
        {
            onWin.Invoke();
        }
    }
}
