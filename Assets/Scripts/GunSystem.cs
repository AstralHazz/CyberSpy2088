using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunSystem : MonoBehaviour
{
    public Transform myCameraHead;
    public Transform firePosition;
    public GameObject bullet;
    public GameObject muzzleFlash, bulletHole, floorImpact, explosion, bloodEffect;
    public string gunName;
    public Animator myAnimator;

    //UI
    private UICanvasController myUICanvas;

    public bool canAutoFire = false;
    public bool isReloading = false;
    private bool shooting, readyToShoot = true;

    public float timeBetweenShots, reloadTime;

    public int bulletsAvailable, totalBullets, magazineSize;

    //Aiming
    public Transform aimPosition;
    public float aimSpeed = 20f;
    private Vector3 gunStartPosition;

    //Zooming
    public float pistolZoom = 2f;

    //DMG
    public int damageAmount;

    //Rocket Launcher
    public bool rocketLauncher;
    public GameObject rocketTrail;

    string gunAnimationName;

    public int pickupBulletAmount;

    // Start is called before the first frame update
    void Start()
    {
        totalBullets -= magazineSize;
        bulletsAvailable = magazineSize;

        gunStartPosition = transform.localPosition;

        myUICanvas = FindObjectOfType<UICanvasController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.gameIsPaused) { return; }
        Shoot();
        GunManager();
        UpdateAmmoText();
        AnimationManager();
    }

    private void AnimationManager()
    {
        switch (gunName)
        {
            case "Pistol":
                gunAnimationName = "Pistol Reload";
                break;
            case "Rifle":
                gunAnimationName = "Rifle Reload";
                break;
            case "Sniper":
                gunAnimationName = "Sniper Reload";
                break;
            case "Rocket Launcher":
                gunAnimationName = "Rocket Reload";
                break;
            default:
                break;
        }
    }

    private void GunManager()
    {
        if (Input.GetKeyDown(KeyCode.R) && bulletsAvailable < magazineSize)
            Reload();

        if (Input.GetMouseButton(1))
        {
            transform.position = Vector3.MoveTowards(transform.position, aimPosition.position, aimSpeed * Time.deltaTime);
        }
        else
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, gunStartPosition, aimSpeed * Time.deltaTime);
        }

        if (Input.GetMouseButtonDown(1))
        {
            FindObjectOfType<CameraMove>().ZoomIn(pistolZoom);
        }
        if (Input.GetMouseButtonUp(1))
        {
            FindObjectOfType<CameraMove>().ZoomOut();
        }

    }

    public void AddAmmo()
    {
        totalBullets += pickupBulletAmount;
    }

    private void Reload()
    {
        myAnimator.SetTrigger(gunAnimationName);

        AudioManager.instance.PlayerSFX(7);

        isReloading = true;
        StartCoroutine(Reloading());
    }

    IEnumerator Reloading()
    {
        yield return new WaitForSeconds(reloadTime);

        int bulletsToAdd = magazineSize - bulletsAvailable;

        if (totalBullets > bulletsToAdd)
        {
            totalBullets -= bulletsToAdd;
            bulletsAvailable = magazineSize;
        }
        else
        {
            bulletsAvailable += totalBullets;
            totalBullets = 0;
        }

        isReloading = false;
        readyToShoot = true;
    }

    private void Shoot()
    {

        if(canAutoFire)
        {
            shooting = Input.GetMouseButton(0);
        }
        else
        {
            shooting = Input.GetMouseButtonDown(0);
        }
        //Input.GetMouseButtonDown(0)
        if (shooting && readyToShoot && bulletsAvailable > 0 && !isReloading)
        {
            readyToShoot = false;
            RaycastHit hit;
            if (Physics.Raycast(myCameraHead.position, myCameraHead.forward, out hit, 1000f))
            {
                float dist = Vector3.Distance(myCameraHead.position, hit.point);
                if (dist > 2f)
                {
                    firePosition.LookAt(hit.point);

                    if (!rocketLauncher)
                    {
                        if (hit.collider.tag == "Shootable")
                        {
                            Instantiate(bulletHole, hit.point, Quaternion.LookRotation(hit.normal));
                        }
                        else if (hit.collider.tag == "Ground")
                        {
                            Instantiate(floorImpact, hit.point, Quaternion.LookRotation(hit.normal));
                        }
                    }
                }
                if (hit.collider.CompareTag("Enemy") && !rocketLauncher)
                {
                    hit.collider.GetComponent<EnemyHealthSystem>().TakeDamage(damageAmount);
                    Instantiate(bloodEffect, hit.point, Quaternion.LookRotation(hit.normal));
                    if (hit.collider.GetComponent<EnemyHealthSystem>().currentHealth <= 0)
                    {
                        Instantiate(explosion, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
                else if (hit.collider.CompareTag("Enemy") && rocketLauncher)
                {
                    hit.collider.GetComponent<EnemyHealthSystem>().TakeDamage(damageAmount);
                    if (hit.collider.GetComponent<EnemyHealthSystem>().currentHealth <= 0)
                    {
                        Instantiate(explosion, hit.point, Quaternion.LookRotation(hit.normal));
                    }
                }
            }
            else
            {
                firePosition.LookAt(myCameraHead.position + (myCameraHead.forward * 50f));
            }
            bulletsAvailable--;

            if (!rocketLauncher)
            {
                Instantiate(muzzleFlash, firePosition.position, firePosition.rotation, firePosition);
                Instantiate(bullet, firePosition.position, firePosition.rotation, firePosition);
            }
            else
            {
                Instantiate(bullet, firePosition.position, firePosition.rotation);
                Instantiate(rocketTrail, firePosition.position, firePosition.rotation);
            }


            StartCoroutine(ResetShot());

        }
    }

    IEnumerator ResetShot()
    {
        yield return new WaitForSeconds(timeBetweenShots);

        readyToShoot = true;
    }

    private void UpdateAmmoText()
    {
        myUICanvas.ammoText.SetText(bulletsAvailable + "/" + magazineSize);
        myUICanvas.totalAmmoText.SetText("[" + totalBullets.ToString() + "]");
    }
}
