using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrateManager : MonoBehaviour {

    public static Crate crate1;
    public static Crate crate2;

    [SerializeField]
    public int[] crate1Chance;
    public int[] crate2Chance;

	void Awake()
    {
        crate1 = new Crate("Crate 1");
        for(int i = 0 ; i < crate1Chance.Length; i++)
        {
            crate1.addReward(new CrateItem("crate" + i, i));
            crate1.setProbabilityOfLayer(i, crate1Chance[i]);
        }
        crate1.calculateAutomaticRewardProbability();

        crate2 = new Crate("Crate 2");
        for(int i = 0 ; i < crate2Chance.Length; i++)
        {
            crate2.addReward(new CrateItem("crate" + i, i));
            crate2.setProbabilityOfLayer(i, crate2Chance[i]);
        }
        crate2.calculateAutomaticRewardProbability();
    }
}
