using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DailyQuestItemDTO
{
    public QuestType indexQuest;
    public string title;
    public int finishNumber;
    public int currentNumber;
    public bool isClaim;
    public int rewardNumber;
    public EnemyType? indexEnemy;

    public DailyQuestItemDTO(QuestType indexQuest, string title, int finishNumber, int currentNumber, bool isClaim, int rewardNumber, EnemyType? indexEnemy)
    {
        this.indexQuest = indexQuest;
        this.title = title;
        this.finishNumber = finishNumber;
        this.currentNumber = currentNumber;
        this.isClaim = isClaim;
        this.rewardNumber = rewardNumber;
        this.indexEnemy = indexEnemy;
    }
}

[System.Serializable]
public class DailyQuestDTO
{
    public string timeReceivedDaily;
    public List<DailyQuestItemDTO> listDailyQuestItemDTO;
}
