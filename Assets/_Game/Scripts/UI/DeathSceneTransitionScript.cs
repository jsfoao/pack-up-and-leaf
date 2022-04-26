using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ScriptableEvents;

public class DeathSceneTransitionScript : MonoBehaviour
{
    [SerializeField] ScriptableEventVoid onDeath;
    Animator anim;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    private void OnEnable()
    {
        onDeath.Register(PlayInTransition);
    }

    private void OnDisable()
    {
        onDeath.Unregister(PlayInTransition);
    }

    void PlayInTransition()
    {
        anim.Play("NewAnimIN");
    }


}
