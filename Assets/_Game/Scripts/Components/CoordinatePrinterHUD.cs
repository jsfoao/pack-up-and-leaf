using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CoordinatePrinterHUD : MonoBehaviour
{
    [SerializeField] Vector3Var playerPos;

    [SerializeField] Text xCoordText;
    [SerializeField] Text yCoordText;
    [SerializeField] Text zCoordText;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Vector3 pos = playerPos.Value;
            xCoordText.text = $"X:{pos.x.ToString()}";
            yCoordText.text = $"Y:{pos.y.ToString()}";
            zCoordText.text = $"Z:{pos.z.ToString()}";
        }
    }
}
