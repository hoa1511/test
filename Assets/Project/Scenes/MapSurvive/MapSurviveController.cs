using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class MapSurviveController : Controller
{
    public const string MAPSURVIVE_SCENE_NAME = "MapSurvive";

    public override string SceneName()
    {
        return MAPSURVIVE_SCENE_NAME;
    }

    private void Start() 
    {
        Manager.Add("UIScene");
    }
}