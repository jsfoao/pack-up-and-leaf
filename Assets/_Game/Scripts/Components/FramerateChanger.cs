using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FramerateChanger : MonoBehaviour
{
    [SerializeField] int defaultFramerate;
    [SerializeField] int shitFramerate;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Y))
        {
            Application.targetFrameRate = defaultFramerate;
            
        }

        if (Input.GetKeyDown(KeyCode.T))
        {
            Application.targetFrameRate = shitFramerate;

        }
    }
}
