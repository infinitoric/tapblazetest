using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSuite : MonoBehaviour
{
    public int numTestsToPerform = 1;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public string TestBonusWheel()
    {
        // Create a dictionary to store the results so it's flexible if we change the rewards or values later
        // Dictionary uses a composite key based on the reward + the value, ie (Gems, 35)
        Dictionary<(BonusReward.BonusRewardType, int), int> countOfOutcomes = new Dictionary<(BonusReward.BonusRewardType, int), int>();

        // Add predicted results to compare against, in order to judge the test as Passed or Failed
        Dictionary<(BonusReward.BonusRewardType, int), float> predictedResults = new Dictionary<(BonusReward.BonusRewardType, int), float>();
        predictedResults.Add((BonusReward.BonusRewardType.Life, 30), 20.0f);
        predictedResults.Add((BonusReward.BonusRewardType.Brush, 30), 20.0f);
        predictedResults.Add((BonusReward.BonusRewardType.Gems, 30), 20.0f);
        predictedResults.Add((BonusReward.BonusRewardType.Hammer, 30), 20.0f);
        predictedResults.Add((BonusReward.BonusRewardType.Coins, 30), 20.0f);
        predictedResults.Add((BonusReward.BonusRewardType.Brush, 1), 20.0f);
        predictedResults.Add((BonusReward.BonusRewardType.Gems, 75), 20.0f);
        predictedResults.Add((BonusReward.BonusRewardType.Hammer, 1), 20.0f);


        // Run the test, compiling the results
        for (int i = 0; i < numTestsToPerform; i++)
        {
            BonusReward bonusReward = this.gameObject.GetComponent<GameManager>().TestBonusWheel();
            if (countOfOutcomes.ContainsKey((bonusReward.rewardType, bonusReward.value)))
            {
                countOfOutcomes[(bonusReward.rewardType, bonusReward.value)] = countOfOutcomes[(bonusReward.rewardType, bonusReward.value)] + 1;
            }
            else
            {
                countOfOutcomes.Add((bonusReward.rewardType, bonusReward.value), 1);
            }
        }

        // Iterate through the results, constructing the output
        string output = "Performed " + numTestsToPerform + " test(s) of Bonus Wheel. Results:\n\n";
        int numOutcomes = 0;
        foreach (KeyValuePair<(BonusReward.BonusRewardType, int), int> keyValuePair in countOfOutcomes)
        {
            if (numOutcomes > 0)
            {
                output += "\n";
            }
            int dropCount = keyValuePair.Value;
            float dropChance = Mathf.Floor(((float)keyValuePair.Value / (float)numTestsToPerform) * 1000.0f) / 10.0f; // Convert to percentage w/ one decimal place
            output += "" + BonusReward.StringFromType(keyValuePair.Key.Item1) + "(" + keyValuePair.Key.Item2 + "): " + dropCount + "/" + numTestsToPerform + " = " + dropChance + "%";
            numOutcomes++;
        }

        Debug.Log(output);

        return output;
    }
}
