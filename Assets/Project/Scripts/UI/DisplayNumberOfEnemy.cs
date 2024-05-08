using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayNumberOfEnemy : MonoBehaviour
{
    [SerializeField] private Transform numberOfEnemyCountUIHolder;
    [SerializeField] private GameObject numebrOfEnemyCountPrefab;
    [SerializeField] private GameObject numebrOfBossCountPrefab;
    [SerializeField] private Image numberOfEnemyBackGround;
    [SerializeField] private GridLayoutGroup gridLayoutGroup;

    // [SerializeField] AdsInitializerEndScene adsInitializerEndScene;
    // [SerializeField] InterstitialAdsButton interstitialAdsButton;

    private List<GameObject> numberOfEnemyUI = new List<GameObject>();

    private GameObject player;
    public GameObject[] enemies;
    public GameObject[] normalEnemies;
    public GameObject[] bossEnemies;

    public int currentEnemyCount;
    public int currentEnemyCountCheckKey;
    public int numberOfEnemy;

    GameManager gameManager;
    Bank bank;

    private void Start()
    {
        gameManager = GameManager.Instance;
        bank = Bank.Instance;

        player =  GameObject.FindGameObjectWithTag("Player");
        normalEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        bossEnemies = GameObject.FindGameObjectsWithTag("Boss");
        

        for(int i = 0; i < normalEnemies.Length; i++)
        {
            GameObject enemyCountUI =  Instantiate(numebrOfEnemyCountPrefab,numberOfEnemyCountUIHolder);
            numberOfEnemyUI.Add(enemyCountUI);
        }

        for(int i = 0; i < bossEnemies.Length; i++)
        {
            GameObject enemyBossCountUI =  Instantiate(numebrOfBossCountPrefab,numberOfEnemyCountUIHolder);
            numberOfEnemyUI.Add(enemyBossCountUI);
        }

        numberOfEnemy = normalEnemies.Length;
        PlayerPrefs.SetInt("NumberOfEnemy", numberOfEnemy);
        
        currentEnemyCount = numberOfEnemyUI.Count;
        currentEnemyCountCheckKey = currentEnemyCount;
        numberOfEnemyBackGround.rectTransform.sizeDelta = new Vector2(gridLayoutGroup.cellSize.x * (numberOfEnemyUI.Count + 1) + 40, numberOfEnemyBackGround.rectTransform.rect.height);

    }

    private void InstantiateEnemyCountUI()
    {
        for(int i = 0; i < currentEnemyCount; i++)
        {
            GameObject enemyCountUI =  Instantiate(numebrOfEnemyCountPrefab,numberOfEnemyCountUIHolder);
            numberOfEnemyUI.Add(enemyCountUI);
        }
    }

    public void UpdateNumberOfEnemyUI()
    {
        numberOfEnemyUI[currentEnemyCount - 1].transform.GetChild(0).gameObject.SetActive(true);

        currentEnemyCount -= 1;
        if(currentEnemyCount == 0 && gameManager.hasLoose == false)
        {
            player.GetComponent<Collider>().enabled = false;
            
            gameManager.hasWon = true;

            PlayerPrefs.SetInt("hasPause", 1);

            if(GameManager.Instance.hasReachFinalSkin == false)
            {
               
                if(PlayerPrefs.GetInt("hasPlay30Round") == 0)
                {
                    StartCoroutine(gameManager.DisPlayWinUI(10));
                }
                else if(PlayerPrefs.GetInt("hasPlay30Round") == 1)
                {
                    StartCoroutine(gameManager.DisPlayWinUI(5));
                }
            }

            else if(GameManager.Instance.hasReachFinalSkin == true)
            {
                StartCoroutine(gameManager.DisPlayWinUI(4));
            }
            
        }
    }

}
