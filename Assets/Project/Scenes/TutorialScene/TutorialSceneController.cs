using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class TutorialSceneController : Controller
{
    public const string TUTORIALSCENE_SCENE_NAME = "TutorialScene";
    CharacterHolder characterHolder;

    public override string SceneName()
    {
        return TUTORIALSCENE_SCENE_NAME;
    }

    private void Start()
    {
        characterHolder = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHolder>();
        characterHolder.isWaiting = true;
        
    }

    public void ReturnToGame()
    {
        SoundController.Instance.PlayButtonSound();
        Manager.Close();
    }

    public override void OnHidden()
    {
        if(GameManager.Instance.isWaiting == true)
        {
            //isPauseGame = false;
            PlayerPrefs.SetInt("hasPause", 0);
        }
        else if(GameManager.Instance.isWaiting == false)
        {
            GameManager.Instance.ResumeGame();
            //PlayerPrefs.SetInt("hasPause", 0);
            characterHolder.isWaiting = false;
        }
    }


    IEnumerator normalPlayer()
    {
        yield  return new WaitForSeconds(0.2f);
        characterHolder.isWaiting = false;
    }
}