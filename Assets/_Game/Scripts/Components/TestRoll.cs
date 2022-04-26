using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestRoll : MonoBehaviour
{
    [SerializeField] Collider normalTransform;
    [SerializeField] Collider ballTransform;
    [SerializeField] Rigidbody rb;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            normalTransform.enabled = false;
            ballTransform.enabled = true;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            normalTransform.enabled = true;
            ballTransform.enabled = false;
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            rb.AddForce(new Vector3(0, 3, 3), ForceMode.Impulse);
        }
    }
}
