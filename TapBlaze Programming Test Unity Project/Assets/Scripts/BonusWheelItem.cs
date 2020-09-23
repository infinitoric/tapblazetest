using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusWheelItem : MonoBehaviour
{
    public GameObject heartIcon;
    public GameObject brushIcon;
    public GameObject gemIcon;
    public GameObject hammerIcon;
    public GameObject coinIcon;
    public Text primaryText;
    public Text heartPrimaryText;
    public Text heartSecondaryText;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVisibilityWithBonusReward(BonusReward bonusReward)
    {
        heartIcon.SetActive(false);
        brushIcon.SetActive(false);
        gemIcon.SetActive(false);
        hammerIcon.SetActive(false);
        coinIcon.SetActive(false);
        primaryText.gameObject.SetActive(false);
        heartPrimaryText.gameObject.SetActive(false);
        heartSecondaryText.gameObject.SetActive(false);

        switch (bonusReward.rewardType)
        {
            case BonusReward.BonusRewardType.None:
                // Do nothing
                break;
            case BonusReward.BonusRewardType.Life:
                heartIcon.SetActive(true);
                heartPrimaryText.gameObject.SetActive(true);
                heartSecondaryText.gameObject.SetActive(true);
                heartPrimaryText.text = "x" + bonusReward.value;
                heartSecondaryText.text = "mins";
                break;
            case BonusReward.BonusRewardType.Brush:
                brushIcon.SetActive(true);
                primaryText.gameObject.SetActive(true);
                primaryText.text = "x" + bonusReward.value;
                break;
            case BonusReward.BonusRewardType.Gems:
                gemIcon.SetActive(true);
                primaryText.gameObject.SetActive(true);
                primaryText.text = "x" + bonusReward.value;
                break;
            case BonusReward.BonusRewardType.Hammer:
                hammerIcon.SetActive(true);
                primaryText.gameObject.SetActive(true);
                primaryText.text = "x" + bonusReward.value;
                break;
            case BonusReward.BonusRewardType.Coins:
                coinIcon.SetActive(true);
                primaryText.gameObject.SetActive(true);
                primaryText.text = "x" + bonusReward.value;
                break;
            case BonusReward.BonusRewardType.Count:
                // Do nothing, not an actual valid reward type
                break;
        }
    }
}
