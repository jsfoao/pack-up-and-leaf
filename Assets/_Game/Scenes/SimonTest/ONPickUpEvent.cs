using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ONPickUpEvent : MonoBehaviour
{
    
    private int health;
    public int score;
    private LerpToPlayer _lerpToPlayer;
   
    private void Start()
    {
        
      
        _lerpToPlayer = GameObject.FindGameObjectWithTag("Leaf").GetComponent<LerpToPlayer>();
      

    }

    void ScoreCount(int points, GameObject gameObject)
    {
        if (gameObject.tag == "Leaf")
        {
            score = score + points;
        }

        if (gameObject.tag == "Berry")
        {
            
            Debug.Log("health");
        }
         
                Debug.Log("the score is : " + score);
        
       
        
       
    }
    private void OnEnable()
    {
      
        //LerpToPlayer.onAddPickUp += ScoreCount;
    }

    private void OnDisable()
    {
        //LerpToPlayer.onAddPickUp -= ScoreCount;
    }
    
    
    
    

}
