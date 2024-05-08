using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    public static SaveManager instance;

    public static SaveManager GetInstance()
    {
        if (instance is null)
        {
            instance = new SaveManager();
        }
        return instance;
    }

#region Daily Deal Items
    public void DailyDealItemsToJson(List<DailyDealItem> dailyDealItems)
    {
        List<DailyDealItemsDTO> listSkillFromArchives = new List<DailyDealItemsDTO>();

        for(int i = 0; i < dailyDealItems.Count; i++)
        {
         
            DailyDealItemsDTO dailyDealDTO = new DailyDealItemsDTO();
            dailyDealDTO.skinsIDs = (int)dailyDealItems[i].skinCard.skinsIDs;
            dailyDealDTO.cardQuantity = dailyDealItems[i].cardQuantity;
            dailyDealDTO.hasPurchase = dailyDealItems[i].hasPurchase;
            dailyDealDTO.amountOfCurrencyToUnlock = dailyDealItems[i].amountOfCurrencyToUnlock;
            listSkillFromArchives.Add(dailyDealDTO);

        }
        var savedData = new SaveData<DailyDealItemsDTO>(listSkillFromArchives);

        var playerJson = JsonUtility.ToJson(savedData, true);
        Debug.Log($"Skill Archive: {playerJson}");

        var filePath = Application.persistentDataPath + "/PlayerSkillArchiveData.json";
        Debug.Log(filePath);
        System.IO.File.WriteAllText(filePath, playerJson);
    }

    public void LoadPlayerSkillArchiveFromJson(SaveData<DailyDealItemsDTO> skillItem)
    {
        string filePath = Application.persistentDataPath + "/PlayerSkillArchiveData.json";
        if(System.IO.File.Exists(filePath))
        {
            string skillArchive = System.IO.File.ReadAllText(filePath);
            JsonUtility.FromJsonOverwrite(skillArchive ,skillItem);
            Debug.Log("Load Success");
        }
    }
#endregion

}
