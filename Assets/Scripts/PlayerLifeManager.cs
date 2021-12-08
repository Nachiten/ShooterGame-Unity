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
    
    private void Awake()
    {
        playerLifeSlider = GameObject.Find("Health Bar").GetComponent<Slider>();
        playerStaminaSlider = GameObject.Find("Stamina Bar").GetComponent<Slider>();

        Assert.IsNotNull(playerLifeSlider);
        Assert.IsNotNull(playerStaminaSlider);
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

    public void loseLife(float value)
    {
        playerLife -= value;
        updatePlayerLife();
    }

    private void updatePlayerLife()
    {
        playerLifeSlider.value = playerLife;
    }
    
    public void loseStamina(float value)
    {
        playerStamina -= value;
        updatePlayerStamina();
    }

    private void updatePlayerStamina()
    {
        playerStaminaSlider.value = playerStamina;
    }
}
