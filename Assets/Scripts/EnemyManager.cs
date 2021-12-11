using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyManager : MonoBehaviour
{
    private const float shootingRange = 1.5f, totalCooldown = 2f, animationTime = 0.1f, showTime = 0.8f;
    
    private float cooldownLeft = 2f, life = 4;
    private bool isInCooldown;

    private TMP_Text damageText;

    private bool destroyed;

    private void Awake()
    {
        damageText = transform.Find("Damage").GetComponent<TMP_Text>();
        
        Assert.IsNotNull(damageText);
    }

    private void Start()
    {
        damageText.enabled = false;
    }
    
    public void takeDamage(float damageTaken)
    {
        // lose life
        life-= damageTaken;
        
        if (life <= 0)
        {
            Debug.Log("Me muero pq no tengo mas vida");
            destroyed = true;
            Destroy(gameObject);
            return;
        }
        
        Debug.Log("Ahr q no mori");
        showDamage(damageTaken);
    }

    private void showDamage(float damageTaken)
    {
        if (destroyed)
            return;
        
        damageText.text = "-" + damageTaken;
        damageText.enabled = true;

        // En 0.7 segundos muestro el texto
        damageText.gameObject.transform.DOLocalMoveY(1.17f, animationTime).onComplete += hideDamage;
    }

    private void hideDamage()
    {
        if (destroyed)
            return;
        
        // Espero 0.8f segundos para que se oculte, y lo oculto en el mismo tiempo que se motro
        damageText.gameObject.transform.DOLocalMoveY(0.78f, animationTime).SetDelay(showTime).onComplete += () =>
        {
            if (destroyed)
                return;
            
            // Se desactiva el objeto
            damageText.enabled = false;
        };
    }

    private void Update()
    {
        if (destroyed)
            return;
        
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
