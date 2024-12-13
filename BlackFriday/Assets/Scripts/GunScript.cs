using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunScript : MonoBehaviour
{
    // Ammo
    // Can be added to or subtracted from
    public int ammo;
    public int maxAmmo = 80;

    // Base stats that can be increased by collecting certain items
    // magSize - number of bullets you can shoot before having to reload
    public int magSize;
    // cooldown - how quickly you can shoot in succession
    public float cooldown;
    // bulletAmount - when you shoot, this is the number of bullets that come out (1 to bajillions)
    public int bulletAmount;
    // damage - cmon now, you know this
    public float damage;

    // Gun-specific variables
    private float range = 200;
    [SerializeField]
    private LineRenderer bulletTrail;
    private Camera playerOrientation;
    [SerializeField]
    private LayerMask hitMask;
    [SerializeField]
    private Transform bulletSpawn;
    [SerializeField]
    private GameObject muzzleFlash;
    private bool canShoot;

    public KeyCode shootInput = KeyCode.Mouse0;

    private void Start()
    {
        ammo = 80;
        playerOrientation = Camera.main;
        bulletSpawn = GetComponent<Transform>();
        canShoot = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(shootInput))
        {
            if (ammo > 0 && canShoot)
            {
                canShoot = false;
                Shoot();
                ammo--;
                Invoke(nameof(ShootReset), cooldown);
            }
        }
    }

    private void ShootReset()
    {
        canShoot = true;
    }

    private void Shoot()
    {
        RaycastHit hit;
        if (Physics.Raycast(playerOrientation.transform.position, playerOrientation.transform.forward, out hit, range, hitMask))
        {
            Rigidbody target = hit.transform.GetComponent<Rigidbody>();
            if (target)
            {
                EnemyAI targetAI = target.transform.GetComponent<EnemyAI>();
                targetAI.TakeDamage(damage);
            }
            Debug.Log("Hit");
            StartCoroutine(DrawBulletTrail(hit.point));
        }
    }

    // Original code in the 3D game project, modified to make the trail fade out as it is disabled.
    // I like this more, as it makes the gun trail feel more realistic.
    private IEnumerator DrawBulletTrail(Vector3 hitLocation)
    {
        // Set the start and end points for the bullet trail
        bulletTrail.SetPosition(0, bulletSpawn.position);
        bulletTrail.SetPosition(1, hitLocation);

        // Enables the muzzle flash
        muzzleFlash.SetActive(true);

        // Enables the trail
        bulletTrail.enabled = true;

        // Start fading the trail
        float fadeDuration = 0.1f;  // The duration over which the trail will fade
        float elapsedTime = 0f;

        // Access the material of the LineRenderer and the initial color
        Color startColor = bulletTrail.startColor;

        // fades out the trail
        while (elapsedTime < fadeDuration)
        {
            // alpha determines the transparency of the bullet during fade time
            // effects the start and end transparency of the bullet trail
            float alpha = Mathf.Lerp(1f, 0f, elapsedTime / fadeDuration);
            bulletTrail.startColor = new Color(startColor.r, startColor.g, startColor.b, alpha);
            bulletTrail.endColor = new Color(startColor.r, startColor.g, startColor.b, alpha);

            // adds to elapsed time
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Disables the muzzle flash
        muzzleFlash.SetActive(false);

        // Makes sure the line is transparent
        bulletTrail.startColor = new Color(startColor.r, startColor.g, startColor.b, 0f);
        bulletTrail.endColor = new Color(startColor.r, startColor.g, startColor.b, 0f);

        // Disable the trail after fading
        bulletTrail.enabled = false;
    }

}
