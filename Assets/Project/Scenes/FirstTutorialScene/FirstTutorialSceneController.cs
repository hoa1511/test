using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class FirstTutorialSceneController : Controller
{
    public const string FIRSTTUTORIALSCENE_SCENE_NAME = "FirstTutorialScene";

    public override string SceneName()
    {
        return FIRSTTUTORIALSCENE_SCENE_NAME;
    }
    CharacterHolder characterHolder;
    
    private void Start()
    {
        characterHolder = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHolder>();
        characterHolder.isWaiting = true;
        
    }

    public override void OnHidden()
    {
        //base.OnHidden();
        Manager.Add(UISceneController.UISCENE_SCENE_NAME);
    }

    public void ReturnToGame()
    {
        SoundController.Instance.PlayButtonSound();
        
        GameManager.Instance.ResumeGame();
        
        PlayerPrefs.SetInt("hasPause", 0);
        StartCoroutine(normalPlayer());
        Manager.Close();
    }

    IEnumerator normalPlayer()
    {
        yield  return new WaitForSeconds(0.2f);
        characterHolder.isWaiting = false;
    }

    
}