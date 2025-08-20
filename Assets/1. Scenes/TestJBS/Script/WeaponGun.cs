using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGun : MonoBehaviour
{
    public float range; // ���� ��
    public float accurarcy; // ��Ȯ��
    public float fireRate; // ���癓��
    public float reloadTime; // ������ �ӵ�.

    public int damage; // ���� ������

    public int reloadBulletCount;// �Ѿ� ������ ����.
    public int currentBulletCount; // ���� ź������ �����ִ� �Ѿ��� ����.
    public int maxBulletCount; // �ִ� ���� ���� �Ѿ� ����.
    public int carryBulletCount; // ���� �����ϰ� �ִ� �Ѿ� ����.

    public float retroActionForce; // �ݵ� ����
    public float retroActionFineSightForce; // �����ؽ��� �ݵ� ����.

    public Vector3 fineSightOriginpos;

    public Animator anim;

    public ParticleSystem muzzleFlash;


    public AudioClip fire_Sound; 



 
}
