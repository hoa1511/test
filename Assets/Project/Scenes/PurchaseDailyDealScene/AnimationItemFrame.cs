using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationItemFrame : MonoBehaviour
{
    [SerializeField] private PurchaseDailyDealSceneController purchaseDailyDealScene;
    [SerializeField] private Animator claimAnimator;

    private void Start()
    {
        AnimationClip clip = claimAnimator.runtimeAnimatorController.animationClips[0];

        AnimationEvent animEvent = new AnimationEvent();
        animEvent.time = clip.length;
        animEvent.functionName = "OnPurchaseSuccess";

        clip.AddEvent(animEvent);

        claimAnimator.Play(0);
    }

    private void OnPurchaseSuccess()
    {
        purchaseDailyDealScene.OnPurchaseSuccess();
    }

}
