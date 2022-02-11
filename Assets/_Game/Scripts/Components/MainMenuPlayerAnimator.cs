using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuPlayerAnimator : MonoBehaviour
{
    [SerializeField] GameObject hedgehogObject;
    [SerializeField] GameObject ballObject;
    [SerializeField] Transform gotoTransform;

    [SerializeField] float rollSpeed;
    [SerializeField] float rotationSpeed;

    bool rolling;

    private void Update()
    {
        if (rolling)
        {
            //Location
            Vector3 toPos = Vector3.MoveTowards(ballObject.transform.position, gotoTransform.position, rollSpeed * Time.deltaTime);
            ballObject.transform.position = toPos;

            //Rotation
            Quaternion rot = Quaternion.AngleAxis(rotationSpeed * Time.deltaTime, transform.up);
            ballObject.transform.rotation *= rot;
        }
    }

    public void StartRolling()
    {
        rolling = true;

        hedgehogObject.SetActive(false);
        ballObject.SetActive(true);
    }
}
