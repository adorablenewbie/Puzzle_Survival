using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour
{
    [SerializeField] Transform gun;
    [SerializeField] Transform firePoint;
    [SerializeField] float attackRange;
    [SerializeField] float attackSpeed;
    [SerializeField] bool isAttack;
    [SerializeField] LayerMask enemyLayer;
    [SerializeField] ParticleSystem muzzleFlash;
    [SerializeField] int damage = 30;
    [SerializeField] private float fireRate = 1f;
    private float fireCooldown = 0f;

    private Transform target;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FindNearestEnemy();
 
        if (Vector3.Distance(transform.position, target.position) > attackRange)
        {
            target = null;
            return;
        }

        if(target != null)
        {
            if(fireCooldown <=  0f)
            {
                Shoot();
                fireCooldown = 1f / fireRate;
            }
            fireCooldown -= Time.deltaTime;

        }

    }

    private void FindNearestEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, attackRange);
        float targetDist = Mathf.Infinity;
        Transform nearestEnemy = null;

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                float dist = Vector3.Distance(transform.position, collider.transform.position);

                if (dist < targetDist)
                {
                    targetDist = dist;
                    nearestEnemy = collider.transform;
                }
            }
        }

        if (nearestEnemy != null && targetDist <= attackRange)
        {
            target = nearestEnemy;
            Debug.Log(target);
            Vector3 dir = target.position - transform.position;
            Quaternion lookRot = Quaternion.LookRotation(dir);

            gun.rotation = lookRot;

        }
    }


    void Shoot()
    {
        if (muzzleFlash != null)
        {
            muzzleFlash.Play();
        }
        Ray ray = new Ray(firePoint.position, firePoint.forward);

        Debug.DrawRay(firePoint.position, firePoint.forward, Color.red);

        if (Physics.Raycast(ray, out RaycastHit hit, attackRange))
        {
            IDamagable damagable = hit.collider.gameObject.GetComponent<IDamagable>();

            if (damagable != null)
            {
                damagable.TakePhysicalDamage(damage);

            }//0.015 0.01 0.03
            else
            {
                return;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
