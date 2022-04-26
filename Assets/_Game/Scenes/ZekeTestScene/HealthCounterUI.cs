using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCounterUI : MonoBehaviour
{
    public bool isVisible = true;
    float originX;

    int currentHealth;
    public Image health1;
    public Image health2;
    public Image health3;

    [Header("Animation")] //UI animation set up starts here.
    [SerializeField] Vector3 fadeOutPosition; //the recttransform position when UI faded out
    [SerializeField] Vector3 fadeInPosition; //the recttransform position when UI faded in
    [SerializeField] float lerpDuration = 0.8f;
    private RectTransform healthRecttransform;
    void Start()
    {
        currentHealth = GameObject.Find("Player").GetComponent<PlayerEntity>().healthRef.Health;
        healthRecttransform = gameObject.GetComponent<RectTransform>();
        healthRecttransform.anchoredPosition3D = fadeInPosition;
    }

    // Update is called once per frame
    void Update()
    {

        currentHealth = GameObject.Find("Player").GetComponent<PlayerEntity>().healthRef.Health;

        DisplayHealthUI(currentHealth);
    }

    public void HealthUIFadeIn()
    {
        StartCoroutine(LerpPosition(fadeInPosition, lerpDuration));
    }

    public void HealthUIFadeOut()
    {
        StartCoroutine(LerpPosition(fadeOutPosition, lerpDuration));
    }

    IEnumerator LerpPosition(Vector3 targetPosition, float duration)
    {
        float time = 0;
        Vector3 currentPosition = healthRecttransform.anchoredPosition3D;
        while (time<duration)
        {
            float t = time / duration;
            float lerpSmoothStep = t * t * (3f - 2f * t);
            healthRecttransform.anchoredPosition3D = Vector3.Lerp(currentPosition, targetPosition, lerpSmoothStep);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        healthRecttransform.anchoredPosition3D = targetPosition;
    }
    // update the UI based on the current health value
    void DisplayHealthUI(int health)
    {

        switch (health) {
            case 3:
                health1.enabled = true;
                health2.enabled = true;
                health3.enabled = true;
                break;
            case 2:
                health1.enabled = true;
                health2.enabled = true;
                health3.enabled = false;
                break;
            case 1:
                health1.enabled = true;
                health2.enabled = false;
                health3.enabled = false;
                break;
            case 0:
                health1.enabled = false;
                health2.enabled = false;
                health3.enabled = false;
                break;

        }
    }

}
