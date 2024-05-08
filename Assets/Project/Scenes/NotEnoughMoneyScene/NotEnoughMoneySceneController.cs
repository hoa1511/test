using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;
using UnityEngine.UI;

public class NotEnoughMoneySceneController : Controller
{
    [SerializeField] private Button closeButton;
    public const string NOTENOUGHMONEYSCENE_SCENE_NAME = "NotEnoughMoneyScene";

    private void Start()
    {
        closeButton.onClick.AddListener(delegate{Manager.Close();});
    }

    public override string SceneName()
    {
        return NOTENOUGHMONEYSCENE_SCENE_NAME;
    }

    public void Close()
    {
        Manager.Close();
    }
}