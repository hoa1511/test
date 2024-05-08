using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using SS.View;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float min;
    [SerializeField] private Transform weapon;
    [SerializeField] private Transform top;

    private bool canMove = false;
    private Vector3 firstTransform = Vector3.zero;

    private void Start()
    {
        // firstTransform = transform.position;
        // GameManager.Instance.PauseTimeScale();
        // transform.DOMoveY(firstTransform.y, 1.25f).OnComplete(() =>
        // {
        //    transform.DOMoveY(max, 1.25f).OnComplete(() =>
        //    {
        //        transform.DOMoveY(firstTransform.y, 1.25f).OnComplete(() =>
        //        {
        //            //GameManager.Instance.NormalTimeScale();
        //            Manager.Add(UISceneController.UISCENE_SCENE_NAME);
        //            canMove = true;
        //             GameManager.Instance.PauseTimeScale();
        //        }).SetUpdate(true).Play();
        //    }).SetUpdate(true).Play();
        // }).SetUpdate(true).Play();
    }

    void LateUpdate()
    {
        if (weapon)
        {
            transform.position = new Vector3(transform.position.x, Mathf.Clamp(weapon.position.y, min, top.position.y -5), transform.position.z);
        }
    }
}
