using TMPro;
using UnityEngine;

public class PlayerLifeManager : MonoBehaviour
{
    // life of the player starts at 10
    private float playerLife = 10;
    private TMP_Text playerLifeText;

    private void Start()
    {
        playerLifeText = GameObject.Find("PlayerLife").GetComponent<TMP_Text>();
        UpdatePlayerLifeText();
    }

    public void loseLife()
    {
        playerLife--;
        UpdatePlayerLifeText();
    }

    private void UpdatePlayerLifeText()
    {
        playerLifeText.text = "Vida: " + playerLife + "/10";
    }
    
    
}
