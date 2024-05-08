using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SS.View;
using System.Linq;
using System.IO;
using System;

public enum QuestType { 
    Kill,
    Clear,
    Destroy
}


public class DailyQuestController : MonoBehaviour
{
    [SerializeField] private GameObject dailyQuestItemPrefab;
    [SerializeField] private int totalQuest;
    [SerializeField] private bool isDontDestroy;
    [SerializeField] private Sprite iconKill, iconClear, iconDestroy;

    private QuestType[] listQuestType = { QuestType.Kill, QuestType.Clear, QuestType.Destroy };
    private List<DailyQuestItem> listDailyQuest;
        
    // private string receivedDailyTime = "";

    private DailyQuestDTO gameplayDailyQuestDTO;

    private string dataPath = "";

    // fake data
    private int maxLevel = 0;

    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/DailyQuest.json";
        listDailyQuest = new List<DailyQuestItem>();
    }

    private List<DailyQuestItemDTO> RandomQuest()
    {
        List<DailyQuestItemDTO> dailyQuestItemDTOs = new List<DailyQuestItemDTO>();
        for (int i = 0; i < listQuestType.Length; i++)
        {
            DailyQuestItemDTO newDTO = null;
            switch (listQuestType[i])
            {
                case QuestType.Kill:
                    newDTO = new DailyQuestItemDTO(QuestType.Kill, "KILL ANY BOSS", 1, 0, false, 10, EnemyType.Boss);
                    break;
                case QuestType.Clear:
                    newDTO = new DailyQuestItemDTO(QuestType.Clear, "CLEAR 10 LEVEL", 10, 0, false, 15, null);
                    break;
                case QuestType.Destroy:
                    newDTO = new DailyQuestItemDTO(QuestType.Destroy, "DESTROY 05 CHESTS ON BATTLE", 5, 0, false, 10, null);
                    break;
            }
            dailyQuestItemDTOs.Add(newDTO);
        }
        SaveQuestToJson(dailyQuestItemDTOs);
        return dailyQuestItemDTOs;
    }

    private void SaveQuestToJson(List<DailyQuestItemDTO> dailyQuestItemDTOs)
    {
        DailyQuestDTO dailyQuestDTO = new DailyQuestDTO();

        dailyQuestDTO.timeReceivedDaily = DateTime.Now.ToString();
        gameplayDailyQuestDTO.timeReceivedDaily = dailyQuestDTO.timeReceivedDaily;
        dailyQuestDTO.listDailyQuestItemDTO = dailyQuestItemDTOs;

        var dailyQuestJSON = JsonUtility.ToJson(dailyQuestDTO, true);
        Debug.Log($"dailyquest json: {dailyQuestJSON}");
      
        File.WriteAllText(dataPath, dailyQuestJSON);
    }

    public void CheckDailyQuest()
    {
        gameplayDailyQuestDTO = new DailyQuestDTO();
        if(File.Exists(dataPath))
        {
            string dailyJSON = File.ReadAllText(dataPath);
            DailyQuestDTO dailyQuestDTO = JsonUtility.FromJson<DailyQuestDTO>(dailyJSON);
            gameplayDailyQuestDTO.timeReceivedDaily = dailyQuestDTO.timeReceivedDaily;
            DateTime currentTime = DateTime.Now;
            DateTime receivedTime = DateTime.Parse(dailyQuestDTO.timeReceivedDaily);
            DateTime tomorrowLastTime = receivedTime.AddDays(1);
            if (currentTime.Day == tomorrowLastTime.Day)
            {
                // reset daily at 8 AM
                if (currentTime.TimeOfDay.Hours >= 8)
                {
                    RandomQuest();
                    return;
                }
            } else if(currentTime.Day > tomorrowLastTime.Day)
            {
                RandomQuest();
                return;
            }
        } else
        {
            RandomQuest();
        }
    }

    public List<DailyQuestItem> LoadQuestFromJsonToView(Transform dailyQuestView)
    {
        if (File.Exists(dataPath))
        {
            string dailyJSON = File.ReadAllText(dataPath);
            DailyQuestDTO dailyQuestDTO = JsonUtility.FromJson<DailyQuestDTO>(dailyJSON);
            dailyQuestDTO.listDailyQuestItemDTO.ForEach(value =>
            {
                GameObject newQuest = Instantiate(dailyQuestItemPrefab, dailyQuestView);
                DailyQuestItem item = newQuest.GetComponent<DailyQuestItem>();
                Sprite icon = null;
                switch(value.indexQuest)
                {
                    case QuestType.Kill:
                        icon = iconKill;
                        break;
                    case QuestType.Clear:
                        icon = iconClear;
                        break;
                    case QuestType.Destroy:
                        icon = iconDestroy;
                        break;
                }
                item.Setup(value.indexQuest, value.finishNumber, value.rewardNumber, value.title, value.isClaim, value.currentNumber, icon);
                listDailyQuest.Add(item);
            });
            return listDailyQuest;
        }
        return null;
    }


    public List<DailyQuestItemDTO> LoadQuestFromJsonToDTO()
    {
        if (File.Exists(dataPath))
        {
            string dailyJSON = File.ReadAllText(dataPath);
            DailyQuestDTO dailyQuestDTO = JsonUtility.FromJson<DailyQuestDTO>(dailyJSON);
            return dailyQuestDTO.listDailyQuestItemDTO; ;
        }
        return null;
    }

    public void DoDailyQuest(QuestType questType)
    {
        DailyQuestItemDTO dailyQuestItemDTO = null;

        QuestManager.Instance.listDailyQuestItemDTO.ForEach((value) =>
        {
            if (value.indexQuest.Equals(questType))
            {
                dailyQuestItemDTO = value;
            }
        });

        if (dailyQuestItemDTO != null)
        {
            dailyQuestItemDTO.currentNumber += 1;
            if (dailyQuestItemDTO.currentNumber > dailyQuestItemDTO.finishNumber)
            {
                dailyQuestItemDTO.currentNumber = dailyQuestItemDTO.finishNumber;
            }
            else SaveProgressToJson(gameplayDailyQuestDTO);
        }
    }


    private void SaveProgressToJson(DailyQuestDTO gameplayDailyQuestDTO)
    {
        gameplayDailyQuestDTO.listDailyQuestItemDTO = QuestManager.Instance.listDailyQuestItemDTO;
        var dailyProgressQuestJSON = JsonUtility.ToJson(gameplayDailyQuestDTO, true);
        File.WriteAllText(dataPath, dailyProgressQuestJSON);
    }

    public void OnClaimDailyQuest(QuestType questType)
    {
        QuestManager.Instance.listDailyQuestItemDTO.ForEach((value) =>
        {
            if (value.indexQuest.Equals(questType))
            {
                value.isClaim = true;
            }
        });
        SaveProgressToJson(gameplayDailyQuestDTO);
    }

}
