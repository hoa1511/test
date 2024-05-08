using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class QuestManager : MonoBehaviour
{
    public List<DailyQuestItemDTO> listDailyQuestItemDTO;

    [SerializeField] private DailyQuestController dailyQuestController;
    [SerializeField] private AchievementController achievementController;
    [SerializeField] private GameObject gemSpawnPrefab;
    [SerializeField] private AudioClip gemSound;
    private Transform dailyQuestView, achievementView;
    private Transform posMoveGold, posMoveGem, parent;

    public static QuestManager Instance;

    public DailyQuestController DailyQuestController { get => dailyQuestController; }
    public AchievementController AchievementController { get => achievementController; }

    public Transform PosMoveGold { get => posMoveGold; set => posMoveGold = value; }
    public Transform PosMoveGem { get => posMoveGem; set => posMoveGem = value; }
    public Transform Parent { get => parent; set => parent = value; }
    public Transform DailyQuestView { get => dailyQuestView; set => dailyQuestView = value; }
    public Transform AchievementView { get => achievementView; set => achievementView = value; }
    
    private void Awake()
    {
        if(!Instance)
        {
            Instance = this;
        }
    }

    public void FechDailyQuest()
    {
        foreach (Transform child in dailyQuestView.transform)
        {
            Destroy(child.gameObject);
        }

        DailyQuestController.LoadQuestFromJsonToView(dailyQuestView);
    }

    public void FetchAchievement()
    {
        foreach (Transform child in achievementView.transform)
        {
            Destroy(child.gameObject);
        }

        AchievementController.LoadAchievementToView(achievementView);
    }

    private void Start()
    {
        dailyQuestController.CheckDailyQuest();

        listDailyQuestItemDTO = dailyQuestController.LoadQuestFromJsonToDTO();

        achievementController.LoadProgressFromJsonToDictionary();

        // FetchAchievement();
        // FechDailyQuest();
    }


    public IEnumerator StartSpawnGemImage(int number, Vector3 startPos, QuestType questType)
    {
        
        int loopNum = number / 5;
        int i = 0;
        while(i < loopNum)
        {
            GameObject gem = Instantiate(gemSpawnPrefab, startPos, Quaternion.identity, parent);
            gem.transform.localScale = new Vector3(1, 1, 1);
            number -= 5;
            if (gem != null)
            {
                gem.transform.DOMove(posMoveGem.position, 0.25f).OnComplete(() => {
                    gem.GetComponent<AudioSource>().PlayOneShot(gemSound);
                    Bank.Instance.Deposit(5, Currency.Gem);
                    Destroy(gem);
                }).Play().SetUpdate(true);
            }
            i++;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        DailyQuestController.OnClaimDailyQuest(questType);
        FechDailyQuest();
        yield return new WaitForSecondsRealtime(0.05f);
    }

    public IEnumerator StartSpawnGemImage(int number, Vector3 startPos, int index)
    {

        int loopNum = number / 5;
        int i = 0;
        while (i < loopNum)
        {
            GameObject gem = Instantiate(gemSpawnPrefab, startPos, Quaternion.identity, parent);
            gem.transform.localScale = new Vector3(1, 1, 1);
            number -= 5;
            if (gem != null)
            {
                gem.transform.DOMove(posMoveGem.position, 0.25f).OnComplete(() => {
                    gem.GetComponent<AudioSource>().PlayOneShot(gemSound);
                    Bank.Instance.Deposit(5, Currency.Gem);
                    Destroy(gem);
                }).Play().SetUpdate(true);
            }
            i++;
            yield return new WaitForSecondsRealtime(0.05f);
        }
        AchievementController.OnClaimReward(index);
        FetchAchievement();
        yield return new WaitForSecondsRealtime(0.05f);
    }
}
