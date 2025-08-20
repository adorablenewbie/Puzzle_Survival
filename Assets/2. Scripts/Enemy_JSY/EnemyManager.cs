using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy_Zombie zombiePrefab;
    [SerializeField] private GameObject bearPrefab;

    [SerializeField] private Transform[] spawnPoint;

    private float curTime;

    List<Enemy_Zombie> zombies = new List<Enemy_Zombie>();

    public float interval = 2.0f;

    private Coroutine loop;

    [SerializeField]DayNightCycle dayNightCycle;

    // Start is called before the first frame update
    void Start()
    {
        dayNightCycle.OnNight += NightStart;
        dayNightCycle.OnDay += AllRemove;
    }

    // Update is called once per frame
    void Update()
    {
        curTime = dayNightCycle.time;

        if(dayNightCycle.isNight)
        {
            NightStart();
            
        }

        if(dayNightCycle.isDay)
        {
            AllRemove();
            
        }


        if (curTime >= 0.75 && curTime < 0.76)
        {
            NightStart();
        }
        else if (curTime >= 0.25 && curTime < 0.26)
        {
            AllRemove();
        }

        if (Input.GetKeyDown(KeyCode.I)) NightStart();

        if(Input.GetKeyDown(KeyCode.K))
        {
            AllRemove();
        }
    }

    public void NightStart()
    {
        dayNightCycle.isNight = false;
        Debug.Log("dfasdfasd");
        if (loop == null) loop = StartCoroutine(SpawnLoop());
    }

    IEnumerator SpawnLoop()
    {
        while (true)
        {
            SpawnZombie();
            yield return new WaitForSeconds(interval);
        }

    }


    private void SpawnZombie()
    {
        if (spawnPoint == null) return;

        int index = Random.Range(0, spawnPoint.Length);

        Enemy_Zombie zombie = Instantiate(zombiePrefab, spawnPoint[index]);

        zombies.Add(zombie);

    }


    private void AllRemove()
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
}
