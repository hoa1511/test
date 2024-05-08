using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class SettingSceneController : Controller
{
    public const string SETTINGSCENE_SCENE_NAME = "SettingScene";
    CharacterHolder characterHolder;

    private void Start()
    {
        // if(GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHolder>() is not null)
        // {
        //     characterHolder = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterHolder>();
        // }
        // else
        // {
        //     characterHolder = null;
        // }
    }

    public override string SceneName()
    {
        return SETTINGSCENE_SCENE_NAME;
    }

    public void Back()
    {
        GameManager.Instance.ResumeGame();
        StartCoroutine(normalPlayer());
        Manager.Close();
    }

    IEnumerator normalPlayer()
    {
        if(characterHolder is not null)
        {
            yield  return new WaitForSeconds(0.2f);
            characterHolder.isWaiting = false;
        }
    }
}