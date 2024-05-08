using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.UI;
public class StatusTextController : MonoBehaviour
{
    [SerializeField] private GameObject statusTextPrefab;
    public static StatusTextController Instance;

    [Header("Status Sprites")]
    public Sprite missSprite;
    public Sprite electricShockSprite;
    public Sprite headShotSprite;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }   
    }

#region "Display Status Text"
    public void InstantiateStatusText(string textToSpawn, Sprite statusImage, Vector3 positionToSpawn, Color textColor, float yOffset)
    {
        Vector3 instantiateScale = new Vector3(0.1f, 0.1f, 0.1f);
        Vector3 instantiateOffset = new Vector3(0.5f, 0.8f, 0);

        GameObject spawnStatusText = Instantiate(statusTextPrefab);

        spawnStatusText.transform.localScale = instantiateScale;
        spawnStatusText.transform.position = positionToSpawn + instantiateOffset;
        spawnStatusText.transform.rotation = Quaternion.Euler(0, 180, 0);

        spawnStatusText.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().text = textToSpawn;
        spawnStatusText.gameObject.transform.GetChild(0).GetComponent<TextMeshPro>().color = textColor;

        spawnStatusText.gameObject.transform.GetChild(1).GetChild(0).GetComponent<Image>().sprite = statusImage;

        spawnStatusText.transform.DOMoveY(spawnStatusText.transform.position.y + yOffset, 0.5f).OnComplete(()=>{
            Destroy(spawnStatusText.gameObject);
        });
    }
#endregion

}
