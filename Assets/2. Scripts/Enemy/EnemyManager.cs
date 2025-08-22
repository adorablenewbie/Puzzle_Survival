using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy_Zombie zombiePrefab;
    [SerializeField] private Enemy_Bear bearPrefab;

    [SerializeField] private Transform[] zombieSpawns;
    [SerializeField] private Transform[] bearSpawns;

    private float curTime;

    List<Enemy_Zombie> zombies = new List<Enemy_Zombie>();
    List<Enemy_Bear> bears = new List<Enemy_Bear>();

    private int maxBearCnt;

    public float interval = 0.1f;

    private Coroutine loop;

    [SerializeField]DayNightCycle dayNightCycle;

    // Start is called before the first frame update
    void Start()
    {
        dayNightCycle.OnDay += HandleDay;
        dayNightCycle.OnNight += HandleNight;
    }

    private void HandleDay()
    {
        DayStart();
        ZombieAllRemove();
    }

    private void HandleNight()
    {
        BearAllRemove();
        NightStart();
    }


    public void DayStart()
    {
        for(int i = 0; i < bearSpawns.Length; i++)
        {
            Enemy_Bear bear = Instantiate(bearPrefab, bearSpawns[i]);
            bears.Add(bear);
            bear.OnDie += BearListUpdate;
        } 
    }

    public void NightStart()
    {
        if (loop == null) loop = StartCoroutine(ZombieSpawnLoop());
    } 

    IEnumerator ZombieSpawnLoop()
    {
        while (true)
        {
            SpawnZombie();
            yield return new WaitForSeconds(interval);
        }
    }


    public void RespawnBear()
    {
        int index = Random.Range(0, bearSpawns.Length);
        Enemy_Bear bear = Instantiate(bearPrefab, bearSpawns[index]);
        bears.Add(bear);
        bear.OnDie += BearListUpdate;
    }


    private void SpawnZombie()
    {
        if (zombieSpawns == null) return;

        int index = Random.Range(0, zombieSpawns.Length);

        Enemy_Zombie zombie = Instantiate(zombiePrefab, zombieSpawns[index]);

        zombies.Add(zombie);

        zombie.OnDie += ZombieListUpdate;

    }

    public void ZombieListUpdate(Enemy_Zombie deadZombie)
    {
        if(zombies.Contains(deadZombie))
        {
            zombies.Remove(deadZombie);
        }
    }

    private void ZombieAllRemove()
    {
        foreach(Enemy_Zombie zombie in zombies)
        {
            Destroy(zombie.gameObject);
        }
        zombies.Clear();
        if(loop != null)
        {
            StopCoroutine(loop);
        }

        loop = null;
    }

    private void BearAllRemove()
    {
        foreach(Enemy_Bear bear in bears)
        {
            Destroy(bear.gameObject);
        }
        bears.Clear();
    }

    public void BearListUpdate(Enemy_Bear deadBear)
    {
        if(bears.Contains(deadBear))
        {
            bears.Remove(deadBear);
        }

        Transform spawn = deadBear.transform.parent;
        Enemy_Bear newBear = Instantiate(bearPrefab, spawn);
        bears.Add(newBear);
        newBear.OnDie += BearListUpdate;
    }
}
