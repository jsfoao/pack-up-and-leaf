using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CutsceneScript : MonoBehaviour
{
    [SerializeField] float textAppearTime;
    [SerializeField] float sceneEndTime;
    [SerializeField] Image textImg;
    [SerializeField] Animator transitionAnim;
    IEnumerator CutSceneCoroutine()
    {
        yield return new WaitForSeconds(textAppearTime);
        textImg.enabled = true;
        yield return new WaitForSeconds(sceneEndTime);
        transitionAnim.Play("NewAnimIN");
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("Erik FINAL");
        
    }

    private void Start()
    {
        StartCoroutine(CutSceneCoroutine());
    }


}
