using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crate {

    //Name of the crate to indentify it on debug methods
    private string name;

    //Dictionary of the reward in this crate by id(string)
    private Dictionary<string, CrateItem> rewards;
    //Each layer of the crate with it probabilty to set on automatic calculation
    private Dictionary<int, float> probabilities;

    /// <summary>
    /// Creates a new crate
    /// </summary>
    /// <param name="name">Crate name</param>
	public Crate(string name)
    {
        this.name = name;
        rewards = new Dictionary<string, CrateItem>();
        probabilities = new Dictionary<int, float>();
    }

    /// <summary>
    /// Debug the crate with each reward probability
    /// </summary>
    public void DebugCrate()
    {
        string log = "";
        log += "         -------- CRATE DEBUG --------\n";
        log += "Name: " + name + "\n";

        foreach (string id in rewards.Keys)
        {
            CrateItem item;
            rewards.TryGetValue(id, out item);
            log += "Item: " + item.getId() + ", RewardLevel: " + item.getRewardLevel() + ", Probability: " + item.getProbability() + "%\n";
        }

        log += "Reward: " + getReward().getId() + "\n";
        log += "         ----------------------------------\n";
        Debug.Log(log);
    }

    /// <summary>
    /// Gets you the reward from it probabilities
    /// </summary>
    /// <returns>Crate Item class with the information of the reward</returns>
    public CrateItem getReward()
    {
        float totalProb = 0;

        foreach (string id in rewards.Keys)
        {
            CrateItem item;
            rewards.TryGetValue(id, out item);

            totalProb += item.getProbability();
        }

        if (totalProb > 100)
        {
            Debug.LogError("Probabilities setted on crate " + name + " are too big! Try to use calculateAutomaticRewardProbability() or set it manual!");
            return null;
        }

        if (totalProb < 100)
        {
            Debug.LogError("Probabilities setted on crate " + name + " are too low! Try to use calculateAutomaticRewardProbability() or set it manual!");
            return null;
        }

        int random = Random.Range(1, 101);

        totalProb = 0;

        foreach (string id in rewards.Keys)
        {
            CrateItem item;
            rewards.TryGetValue(id, out item);

            totalProb += item.getProbability();

            if (totalProb >= random)
            {
                return item;
            }
        }

        return null;
    }

    /// <summary>
    /// Adds a new reward to the rewards list
    /// </summary>
    /// <param name="item">Crate item to add as a new reward</param>
    /// <returns>If it added or not</returns>
    public bool addReward(CrateItem item)
    {
        if (!rewards.ContainsKey(item.getId()))
        {
            rewards.Add(item.getId(), item);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Remove a Crate reward from rewards
    /// </summary>
    /// <param name="item">Crate class to be removed</param>
    /// <returns>If removed or not</returns>
    public bool removeReward(CrateItem item)
    {
        if (rewards.ContainsKey(item.getId()))
        {
            rewards.Remove(item.getId());
            return true;
        }
        return false;
    }

    /// <summary>
    /// Removes a crate reward by id
    /// </summary>
    /// <param name="itemID">String id of the crate</param>
    /// <returns>If removed or not</returns>
    public bool removeReward(string itemID)
    {
        if (rewards.ContainsKey(itemID))
        {
            rewards.Remove(itemID);
            return true;
        }
        Debug.LogWarning("Trying to remove a not existing item of rewards in crate: " + name);
        return false;
    }

    /// <summary>
    /// Sets the layer(rewardLevel) probability
    /// </summary>
    /// <param name="layer">Layer(rewardLevel)</param>
    /// <param name="probability">Probability (0->100)</param>
    public void setProbabilityOfLayer(int layer, float probability)
    {
        if (probability < 0 || probability > 100)
        {
            Debug.LogError("Trying to set a invalid probability! Try it beetween 0 and 100! Crate: " + name);
            return;
        }

        if (probabilities.ContainsKey(layer))
        {
            probabilities.Remove(layer);
            probabilities.Add(layer, probability);
        } else
        {
            probabilities.Add(layer, probability);
        }
    }

    /// <summary>
    /// Calculates the probability of each reward base on layer reward automatically
    /// </summary>
    public void calculateAutomaticRewardProbability()
    {
        Dictionary<int, int> layerCount = new Dictionary<int, int>();

        float probabilityLeft = 100f;
        int left = 0;
        float totalProb = 0;

        foreach (int id in probabilities.Keys)
        {
            float item = -1;
            probabilities.TryGetValue(id, out item);

            if (item != -1)
            {
                probabilityLeft -= item;
            }
        }

        foreach (string id in rewards.Keys)
        {
            CrateItem item;
            rewards.TryGetValue(id, out item);

            if (item != null)
            {
                if (probabilities.ContainsKey(item.getRewardLevel()))
                {
                    int count = -1;
                    layerCount.TryGetValue(item.getRewardLevel(), out count);

                    if (count != -1)
                    {
                        count++;
                        layerCount.Remove(item.getRewardLevel());
                        layerCount.Add(item.getRewardLevel(), count);
                    } else
                    {
                        count = 1;
                        layerCount.Add(item.getRewardLevel(), count);
                    }
                } else
                {
                    left++;
                }
            }
        }

        foreach (string id in rewards.Keys)
        {
            CrateItem item;
            rewards.TryGetValue(id, out item);

            if (layerCount.ContainsKey(item.getRewardLevel()) && probabilities.ContainsKey(item.getRewardLevel()))
            {
                int count = -1;
                layerCount.TryGetValue(item.getRewardLevel(), out count);

                float probability = -1f;
                probabilities.TryGetValue(item.getRewardLevel(), out probability);

                if (count != -1)
                {
                    item.setProbability(probability / count);
                    totalProb += probability / count;
                }
            } else
            {
                item.setProbability(probabilityLeft / left);
                totalProb += probabilityLeft / left;
            }
        }

        if (totalProb > 100)
            Debug.LogError("Probabilities setted on crate " + name + " are too big!");

        if (totalProb < 100)
            Debug.LogError("Probabilities setted on crate " + name + " are too low!");
    }

    /// <summary>
    /// Gets the crate name
    /// </summary>
    /// <returns>Crate name</returns>
    public string getCrateName()
    {
        return name;
    }

    /// <summary>
    /// Set the Probability of a crate reward manually
    /// </summary>
    /// <param name="id">id of reward</param>
    /// <param name="probability">Probability (0->100)</param>
    public void setProbabiltyOfCrateItem(string id, float probability)
    {
        if (probability < 0 || probability > 100)
        {
            Debug.LogError("Trying to set a invalid probability! Try it beetween 0 and 100! Crate: " + name);
            return;
        }

        if (rewards.ContainsKey(id))
        {
            CrateItem item;
            rewards.TryGetValue(id, out item);

            item.setProbability(probability);

            rewards.Remove(id);
            rewards.Add(id, item);
        } else
        {
            Debug.LogError("Trying to set a probability in a invalid crate item! Check the id or if this crate cointais it! Crate: " + name);
            return;
        }
    }

    /// <summary>
    /// Checks if this crate have the reward by id
    /// </summary>
    /// <param name="id">id to check</param>
    /// <returns>Has or not</returns>
    public bool cointaisCrateItem(string id)
    {
        return rewards.ContainsKey(id);
    }
}