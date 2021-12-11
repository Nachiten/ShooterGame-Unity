using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class EnemyManager : MonoBehaviour
{
    private const float shootingRange = 1.5f, totalCooldown = 2f, animationTime = 0.6f;
    
    private float cooldownLeft = 2f, life = 4;
    
    private bool isInCooldown, destroyed;

    private Transform textTemplate;

    private void Awake()
    {
        textTemplate = transform.Find("Damage").GetComponent<Transform>();
        
        Assert.IsNotNull(textTemplate);
    }

    private void Start()
    {
        textTemplate.gameObject.SetActive(false);
    }

    private const float moveTime = 0.5f, moveMultiplier = 0.8f;
    
    public void takeDamage(float damageTaken, Vector3 direction)
    {
        // lose life
        life-= damageTaken;
        
        if (life <= 0)
        {
            destroyed = true;
            Destroy(gameObject);
            return;
        }
        
        showDamage(damageTaken, direction);

        direction *= moveMultiplier;
        direction.y = 0;
        
        transform.DOMove(transform.position + direction, moveTime).SetEase(Ease.OutExpo);
    }

    private void showDamage(float damageTaken, Vector3 direction)
    {
        if (destroyed)
            return;

        Transform damageText = Instantiate(textTemplate, transform);

        damageText.position = textTemplate.position;
        damageText.forward = direction;

        damageText.GetComponent<TMP_Text>().text = "-" + damageTaken;
        damageText.gameObject.SetActive(true);

        // En 0.7 segundos muestro el texto
        damageText.DOLocalMoveY(1.7f, animationTime)
            .OnUpdate( () => setAlpha(damageText))
            .OnComplete( () => hideDamage(damageText));
    }
    
    private void setAlpha(Transform text)
    {
        if (destroyed)
            return;

        float currentAlpha = 1 - Math.Abs((text.localPosition.y - 1.3f) / 0.5f);

        // El alpha se setea al modulo de la diferencia entre la posicion central y actual
        text.GetComponent<TextMeshPro>().alpha = currentAlpha;
    }

    private void hideDamage(Transform theTransform)
    {
        if (destroyed)
            return;
        
        // Se desactiva el objeto
        Destroy(theTransform.gameObject);
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
