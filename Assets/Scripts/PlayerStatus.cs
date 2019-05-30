using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatus : MonoBehaviour
{
    public float playerMaxHealth;
    [HideInInspector]
    public float playerCurrentHealth;

    private float playerMinHealth = 0f;
    private bool playerIsDead = false;
    private GameObject playersUI;
    private Slider healthBar;

    // Start is called before the first frame update
    void Start()
    {
        playerCurrentHealth = playerMaxHealth;

        //getting the battle UI script and perparing to reference a player's battle UI to a player
        GameObject battleUI = Resources.Load<GameObject>("UI/GameCanvas");
        var battleUIScript = battleUI.GetComponent<BattleUIController>();

        //find the player this script is attached to and reference the right players battle UI
        string playerName = gameObject.ToString();
        switch (playerName)
        {
            case "player1":
                playersUI = battleUIScript.player01;
                break;
            case "player2":
                playersUI = battleUIScript.player02;
                break;
            case "player3":
                playersUI = battleUIScript.player03;
                break;
            case "player4":
                playersUI = battleUIScript.player04;
                break;
            default:
                print("invalid player names. Names must be Player1, Player2, Player3, or Player4");
                break;
        }
        var sliderRef = playersUI.GetComponent<PlayerUIRef>();
        healthBar = sliderRef.healthSlider;

    }


    public void TakeDamage(float damageAmount)
    {
        playerCurrentHealth -= damageAmount;
        UpdateHealthBar();
        if (playerCurrentHealth <= playerMinHealth)
        {
            playerIsDead = true;
            print("I am dead");
        }
    }
    public void UpdateHealthBar()
    {
        healthBar.value = playerCurrentHealth / playerMaxHealth;
    }
}
