using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableEvents;
public class Leaf : MonoBehaviour
{
   private bool IsLerping=false;
   
   private Rigidbody rb;
   
   public int Points=1;
   public ScriptableEventVoid OnLeafPickup;
   public Vector3Var playerPos;
   public IntVar LeafCount;
   
   private float distance;
   
   public float animSpeed = 3;
   

   
  public Vector3 Playerpos
  {
     get { return playerPos.Value; }
   
  }
  private void Start()
  {
     rb = GetComponent<Rigidbody>();
  }

  private void Update()
  {
      distance = (gameObject.transform.position - playerPos.Value).magnitude;
    
  }

  private void OnDrawGizmos()
   {
       
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


      while (distance != 0)
      {
        
           gameObject.transform.position = Vector3.Lerp(gameObject.transform.position, playerPos.Value, animSpeed * Time.deltaTime);
                  yield return new WaitForEndOfFrame();
      }
      
      //eventual alternative instead of lerping
         //rb.AddForce(playerpos.Value-transform.position,ForceMode.Impulse);
        
      
      

   }

   private void OnTriggerEnter(Collider other) {
      if (other.CompareTag("Player"))
      {
         
         LeafCount.Value = LeafCount.Value + 1;
         Debug.Log(LeafCount.Value);
         OnLeafPickup.Raise();
         Destroy(gameObject);
      } 
   }
}
