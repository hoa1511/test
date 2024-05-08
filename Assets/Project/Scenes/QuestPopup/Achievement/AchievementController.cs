using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Linq;

public enum AchievementType
{
    KillBoss,
    SpendGold,
    UnlockSkin,
    UnlockTrident,
    KillEnemy,
    ReachLevel,
    KillArcher,
    KillGuard,
    KillShark,
    KillSword,
    KillFireMan
}

[System.Serializable]
public class AchievementDTO
{
    public Sprite icon;
    public AchievementType achievementType;
    public int index;
    public int[] progressionComplete;
    public int[] progressionReward;
    public EnemyType enemyType;
    public int currentProgress;
    public int totalNumber;
    public string prefix;
    public string suffixes;

    public bool isNumberLast;

    public string Title(int currentProgress)
    {
        if (currentProgress >= progressionComplete.Length) currentProgress = progressionComplete.Length - 1;
        if(isNumberLast)
        {
            return string.Format("{0} {1} {2}", prefix, suffixes, progressionComplete[currentProgress]);
        }
        return string.Format("{0} {1} {2}", prefix, progressionComplete[currentProgress], suffixes);
    }

    public int GetLength()
    {
        return progressionComplete.Length;
    }
}

[System.Serializable]
class ProgressDTO
{
    public int index;
    public int currentProgress;
    public int totalNumber;

    public ProgressDTO(int index, int currentProgress, int totalNumber)
    {
        this.index = index;
        this.currentProgress = currentProgress;
        this.totalNumber = totalNumber;
    }
}

[System.Serializable]
class SaveProgress
{
    public List<ProgressDTO> listProgressDTO;

    public int GetCurrentProgress(int index)
    {
        return listProgressDTO[index].currentProgress;
    }
}


public class AchievementController : MonoBehaviour
{
    [SerializeField] private AchievementDTO[] achievementDTOs;
    [SerializeField] private GameObject achievementItem;
    [SerializeField] private List<ProgressDTO> listProgressAchievement;
    [SerializeField] private Dictionary<int, AchievementDTO> dictionaryProgress;

    private string dataPath = "";

    private void Awake()
    {
        dataPath = Application.persistentDataPath + "/AchievementProgress.json";
    }


    public void LoadProgressFromJsonToDictionary()
    {
        dictionaryProgress = new Dictionary<int, AchievementDTO>();
        foreach(AchievementDTO dto in achievementDTOs)
        {
            dictionaryProgress.Add(dto.index, dto);
        }
        listProgressAchievement = new List<ProgressDTO>();
        if (File.Exists(dataPath))
        {
            string dailyJSON = File.ReadAllText(dataPath);
            listProgressAchievement = JsonUtility.FromJson<SaveProgress>(dailyJSON).listProgressDTO;
            if(listProgressAchievement.Count > 0)
            {
                listProgressAchievement.ForEach((value) =>
                {
                    dictionaryProgress[value.index].currentProgress = value.currentProgress;
                    dictionaryProgress[value.index].totalNumber = value.totalNumber;
                });
            } else
            {
                SaveAchievementToJSON();
            }
        } else
        {
            SaveAchievementToJSON();
        }
    }

    public void LoadAchievementToView(Transform view)
    {
        foreach(AchievementDTO dto in achievementDTOs)
        {
            GameObject achievement = Instantiate(achievementItem, view);
            AchievementItem item = achievement.GetComponent<AchievementItem>();
            int tempCurrentProgress = dictionaryProgress[dto.index].currentProgress >= dto.GetLength() ? (dto.GetLength() - 1) : dictionaryProgress[dto.index].currentProgress;
            item.SetupItem(dto.Title(dictionaryProgress[dto.index].currentProgress), dictionaryProgress[dto.index].totalNumber, dto.progressionComplete[tempCurrentProgress], dto.progressionReward[tempCurrentProgress], dto.icon, dto.index, dto.GetLength(), dictionaryProgress[dto.index].currentProgress);
        }
    }

    public void OnClaimReward(int index)
    {
        AchievementDTO achievementDTO = dictionaryProgress[index];
        if (achievementDTO.achievementType.Equals(AchievementType.ReachLevel))
        {
            achievementDTO.totalNumber = PlayerPrefs.GetInt("displayLevel");
        }
        else achievementDTO.totalNumber = 0;
        achievementDTO.currentProgress += 1;

        SaveAchievementToJSON();
    }

    public void DoAchievementReachLevel(int level)
    {
        foreach (AchievementDTO achievementDTO in achievementDTOs)
        {
            if (achievementDTO.achievementType.Equals(AchievementType.ReachLevel))
            {
                dictionaryProgress[achievementDTO.index].totalNumber = level;
                SaveAchievementToJSON();
            }
        }
    }

    public void DoAchievementSpendMoney(int amount)
    {
        foreach (AchievementDTO achievementDTO in achievementDTOs)
        {
            if (achievementDTO.achievementType.Equals(AchievementType.SpendGold))
            {
                dictionaryProgress[achievementDTO.index].totalNumber += amount;
                SaveAchievementToJSON();
            }
        }
    }

    public void DoAchievementUnlock(AchievementType achievementType)
    {
        foreach (AchievementDTO achievementDTO in achievementDTOs)
        {
            if (achievementDTO.achievementType.Equals(achievementType))
            {
                dictionaryProgress[achievementDTO.index].totalNumber += 1;
                SaveAchievementToJSON();
            }
        }
    }

    public void DoAchievementKill(EnemyType enemyType)
    {
        AchievementType achievementType = AchievementType.KillEnemy;

        switch(enemyType)
        {
            case EnemyType.Archer:
                achievementType = AchievementType.KillArcher;
                break;
            case EnemyType.Fireman:
                achievementType = AchievementType.KillFireMan;
                break;
            case EnemyType.Guard:
                achievementType = AchievementType.KillGuard;
                break;
            case EnemyType.Shark:
                achievementType = AchievementType.KillShark;
                break;
            case EnemyType.Sword:
                achievementType = AchievementType.KillSword;
                break;
            case EnemyType.Boss:
                achievementType = AchievementType.KillBoss;
                break;
        }

        foreach(AchievementDTO achievementDTO in achievementDTOs)
        {
            if(achievementDTO.achievementType.Equals(achievementType) || achievementDTO.achievementType.Equals(AchievementType.KillEnemy))
            {
                dictionaryProgress[achievementDTO.index].totalNumber += 1;
                SaveAchievementToJSON();
            }
        }
    }


    private void SaveAchievementToJSON()
    {
        listProgressAchievement.Clear();
        foreach (AchievementDTO item in achievementDTOs)
        {
            listProgressAchievement.Add(new ProgressDTO(item.index, dictionaryProgress[item.index].currentProgress, dictionaryProgress[item.index].totalNumber));
        }

        SaveProgress saveProgress = new SaveProgress();
        saveProgress.listProgressDTO = listProgressAchievement;

        var progressJSON = JsonUtility.ToJson(saveProgress, true);
        File.WriteAllText(dataPath, progressJSON);
    }
}
