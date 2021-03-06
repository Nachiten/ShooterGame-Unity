using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;

public class EnemyManager : MonoBehaviour
{
    public float movementSpeed = 7, shootingCooldown = 2f, damage = 1, totalLife = 4;

    private const float animationTime = 0.7f, shootingRange = 1.5f;
    private float cooldownLeft, currentLife;
    
    private bool isInCooldown, destroyed;

    private Transform textTemplate, player;

    public ObjectType objectType;
    
    private WavesManager wavesManager;

    private void OnEnable()
    {
        currentLife = totalLife;
        isInCooldown = false;
        destroyed = false;
    }

    private void Awake()
    {
        textTemplate = transform.Find("Damage").GetComponent<Transform>();
        player = GameObject.Find("First Person Player").transform;
        wavesManager = GameObject.Find("ObjectSpawner").GetComponent<WavesManager>();
        
        Assert.IsNotNull(textTemplate);
        Assert.IsNotNull(player);
        Assert.IsNotNull(wavesManager);
    }

    private void Start()
    {
        textTemplate.gameObject.SetActive(false);
        GetComponent<NavMeshAgent>().speed = movementSpeed;
    }

    private const float moveTime = 0.5f, moveMultiplier = 0.8f;

    public void takeDamage(float damageTaken, Vector3 direction)
    {
        // lose life
        currentLife-= damageTaken;
        
        if (currentLife <= 0)
        {
            destroyed = true;
            
            // Kill all animations asociated to this gameobject
            transform.DOKill();
            
            // Generate ammo where the enemy was killed
            GameObject ammo = ObjectPoolManager.instance.getObject(ObjectType.Ammo);

            ammo.transform.position = new Vector3(transform.position.x, -0.007f, transform.position.z);
            ammo.SetActive(true);
            
            // Return the enemy to the pool
            ObjectPoolManager.instance.returnObject(gameObject, objectType);
            
            wavesManager.checkIfWaveIsOver();
            
            return;
        }
        
        showDamage(damageTaken);

        direction *= moveMultiplier;
        direction.y = 0;
        
        transform.DOMove(transform.position + direction, moveTime).SetEase(Ease.OutExpo);
    }

    private void showDamage(float damageTaken)
    {
        if (destroyed)
            return;

        Transform damageText = Instantiate(textTemplate, transform);

        damageText.gameObject.SetActive(false);
        
        damageText.position = textTemplate.position;
        damageText.LookAt(player);
        damageText.GetComponent<TMP_Text>().text = "-" + damageTaken;
        
        damageText.gameObject.SetActive(true);

        // Show the text in animationTime seconds
        damageText.DOLocalMoveY(4.93f, animationTime)
            .OnUpdate( () => setAlpha(damageText))
            .OnComplete( () => hideDamage(damageText));
    }
    
    private void setAlpha(Transform text)
    {
        if (destroyed)
            return;

        // Calculate courrent alpha according to the distance to the center point
        float currentAlpha = 1 - Math.Abs((text.localPosition.y - 4.09f) / 0.84f);

        // Alpha is set to the calculated value
        text.GetComponent<TextMeshPro>().alpha = currentAlpha;
    }

    private void hideDamage(Transform theTransform)
    {
        if (destroyed)
            return;
        
        // Destroy the text
        Destroy(theTransform.gameObject);
    }

    private void Update()
    {
        if (destroyed)
            return;
        
        if (isInCooldown)
        {
            // Reduce cooldown
            cooldownLeft -= Time.deltaTime;

            // Return if there is cooldown remaining
            if (cooldownLeft > 0) 
                return;
            
            // Reset cooldown if it is finished
            isInCooldown = false;

            return;
        }
        
        if (!(Physics.Raycast(transform.position, transform.forward, out var hit, shootingRange) && hit.collider.gameObject.CompareTag("Player")))
            return;
        
        // draw the ray
        Debug.DrawRay(transform.position, transform.forward * shootingRange, Color.red, 2f);

        isInCooldown = true;
        cooldownLeft = shootingCooldown;
        
        hit.collider.gameObject.GetComponent<PlayerLifeManager>().loseLife(damage);
    }
}
