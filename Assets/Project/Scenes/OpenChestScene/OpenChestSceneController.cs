using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;

public class OpenChestSceneController : Controller
{
    public const string OPENCHESTSCENE_SCENE_NAME = "OpenChestScene";

    public override string SceneName()
    {
        return OPENCHESTSCENE_SCENE_NAME;
    }

    public void Close()
    {
        Manager.Close();
    }
}