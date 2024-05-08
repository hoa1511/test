using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class LoseGameSceneController : Controller
{
    public const string LOSEGAMESCENE_SCENE_NAME = "LoseGameScene";

    public override string SceneName()
    {
        return LOSEGAMESCENE_SCENE_NAME;
    }

    public void RePlayCheckpoint()
    {
        SoundController.Instance.PlayButtonSound();
        
        GameManager.Instance.ReLoadCheckPoint();
    }
}