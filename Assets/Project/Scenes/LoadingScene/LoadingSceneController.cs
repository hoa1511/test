using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class LoadingSceneController : Controller
{
    public const string LOADINGSCENE_SCENE_NAME = "LoadingScene";

    public override string SceneName()
    {
        return LOADINGSCENE_SCENE_NAME;
    }
}