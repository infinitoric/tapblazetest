using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BonusWheel : MonoBehaviour
{
    [SerializeField]
    private GameObject bonusWheel;
    [SerializeField]
    private RectTransform wheelContainer;
    [SerializeField]
    private Image blurredWheel;
    [SerializeField]
    private GameObject spinButton;
    [SerializeField]
    private GameObject claimButton;
    [SerializeField]
    private BonusWheelItem bonusWheelItem;


    [Header("Animation settings")]
    [SerializeField]
    private float animationDuration = 5.0f;
    [SerializeField]
    private float easeInDuration = 0.5f;
    [SerializeField]
    private float easeOutDuration = 0.5f;

    [Header("Rewards (Prizes)")]
    [SerializeField]
    private BonusReward[] bonusRewards;

    private const int numWheelSections = 8;

    private bool isAnimating = false;
    private float timeAtAnimationStart = 0;
    private float timeAtEaseOutStart = 0;
    private float rotationAngleAtEaseOutStart = 0;
    private float startRotationAngle = 0;
    private float targetRotationAngle = 0;
    private float previousRotationAngle = 0;
    private float currentRotationAngle = 0;
    private float currentSpeed = 1.0f;
    private bool didStartEaseOut = false;

    private BonusReward targetReward;

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        if (this.isAnimating)
        {
            AnimateToTarget();
        }
    }

    public void Reset()
    {
        this.targetReward = null;
        this.bonusWheel.SetActive(true);
        this.spinButton.SetActive(true);
        this.claimButton.SetActive(false);
        this.bonusWheelItem.gameObject.SetActive(false);
        this.isAnimating = false;
        this.timeAtAnimationStart = 0;
        this.timeAtEaseOutStart = 0;
        this.rotationAngleAtEaseOutStart = 0;
        this.startRotationAngle = 0;
        this.targetRotationAngle = 0;
        this.previousRotationAngle = 0;
        this.currentRotationAngle = 0;
        this.currentSpeed = 1.0f;
        this.didStartEaseOut = false;
        this.wheelContainer.localRotation = Quaternion.Euler(0, 0, this.currentRotationAngle);
        this.blurredWheel.color = new Color(this.blurredWheel.color.r, this.blurredWheel.color.g, this.blurredWheel.color.b, 0);
        if (!this.spinButton.GetComponent<Animator>().isInitialized)
        {
            this.spinButton.GetComponent<Animator>().Rebind();
        }
        if (!this.claimButton.GetComponent<Animator>().isInitialized)
        {
            this.claimButton.GetComponent<Animator>().Rebind();
        }
    }

    public void Spin()
    {
        // TODO: Play SFX
        // Check if wheel hasn't been spun yet before spinning
        if (!this.isAnimating)
        {
            this.targetReward = SpinWheelWithAnimation(true);
        }
    }

    public void ClaimReward()
    {
        // TODO: Play SFX
        if (this.targetReward != null)
        {
            GiveRewardToPlayer(this.targetReward);
            this.targetReward = null;
            this.bonusWheelItem.gameObject.SetActive(false);
        }
    }

    public BonusReward Test()
    {
        return SpinWheelWithAnimation(false);
    }



    // This method both initiates the spin animation as well as returns the resultant reward,
    //  so that it can be processed and the user's. Calling w/ shouldPlayAnimation = false
    //  allows for testing without triggering animation.
    private BonusReward SpinWheelWithAnimation(bool shouldPlayAnimation = true)
    {
        // Get target reward 
        float randomPercentage = Random.Range(0, 100.0f);
        BonusReward bonusReward = null;
        float cumulativeDropChance = 0;
        int bonusIndex = 0;
        foreach (BonusReward reward in this.bonusRewards)
        {
            cumulativeDropChance += reward.dropChance;
            if (randomPercentage < cumulativeDropChance)
            {
                bonusReward = reward;
                break;
            }
            bonusIndex++;
        }

        // TODO: Play animation based on target reward (bonusIndex)
        if (shouldPlayAnimation)
        {
            this.targetRotationAngle = (((float)bonusIndex + 0.5f) / (float)numWheelSections) * 360.0f;
            StartAnimationTowardsTarget();
        }

        return bonusReward;
    }

    private void GiveRewardToPlayer(BonusReward bonusReward)
    {
        switch (bonusReward.rewardType)
        {
            case BonusReward.BonusRewardType.None:
                // Do nothing, no reward to give
                break;
            case BonusReward.BonusRewardType.Life:
                GameManager.sharedInstance.AddToPlayerLives(bonusReward.value);
                break;
            case BonusReward.BonusRewardType.Brush:
                GameManager.sharedInstance.AddToPlayerBrushes(bonusReward.value);
                break;
            case BonusReward.BonusRewardType.Gems:
                GameManager.sharedInstance.AddToPlayerGems(bonusReward.value);
                break;
            case BonusReward.BonusRewardType.Hammer:
                GameManager.sharedInstance.AddToPlayerHammers(bonusReward.value);
                break;
            case BonusReward.BonusRewardType.Coins:
                GameManager.sharedInstance.AddToPlayerCoins(bonusReward.value);
                break;
            case BonusReward.BonusRewardType.Count:
                // Do nothing, not an actual valid reward type
                break;
        }
    }

    private void StartAnimationTowardsTarget()
    {
        this.startRotationAngle = this.wheelContainer.localRotation.eulerAngles.z;
        this.currentRotationAngle = this.startRotationAngle;
        this.previousRotationAngle = this.currentRotationAngle;
        this.timeAtAnimationStart = Time.time;
        this.isAnimating = true;
    }

    private void AnimateToTarget()
    {
        this.previousRotationAngle = this.currentRotationAngle;

        float timeSinceStart = Time.time - this.timeAtAnimationStart;

        if (timeSinceStart < this.easeInDuration)
        {
            // Ease In
            float tEaseIn = timeSinceStart / this.easeInDuration;
            float easing = BackEaseIn(tEaseIn, 0, 1.0f, 1.0f);
            this.currentRotationAngle = easing * -360.0f * 0.25f;
            this.currentSpeed = (this.currentRotationAngle - this.previousRotationAngle) / Time.deltaTime;
        }
        else if (timeSinceStart < this.animationDuration - this.easeOutDuration)
        {
            // Main rotation period
            //this.currentSpeed = -360.0f * 2.0f;
            this.currentRotationAngle += this.currentSpeed * Time.deltaTime;
        }
        else if (timeSinceStart < this.animationDuration)
        {
            // Ease Out
            if (!didStartEaseOut)
            {
                // Shift target angle to 4 rotations ahead
                while (targetRotationAngle > this.currentRotationAngle - (4.0f * 360.0f))
                {
                    targetRotationAngle -= 360.0f;
                }
                this.rotationAngleAtEaseOutStart = this.currentRotationAngle;
                this.timeAtEaseOutStart = Time.time;
                didStartEaseOut = true;
            }
            float timeSinceEaseOutStart = Time.time - this.timeAtEaseOutStart;
            float tEaseOut = timeSinceEaseOutStart / this.easeOutDuration;
            float easing = SineEaseOut(tEaseOut, 0, 1.0f, 1.0f);
            this.currentRotationAngle = this.rotationAngleAtEaseOutStart + easing * (this.targetRotationAngle - this.rotationAngleAtEaseOutStart);
            this.currentSpeed = (this.currentRotationAngle - this.previousRotationAngle) / Time.deltaTime;
        }
        else
        {
            // Finish
            this.currentSpeed = 0;
            this.currentRotationAngle = targetRotationAngle;
            this.isAnimating = false;
            StartCoroutine(ShowRewardAfterDelay());
            StartCoroutine(HideWheelAfterDelay());
        }

        this.wheelContainer.localRotation = Quaternion.Euler(0, 0, this.currentRotationAngle);

        // Set blurred wheel opacity based on current speed to simulate motion blur effect
        this.blurredWheel.color = new Color(this.blurredWheel.color.r, this.blurredWheel.color.g, this.blurredWheel.color.b, Mathf.Clamp01(Mathf.Abs(1.0f * this.currentSpeed/ 1000.0f)));


    }

    IEnumerator ShowRewardAfterDelay()
    {
        yield return new WaitForSeconds(0.1f);
        this.bonusWheelItem.SetVisibilityWithBonusReward(this.targetReward);
        this.bonusWheelItem.gameObject.SetActive(true);
    }

    IEnumerator HideWheelAfterDelay()
    {
        yield return new WaitForSeconds(0.4f);
        this.claimButton.SetActive(true);
        this.bonusWheel.SetActive(false);
    }

    // Easing equation by Robert Penner (http://robertpenner.com/easing/)
    public float BackEaseIn(float t, float b, float c, float d)
    {
        return c * (t /= d) * t * ((1.70158f + 1) * t - 1.70158f) + b;
    }

    // Easing equation by Robert Penner (http://robertpenner.com/easing/)
    public float SineEaseOut(float t, float b, float c, float d)
    {
        return c * Mathf.Sin(t / d * (Mathf.PI / 2)) + b;
    }
}
