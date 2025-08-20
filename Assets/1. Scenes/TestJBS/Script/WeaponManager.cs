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
        PlaySE(currentGun.fire_Sound);
        currentGun.muzzleFlash.Play();
        Debug.Log("발싸 퓻");
    }

    private void PlaySE(AudioClip _clip)
    {
        audioSource.clip = _clip;
        audioSource.Play();
    }
}
