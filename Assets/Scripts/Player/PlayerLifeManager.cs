using System;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class PlayerLifeManager : MonoBehaviour
{
    private const float totalPlayerLife = 10f;
    private float playerLife;
    private Slider playerLifeSlider;

    private const float totalPlayerStamina = 50f;
    private float playerStamina;
    private Slider playerStaminaSlider;

    private bool isRunning;
    private PlayerMovement playerMovement;

    private bool isInCooldown;
    private const float totalCooldown = 1f;
    private float cooldownLeft;
    
    private void Awake()
    {
        playerLifeSlider = GameObject.Find("Health Bar").GetComponent<Slider>();
        playerStaminaSlider = GameObject.Find("Stamina Bar").GetComponent<Slider>();
        playerMovement = GetComponent<PlayerMovement>();

        Assert.IsNotNull(playerLifeSlider);
        Assert.IsNotNull(playerStaminaSlider);
        Assert.IsNotNull(playerMovement);
    }

    private void Start()
    {
        playerLife = totalPlayerLife;
        playerLifeSlider.minValue = 0;
        playerLifeSlider.maxValue = totalPlayerLife;
        
        playerStamina = totalPlayerStamina;
        playerStaminaSlider.minValue = 0;
        playerStaminaSlider.maxValue = totalPlayerStamina;
        
        updatePlayerLife();
        updatePlayerStamina();
    }
    
    private const float staminaIncrease = 5f, staminaDecrease = 10f;
    
    private void Update()
    {
        if (isRunning)
        {
            // If player is running, stamina decreases
            playerStamina -= Time.deltaTime * staminaDecrease;
            updatePlayerStamina();
        }
        
        if (!isRunning && playerStamina < totalPlayerStamina)
        {
            // If there is cooldown
            if (isInCooldown)
            {
                // Decrease the cooldown
                cooldownLeft -= Time.deltaTime;
            
                if (!(cooldownLeft <= 0)) 
                    return;
            
                // If there is no cooldown left, finish it
                isInCooldown = false;
                cooldownLeft = 0;
                return;
            }
            
            // If player is not running and stamina is not full, stamina increases
            playerStamina += Time.deltaTime * staminaIncrease;
            updatePlayerStamina();
        }

        if (playerStamina <= 0)
        {
            // If there is no stamina left, stop running
            playerStamina = 0;
            setIsRunning(false);
        }
    }

    public void loseLife(float value)
    {
        playerLife -= value;
        updatePlayerLife();
    }

    private void updatePlayerLife()
    {
        playerLifeSlider.value = playerLife;
    }

    private void updatePlayerStamina()
    {
        playerStaminaSlider.value = playerStamina;
    }
    
    public void setIsRunning(bool value)
    {
        isRunning = value;
        
        if (isRunning) 
            return;
        
        // If he stopped running, enter cooldown after starting stamina regen
        isInCooldown = true;
        cooldownLeft = totalCooldown;
    }

    public bool canRun()
    {
        return playerStamina > 0;
    }
}
