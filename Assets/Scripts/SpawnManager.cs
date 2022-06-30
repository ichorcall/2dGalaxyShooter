using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{

    public GameObject enemyPrefab;

    [SerializeField]
    private float _spawnTime = 4f;
    private bool _spawnEnemy = true;

    [SerializeField]
    private GameObject _enemyContainer;

    void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    // Update is called once per frame
    void Update()
    {

    }

    
    public IEnumerator SpawnRoutine()
    {
        while(_spawnEnemy == true)
        {
            float randomPosX = Random.Range(-9f, 9f);
            GameObject enemy = Instantiate(enemyPrefab, new Vector3(randomPosX, 7, 0), Quaternion.identity);

            enemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(_spawnTime);
        }
    }
    

    public void OnPlayerDeath()
    {
        _spawnEnemy = false;
    }

    
}
