using TMPro;
using UnityEngine;
using UnityEngine.Assertions;

public class PlayerAmmoManager : MonoBehaviour
{
    private int totalAmmo = 30, currentAmmo = 0;
    
    private const int magazineSize = 10;

    private TMP_Text ammoText, reloadingText;

    private bool reloading;

    private const float reloadTime = 3f;
    private float reloadTimer;
    
    void Awake()
    {
        ammoText = GameObject.Find("Ammo Value").GetComponent<TMP_Text>();
        Assert.IsNotNull(ammoText);
        
        reloadingText = GameObject.Find("Reloading").GetComponent<TMP_Text>();
        Assert.IsNotNull(reloadingText);
    }

    private void Start()
    {
        // This is to fill current ammo with magazine size
        currentAmmo = magazineSize;
        totalAmmo -= magazineSize;
        updateUI();

        reloadingText.enabled = false;
    }

    private void Update()
    {
        if (!reloading) 
            return;
        
        reloadTimer -= Time.deltaTime;
        
        if (reloadTimer > 0)
            return;
        
        finishReloading();
    }

    public void reload()
    {
        // The ammo im gonna to reload is magazineSize - currentAmmo or totalAmmo if there is less left
        int ammoToReload = Mathf.Min(magazineSize - currentAmmo, totalAmmo);

        // If there is no ammo left or nothing to reload, return
        if (ammoToReload == 0)
            return;
        
        // Update current ammo to fill magazine
        currentAmmo += ammoToReload;
        // Update total ammo with ammo left over
        totalAmmo -= ammoToReload;
        
        reloading = true;
        reloadingText.enabled = true;
        reloadTimer = reloadTime;
    }
    
    private void finishReloading()
    {
        // Update UI
        updateUI();
        
        reloading = false;
        reloadingText.enabled = false;
    }

    private void updateUI()
    {
        ammoText.text = "Ammo: " + currentAmmo + "/" + totalAmmo;
    }

    public void shoot()
    {
        currentAmmo--;
        updateUI();
        
        if (currentAmmo == 0)
            reload();
    }

    public bool canShoot()
    {
        return currentAmmo > 0 && !reloading;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Trigger contra: " + other.tag);
    }
}
