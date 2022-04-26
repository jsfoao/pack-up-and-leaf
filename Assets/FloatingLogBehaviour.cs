using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class FloatingLogBehaviour : MonoBehaviour
{
    private Animator anim;
    public float forceThreshhold;
    private Rigidbody rb;
    private AudioSource logAudio;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponentInParent<Animator>();
        logAudio = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("entered");
            anim.SetBool("Landed",true);
            logAudio.PlayOneShot(logAudio.clip);
            
        }

       /* if (other.gameObject.CompareTag("Player"))
        {
            //Debug.Log(rb.velocity.y);
            //rb = other.GetComponent<Rigidbody>();
            //if (rb.velocity.y > forceThreshhold)
               // anim.Play("Log_Bob", 1, 0);
                anim.SetTrigger("Bob");
        }*/
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
            anim.SetBool("Landed", false);
    }

}
