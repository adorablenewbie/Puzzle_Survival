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

    public float interval = 2.0f;

    private Coroutine loop;

    [SerializeField]DayNightCycle dayNightCycle;

    // Start is called before the first frame update
    void Start()
    {
        maxBearCnt = bears.Count;
        
    }

    // Update is called once per frame
    void Update()
    {  
        Debug.Log(dayNightCycle.isNight);
        Debug.Log(dayNightCycle.isDay);

        if(dayNightCycle.isNight)
        {
            BearAllRemove();
            NightStart();
        }

        if(dayNightCycle.isDay)
        {
            DayStart();
            ZombieAllRemove();
        }
    }

    public void DayStart()
    {
        foreach(Transform spawn in bearSpawns)
        {
            Enemy_Bear bear = Instantiate(bearPrefab, spawn);
            bears.Add(bear);
            bear.OnDie += RespawnBear;
        } 
    }

    public void NightStart()
    {
        dayNightCycle.isNight = false;
        Debug.Log("dfasdfasd");
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
        foreach (Transform spawn in bearSpawns)
        {
            Enemy_Bear bear = Instantiate(bearPrefab, spawn);
            bears.Add(bear);
        }
    }


    private void SpawnZombie()
    {
        if (zombieSpawns == null) return;

        int index = Random.Range(0, zombieSpawns.Length);

        Enemy_Zombie zombie = Instantiate(zombiePrefab, zombieSpawns[index]);

        zombies.Add(zombie);

    }

    private void ZombieAllRemove()
    {
        dayNightCycle.isDay = false;
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
}
