using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInput
{
    Vector2 InputDirection
    {
        get;
        set;
    }

    bool Jump
    {
        get;
        set;
    }
    
    bool JumpHold
    {
        get;
        set;
    }

    bool JumpUp
    {
        get;
        set;
    }

    bool StartRoll
    {
        get;
        set;
    }

    bool StopRoll
    {
        get;
        set;
    }
}
