using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BonusReward
{
    public enum BonusRewardType
    {
        None = 0,
        Life,
        Brush,
        Gems,
        Hammer,
        Coins,

        Count // Leave as last element in enum
    }

    public BonusRewardType rewardType = BonusRewardType.None;
    public int value = 0;
    public float dropChance = 12.5f;

    public static string StringFromType(BonusRewardType bonusRewardType)
    {
        string results = "";
        switch (bonusRewardType)
        {
            case BonusReward.BonusRewardType.None:
                results = "None";
                break;
            case BonusReward.BonusRewardType.Life:
                results = "Life";
                break;
            case BonusReward.BonusRewardType.Brush:
                results = "Brush";
                break;
            case BonusReward.BonusRewardType.Gems:
                results = "Gems";
                break;
            case BonusReward.BonusRewardType.Hammer:
                results = "Hammer";
                break;
            case BonusReward.BonusRewardType.Coins:
                results = "Coins";
                break;
            case BonusReward.BonusRewardType.Count:
                // Do nothing, not an actual valid reward type
                break;
        }
        return results;
    }

}
