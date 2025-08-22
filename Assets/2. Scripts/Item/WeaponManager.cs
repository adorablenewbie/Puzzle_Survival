using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    

    [SerializeField]
    private WeaponGun currentGun;

    private float currentFireRate;

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        GunFireRateCalc();
        TryFire();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);
    }
    void GunFireRateCalc()
    {
        if(currentFireRate > 0)
        {
            currentFireRate -= Time.deltaTime; // 1초의 1/60 1초에 1을 감소 시킨다.
        }
    }
    void TryFire()
    {
        if(Input.GetButton("Fire1") && currentFireRate <= 0)
        {
            Fire();
        }
    }

    private void Fire()
    {
        currentFireRate = currentGun.fireRate;
        Shoot();
    }

    private void Shoot()
    {
        if (currentGun.muzzleFlash != null )
        {
            currentGun.muzzleFlash.Play();
        }
        SoundManager.Instance.ShootingSound();
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width /2, Screen.height /2));

        if(Physics.Raycast(ray, out RaycastHit hit, currentGun.range))
        {
            IDamagable damagable = hit.collider.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.TakePhysicalDamage(currentGun.damage);

            }//0.015 0.01 0.03
            else
            {
                return;
            }
        }
    }
}
