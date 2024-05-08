using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class SurvivalModeSceneController : Controller
{
    public const string SURVIVALMODESCENE_SCENE_NAME = "SurvivalModeScene";

    public override string SceneName()
    {
        return SURVIVALMODESCENE_SCENE_NAME;
    }

    private void Start()
    {
        Manager.Add(UISceneController.UISCENE_SCENE_NAME, true);
    }
}