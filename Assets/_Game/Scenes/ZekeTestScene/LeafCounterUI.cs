using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class LeafCounterUI : MonoBehaviour
{
    [SerializeField] float maxSecond;
    [SerializeField] float existSecond;
    [SerializeField] float timeTillVanish;
    float originX;

    [SerializeField] IntVar leafAmount;
    [SerializeField] private IntVar berryAmount;

    [Header("Animation")] //UI animation set up starts here.
    [SerializeField] Vector3 fadeOutPosition; //the recttransform position when UI faded out
    [SerializeField] Vector3 fadeInPosition; //the recttransform position when UI faded in
    [SerializeField] float lerpDuration = 0.8f;
    private RectTransform leafRecttransform;

    [SerializeField] private Text berryText_1;
    [SerializeField] private Text berryText_2;
    

    // Start is called before the first frame update
    void Start()
    {
        existSecond = timeTillVanish;
        //originX = transform.position.x;
        leafRecttransform = gameObject.GetComponent<RectTransform>();
        leafRecttransform.anchoredPosition3D = fadeInPosition;
    }

    // Update is called once per frame
    void Update()
    {

        // Display the number based on the number of the leaf
        if (leafAmount.Value >= 10)
        {
            GameObject.Find("LeafNumber").GetComponent<Text>().text = leafAmount.Value.ToString();
        } else
        {
            GameObject.Find("LeafNumber").GetComponent<Text>().text = "0"+ leafAmount.Value.ToString();
        }
        
        berryText_1.text = berryAmount.Value.ToString();
        berryText_2.text = berryAmount.Value.ToString();

        if (Input.GetKeyDown(KeyCode.K))
        {
            ShowLeafCounter();
        }
    }

    IEnumerator LerpToPosition(float duration)
    {
        float time = 0;
        Vector3 currentPosition = leafRecttransform.anchoredPosition3D;
        while(time<duration)
        {
            float t = time / duration;
            float lerpSmoothStep = t * t * (3f - 2f * t);
            leafRecttransform.anchoredPosition3D = Vector3.Lerp(currentPosition, fadeInPosition, lerpSmoothStep);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        leafRecttransform.anchoredPosition3D = fadeInPosition;
        existSecond = timeTillVanish;
        while(existSecond>0)
        {
            existSecond -= Time.unscaledDeltaTime;
            time = 0;
            currentPosition = leafRecttransform.anchoredPosition3D;
            yield return null;
        }
        while (time < duration)
        {
            float t = time / duration;
            float lerpSmoothStep = t * t * (3f - 2f * t);
            leafRecttransform.anchoredPosition3D = Vector3.Lerp(currentPosition, fadeOutPosition, lerpSmoothStep);
            time += Time.unscaledDeltaTime;
            yield return null;
        }
        leafRecttransform.anchoredPosition3D = fadeOutPosition;
    }

    public void UpdateText()
    {
        leafAmount.Value += 1;
        ShowLeafCounter();
    }

    public void UpdateBerryCount()
    {
        berryAmount.Value += 1;
        Debug.Log("Berries: " + berryAmount.Value);
    }

    public void ShowLeafCounter()
    {
        existSecond = timeTillVanish;
        StartCoroutine(LerpToPosition(lerpDuration));
    }
}
