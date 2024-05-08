using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;
using DG.Tweening;

public enum TypeRewardWeek
{
    Gold,
    Gem,
    Chest,
    SpecialChest
}

public class RewardWeeksController : Controller
{
    public const string REWARDWEEKS_SCENE_NAME = "RewardWeeks";

    private int day, nextDay;

    public int Day { get => day; set => day = value; }
    public int NextDay { get => nextDay; set => nextDay = value; }

    public Transform parent;

    [SerializeField] private GameObject prefabGold, prefabGem;
    [SerializeField] private AudioClip coinSound;

    private MenuSceneController menuSceneController;

    public override string SceneName()
    {
        return REWARDWEEKS_SCENE_NAME;
    }

    private void Awake()
    {
        day = PlayerPrefs.GetInt("rewardWeekDay") > 1 ? PlayerPrefs.GetInt("rewardWeekDay") : 1;
        nextDay = day + 1;

        menuSceneController = GameObject.FindGameObjectWithTag("MenuScene").GetComponent<MenuSceneController>();
    }

    public IEnumerator PlayAnimationGetReward(Transform spawnTransfrom, Currency currency, int number)
    {
        GameObject spawnImage = prefabGold;
        Transform endSpawn = menuSceneController.GoldText.transform.parent;
        int loopNum = number / 50;
        int depositPerLoop = 50;
        if(currency.Equals(Currency.Gem))
        {
            spawnImage = prefabGem;
            endSpawn = menuSceneController.GemText.transform.parent;
            loopNum = number / 5;
            depositPerLoop = 5;
        }

        int i = 0;
        while (i < loopNum)
        {
            GameObject spawnObject = Instantiate(spawnImage, spawnTransfrom.position, Quaternion.identity, parent);
            spawnObject.transform.localScale = new Vector3(1, 1, 1);
            number -= depositPerLoop;

            if (spawnObject != null)
            {
                spawnObject.transform.DOMove(endSpawn.position, 0.25f).OnComplete(() => {
                    spawnObject.GetComponent<AudioSource>().PlayOneShot(coinSound);
                    Bank.Instance.Deposit(depositPerLoop, currency);
                    Destroy(spawnObject);
                });
            }
            i++;
            yield return new WaitForSeconds(0.05f);
        }
        yield return new WaitForSeconds(0.05f);
    }

    public void Close()
    {
        Manager.Close();
    }

    public void OnClaimItemFinish(RewardWeekItem item)
    {
        item.Focus.SetActive(true);
        item.CheckMarkHolder.SetActive(false);
    }

}