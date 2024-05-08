using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class SM2Controller : Controller
{
    public const string SM2_SCENE_NAME = "SM2";

    public override string SceneName()
    {
        return SM2_SCENE_NAME;
    }
    private void Start()
    {
        Manager.Add(UISceneController.UISCENE_SCENE_NAME);
    }
}