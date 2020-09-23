using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Bonus Wheel")]
    [SerializeField]
    private BonusWheel bonusWheel;

    [Header("Player Stats")]
    [SerializeField]
    private int playerLives = 0;
    [SerializeField]
    private int playerBrushes = 0;
    [SerializeField]
    private int playerGems = 0;
    [SerializeField]
    private int playerHammers = 0;
    [SerializeField]
    private int playerCoins = 0;



    // Create singleton for GameManager, for accessibility from all scripts
    public static GameManager sharedInstance;
    private void Awake()
    {
        GameManager.sharedInstance = this;
    }



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClick_SpinButton()
    {
        StartCoroutine(SpinWheelAfterDelay());
    }

    public void OnClick_ClaimButton()
    {
        StartCoroutine(ClaimRewardAfterDelay());
        StartCoroutine(ResetBonusWheelAfterDelay());
    }

    IEnumerator SpinWheelAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
        this.bonusWheel.Spin();
    }

    IEnumerator ClaimRewardAfterDelay()
    {
        yield return new WaitForSeconds(0.3f);
        this.bonusWheel.ClaimReward();
    }

    IEnumerator ResetBonusWheelAfterDelay()
    {
        yield return new WaitForSeconds(0.5f);
        this.bonusWheel.Reset();
    }

    public BonusReward TestBonusWheel()
    {
        return this.bonusWheel.Test();
    }

    public void AddToPlayerLives(int value)
    {
        this.playerLives += value;
    }

    public void AddToPlayerBrushes(int value)
    {
        this.playerBrushes += value;
    }

    public void AddToPlayerGems(int value)
    {
        this.playerGems += value;
    }

    public void AddToPlayerHammers(int value)
    {
        this.playerHammers += value;
    }

    public void AddToPlayerCoins(int value)
    {
        this.playerCoins += value;
    }
}
