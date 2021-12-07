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
    
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("There hasbeen a trigger");
        
        if (!other.gameObject.CompareTag("Enemy")) 
            return;
        
        playerLife -= 1;
        UpdatePlayerLifeText();
    }
    
    // if the player colliders with tag "Enemy" it loses one life
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("There has been a colission");
        
        if (!collision.gameObject.CompareTag("Enemy")) 
            return;
        
        playerLife--;
        UpdatePlayerLifeText();
        
        if (playerLife <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void UpdatePlayerLifeText()
    {
        playerLifeText.text = "Vida: " + playerLife + "/10";
    }
    
    
}
