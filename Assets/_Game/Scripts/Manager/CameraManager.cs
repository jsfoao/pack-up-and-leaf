using System;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField] private GameCamera[] cameras;

    public void ActivateCamera(string name)
    {
        foreach (GameCamera gameCamera in cameras)
        {
            if (gameCamera.Name == name)
            {
                gameCamera.Camera.enabled = true;
                if (gameCamera.AudioListener != null)
                {
                    gameCamera.AudioListener.enabled = true;
                }
            }
            else
            {
                gameCamera.Camera.enabled = false;
                
                if (gameCamera.AudioListener != null)
                {
                    gameCamera.AudioListener.enabled = false;
                }
            }
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            ActivateCamera("player camera");
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            ActivateCamera("alternative camera");
        }
    }
}

[Serializable]
public class GameCamera
{
    public string Name;
    public Camera Camera;
    public AudioListener AudioListener;
}
