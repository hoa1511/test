using UnityEngine;

[CreateAssetMenu(fileName = "PlayerWeapon", menuName = "Player/PlayerWeapon", order = 0)]
public class PlayerWeapon : Item 
{
    public WeaponIDs weaponId;
    public int weaponSkinIndex;
 
    public Color weaponColor;
    public Material weaponMaterial;
    public ParticleSystem weaponTrailEffect;
    
    public int levelToUnlock;
    public bool isOwned;
}
