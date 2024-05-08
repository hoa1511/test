using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrateItem
{
    //Id of crate item, ex: same id of your database item
    private string id;
    //Layer of reward on the crate
    private int rewardLevel;
    //Probability of the reward
    private float probability;

    /// <summary>
    /// Creates a new Crate reward
    /// </summary>
    /// <param name="id">id of the reward</param>
    /// <param name="rewardLevel">layer</param>
    public CrateItem(string id, int rewardLevel)
    {
        this.id = id;
        this.rewardLevel = rewardLevel;
        probability = 0;
    }

    /// <summary>
    /// Creates a new Crate reward
    /// </summary>
    /// <param name="id">id of the reward</param>
    /// <param name="rewardLevel">layer</param>
    /// <param name="probability">probability (0->100)</param>
    public CrateItem(string id, int rewardLevel, float probability)
    {
        this.id = id;
        this.rewardLevel = rewardLevel;
        this.probability = probability;
    }

    /// <summary>
    /// Returns the id of this reward
    /// </summary>
    /// <returns>ID</returns>
    public string getId()
    {
        return id;
    }

    /// <summary>
    /// Returns the layer of this item
    /// </summary>
    /// <returns>Layer</returns>
    public int getRewardLevel()
    {
        return rewardLevel;
    }

    /// <summary>
    /// Sets the layer of this item
    /// </summary>
    /// <param name="rewardLevel">Layer</param>
    public void setRewardLevel(int rewardLevel)
    {
        this.rewardLevel = rewardLevel;
    }

    /// <summary>
    /// Returns the probability of this item
    /// </summary>
    /// <returns>Probability</returns>
    public float getProbability()
    {
        return probability;
    }

    /// <summary>
    /// Sets a probability of a item
    /// </summary>
    /// <param name="probability">probability</param>
    public void setProbability(float probability)
    {
        if (probability < 0 || probability > 100)
        {
            this.probability = 0;
            Debug.LogError("Trying to set a invalid probability! Try it beetween 0 and 100! Probability set to 0!");
        }
        else
        {
            this.probability = probability;
        }
    }
}

