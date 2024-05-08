using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddGemsButton : MonoBehaviour
{
    [SerializeField] private GameObject scrollRectGameObject;
    [SerializeField] private Button addGemButton;

    void Start()
    {
        SetUpButton();
    }

    void Update()
    {
        
    }
    private void ScrollToGemPack()
    {
        scrollRectGameObject.GetComponent<ScrollRect>().verticalNormalizedPosition = 0;
    }

    private void SetUpButton()
    {
        addGemButton.onClick.AddListener(delegate{ScrollToGemPack();});
    }
}
