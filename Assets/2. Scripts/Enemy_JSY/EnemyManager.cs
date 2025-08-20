using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private Enemy_Zombie zombiePrefab;
    [SerializeField] private GameObject bearPrefab;

    [SerializeField] private Transform[] spawnPoint;

    List<Enemy_Zombie> zombies = new List<Enemy_Zombie>();

    public float interval = 2.0f;

    private Coroutine loop;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I)) NightStart();

        if(Input.GetKeyDown(KeyCode.K))
        {
            AllRemove();
        }
    }

    public void NightStart()
    {
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
        foreach(Enemy_Zombie zombie in zombies)
        {
            Destroy(zombie.gameObject);
        }
        zombies.Clear();
        StopCoroutine(loop);
        loop = null;
    }
}
