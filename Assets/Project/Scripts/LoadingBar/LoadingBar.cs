using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using SS.View;
// using Unity.Advertisement.IosSupport;

//using Unity.Advertisement.IosSupport;

public class LoadingBar : MonoBehaviour
{
    Slider slider;
    public bool isCompleteLoading;
    [SerializeField] TextMeshProUGUI percentText;
    [SerializeField] TextMeshProUGUI loadingText;
    private void Awake()
    {
        slider = GetComponent<Slider>();
    }
    void Start()
    {

    }


    void Update()
    {


    }

    public void StartUpdateLoadingBar(int level)
    {
        StartCoroutine(UpdateLoadingBar(level));
    }

    IEnumerator UpdateLoadingBar(int level)
    {
        for (int i = 0; i <= 100; i++)
        {
            slider.value = i * 0.01f;
            percentText.text = i.ToString() + "%";
            yield return new WaitForSeconds(0.001f);
        }
        CompletedLoading();
        yield return new WaitForSeconds(0.3f);

#if UNITY_IOS && !UNITY_EDITOR
        var status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();

        while (status == ATTrackingStatusBinding.AuthorizationTrackingStatus.NOT_DETERMINED)
        {
            status = ATTrackingStatusBinding.GetAuthorizationTrackingStatus();
            yield return null;
        }
#endif

        Manager.Load("MenuScene");
        // Manager.LoadingAnimation(true);
    }

    void CompletedLoading()
    {
        loadingText.text = "Completed!";
    }
}

