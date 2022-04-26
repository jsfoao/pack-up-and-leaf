using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableEvents;
using Unity.Collections;
using UnityEngine.Audio;


public class LerpToPlayer : MonoBehaviour
{
 
     
     
     //public delegate void AddPickUp(int points,GameObject gameObject);
     //public static event AddPickUp onAddPickUp;
     
     
     
     private bool IsLerping=false;
     
     private Rigidbody rb;
     private bool TimerIsActive;
     private AudioSource pickupSFXComponent;
     [SerializeField] private AudioClip PickUpSFX;
     public int Points;
     public ScriptableEventVoid OnPickup;
     public Vector3Var playerPos;

     public float Timer=1;
     private float distance;
     
     public float PickUpSpeed = 3;
     private float pickUpSpeed=14;
     
  
     
    public Vector3 Playerpos
    {
       get { return playerPos.Value; }
     
    }
    private void Start()
    {
       pickupSFXComponent= GetComponent<AudioSource>();
       // icallevent = GetComponent<ICallEvent>();
       //rb = GetComponent<Rigidbody>();
    }
  
    private void Update()
    {
        distance = (gameObject.transform.position - playerPos.Value).magnitude;
        if (TimerIsActive == true)
        {
           timer();   
        }
        
    }
    
    public void StartLerp()
     {
        
        if (!IsLerping)
        {
           StartCoroutine(LerpLoop());
           IsLerping = true;
        }
        
  
     }
  
     private IEnumerator LerpLoop()
     { 
        TimerIsActive = true;
        while (distance != 0)
        {
           
          
             gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, playerPos.Value,
                PickUpSpeed * Time.deltaTime);
             
             yield return new WaitForEndOfFrame();
        }

     }
  
     private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player"))
        {


           if (gameObject.tag == "Leaf")
           {
              OnPickup.Raise();
              other.gameObject.GetComponent<PlayerManager>()?.OnLeafCollision();
              pickupSFXComponent.PlayOneShot(PickUpSFX);
              Debug.Log("playing sound");
           }
           else if (gameObject.tag == "Berry")
           {
              OnPickup.Raise();
              other.gameObject.GetComponent<PlayerManager>()?.OnHealthCollision();
              pickupSFXComponent.PlayOneShot(PickUpSFX);
            }

           GetComponent<ComponentManager>().DisableComponents();
           //StartCoroutine(destroyAfterSound());
           Destroy(gameObject,pickupSFXComponent.clip.length);

          
        } 
     }

     IEnumerator destroyAfterSound()
     {

        
        yield return new WaitForSeconds(pickupSFXComponent.clip.length);
     }

     void timer()
     {
       
    
        if (Timer > 0)
        {
           
           Timer -= Time.deltaTime;
        }
        else if (Timer <= 0)
        {

           pickUpSpeed = PickUpSpeed;
           PickUpSpeed = 30;
        }
        
     }
  
   
     
  
}


