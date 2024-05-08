using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatController : MonoBehaviour
{
    public PlayerSkin currentEquipedSkin;
    private PlayerWeapon currentEquipedWeapon;
    [SerializeField] private List<ActiveStats> activeStatList = new List<ActiveStats>();

    public PlayerSkin CurrentEquipedSkin {get => currentEquipedSkin; set => currentEquipedSkin = value;}
    public PlayerWeapon CurrentEquipedWeapon {get => currentEquipedWeapon; set => currentEquipedWeapon = value;}
    public List<ActiveStats> ActiveStatList {get => activeStatList;}

    public static CharacterStatController Instance;

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

    public float GetCurrentSkinStat(PassiveStats skinStat)
    {
        if(currentEquipedSkin.skinStats.Count > 0)
        {
            for(int i = 0; i < currentEquipedSkin.skinStats[currentEquipedSkin.skinLevel].passiveStats.Count; i++)
            {
                if(currentEquipedSkin.skinStats[currentEquipedSkin.skinLevel].passiveStats[i].skinStat == skinStat)
                {
                    return currentEquipedSkin.skinStats[currentEquipedSkin.skinLevel].passiveStats[i].statNumber;
                }
            }
        }
        
        return 0;
    }   


    public void AddActiveStatSkinToList(PlayerSkin playerSkin)
    {
        activeStatList.Clear();
        if(playerSkin.skinStats.Count > 0)
        {
            for(int i = 0; i < playerSkin.skinStats[playerSkin.skinLevel].activeStats.Count; i++)
            {
                activeStatList.Add(playerSkin.skinStats[playerSkin.skinLevel].activeStats[i].skinStat);
            }
        }
    }
}
