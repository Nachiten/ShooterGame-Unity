using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyManager : MonoBehaviour
{
    // the life of the enemy
    private float life = 4;
    // Shooting range
    private const float shootingRange = 1.5f;
    
    private const float totalCooldown = 2f;
    private float cooldownLeft = 2f;
    private bool isInCooldown;

    private TMP_Text damageText;

    private void Awake()
    {
        damageText = transform.Find("Damage").GetComponent<TMP_Text>();
        
        Assert.IsNotNull(damageText);
    }

    private void Start()
    {
        damageText.enabled = false;
    }

    private const float showTime = 0.7f;

    public void takeDamage(float damageTaken)
    {
        // lose life
        life-= damageTaken;
        
        if (life <= 0)
        {
            Destroy(gameObject);
            return;
        }
        
        showDamage(damageTaken);
    }

    private void showDamage(float damageTaken)
    {
        damageText.text = "-" + damageTaken;
        damageText.enabled = true;
        
        LeanTween.value(0, showTime, showTime).setOnComplete(hideDamage);
    }

    private void hideDamage()
    {
        damageText.enabled = false;
    }

    private void Update()
    {
        if (isInCooldown)
        {
            // reduce cooldown
            cooldownLeft -= Time.deltaTime;

            // return if there is cooldown remaining
            if (cooldownLeft > 0) 
                return;
            
            // reset cooldown if it finished
            isInCooldown = false;
            cooldownLeft = totalCooldown;
            
            return;
        }
        
        if (!(Physics.Raycast(transform.position, transform.forward, out var hit, shootingRange) && hit.collider.gameObject.CompareTag("Player")))
            return;
        
        // draw the ray
        Debug.DrawRay(transform.position, transform.forward * shootingRange, Color.red, 2f);

        isInCooldown = true;
        hit.collider.gameObject.GetComponent<PlayerLifeManager>().loseLife(1);
    }
}
