using SS.View;
// using Unity.Advertisement.IosSupport;
//using Unity.Advertisement.IosSupport;
using UnityEngine;

public class MainSceneController : Controller
{
    public const string MAINSCENE_SCENE_NAME = "MainScene";

    private void Start()
    {
#if UNITY_IOS
        if (ATTrackingStatusBinding.GetAuthorizationTrackingStatus() ==
            ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            ATTrackingStatusBinding.RequestAuthorizationTracking();
        }
#endif
    }
    public override string SceneName()
    {
        return MAINSCENE_SCENE_NAME;
    }
}