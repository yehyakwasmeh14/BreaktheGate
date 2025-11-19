using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class WeaponController : MonoBehaviour
{
    [Header("Shooting Settings")]
    public int damage = 1;
    public float weaponRange = 100f;
    public LayerMask hitLayers = -1;
    
    [Header("Ammo Settings")]
    public int startingAmmo = 45;
    public int maxAmmo = 99;
    public int currentAmmo;
    public TextMeshProUGUI ammoText;
    
    [Header("VFX")]
    public Transform muzzlePoint;
    public ParticleSystem muzzleFlashPrefab;
    public float muzzleFlashScale = 0.1f;
    public float muzzleFlashDuration = 0.2f;
    public float muzzleFlashOffset = 0.5f;
    
    [Header("Audio")]
    public AudioClip[] shootSounds;
    public float shootVolume = 0.5f;
    
    [Header("References")]
    public Camera playerCamera;
    public PlayerAiming playerAiming;

    private AudioSource audioSource;

    void Awake()
    {
        currentAmmo = startingAmmo;
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
        audioSource.volume = shootVolume;
        
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
        
        if (playerAiming == null)
        {
            playerAiming = GetComponentInParent<PlayerAiming>();
        }
        
        if (muzzlePoint == null)
        {
            GameObject muzzlePointObj = new GameObject("MuzzlePoint");
            muzzlePointObj.transform.SetParent(transform);
            muzzlePointObj.transform.localPosition = Vector3.zero;
            muzzlePointObj.transform.localRotation = Quaternion.identity;
            muzzlePoint = muzzlePointObj.transform;
        }

        UpdateAmmoUI();
    }

    void Update()
    {
        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (playerAiming != null && playerAiming.IsAiming)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        if (currentAmmo <= 0)
        {
            return;
        }

        if (shootSounds.Length > 0)
        {
            audioSource.pitch = Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(shootSounds[Random.Range(0, shootSounds.Length)]);
        }

        if (muzzleFlashPrefab != null && muzzlePoint != null)
        {
            Vector3 aimDirection = playerCamera.transform.forward;
            Quaternion flashRotation = Quaternion.LookRotation(aimDirection);
            Vector3 flashPosition = muzzlePoint.position + (aimDirection * muzzleFlashOffset);
            
            ParticleSystem flash = Instantiate(muzzleFlashPrefab, flashPosition, flashRotation);
            flash.transform.localScale = Vector3.one * muzzleFlashScale;
            
            var main = flash.main;
            main.duration = muzzleFlashDuration;
            main.startLifetime = muzzleFlashDuration;
            main.startSpeed = 0f;
            main.simulationSpace = ParticleSystemSimulationSpace.World;
            
            var velocityOverLifetime = flash.velocityOverLifetime;
            velocityOverLifetime.enabled = false;
            
            var inheritVelocity = flash.inheritVelocity;
            inheritVelocity.enabled = false;
            
            flash.Clear();
            flash.Play();
            
            Destroy(flash.gameObject, 2f);
        }
        
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, weaponRange, hitLayers))
        {
            EnemyHealth enemyHealth = hit.collider.GetComponent<EnemyHealth>();
            if (enemyHealth != null)
            {
                enemyHealth.TakeDamage(damage);
            }

            ExplosiveFuel explosiveFuel = hit.collider.GetComponent<ExplosiveFuel>();
            if (explosiveFuel != null)
            {
                explosiveFuel.TakeDamage();
            }
        }

        currentAmmo--;
        UpdateAmmoUI();
    }

    void UpdateAmmoUI()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo.ToString();
        }
    }

    public void AddAmmo(int amount)
    {
        currentAmmo += amount;
        if (currentAmmo > maxAmmo)
        {
            currentAmmo = maxAmmo;
        }
        UpdateAmmoUI();
    }
}
