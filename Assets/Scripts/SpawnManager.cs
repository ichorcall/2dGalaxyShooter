using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefab;

    [SerializeField]
    private GameObject[] _powerUps;

    
    private bool _spawnEnemy = true;
    private bool _spawnPowerup = true;
<<<<<<< Updated upstream
=======
    private bool _spawnCollectable = true;
    
>>>>>>> Stashed changes

    [SerializeField]
    private GameObject _enemyContainer;

<<<<<<< Updated upstream
=======

    [SerializeField]
    private GameObject _homingLaserPowerup;

    [SerializeField]
    private float _spawnTime = 6f;
    [SerializeField]
    private int _waveNumber = 0;
    private bool _startWave = true;

    private UIManager _uiManager;

    public void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI manager is null");
        }
    }

>>>>>>> Stashed changes
    public void StartSpawning()
    {
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnEnemyRoutine());
<<<<<<< Updated upstream
        
=======
        StartCoroutine(SpawnCollectablesRoutine());
        StartCoroutine(EnemyWave());
        _uiManager.ChangeWaveCount(_waveNumber);
    }

    //every 20 seconds, divide waitforseconds by 1.5, and flash ui indicating wave.
    public IEnumerator EnemyWave()
    {     
        while (_startWave == true)
        {
            yield return new WaitForSeconds(20f);
            _spawnTime /= 1.2f;
            _waveNumber += 1;
            _uiManager.ChangeWaveCount(_waveNumber);           
        }
>>>>>>> Stashed changes
    }

    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);     

        while(_spawnEnemy == true)
        {
            float randomPosX = Random.Range(-5f, 6f);

            int randomEnemyChance = (Random.Range(0, 10));
            int enemyID = 0;
            if(randomEnemyChance >= 0 && randomEnemyChance < 4)
            {
                enemyID = 1;
            }
            else
            {
                enemyID = 0;
            }

            GameObject enemy = Instantiate(_enemyPrefab[enemyID], new Vector3(randomPosX, 7, 0), Quaternion.identity);
            enemy.transform.parent = _enemyContainer.transform;

            yield return new WaitForSeconds(_spawnTime);
        }
    }
    
    public IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_spawnPowerup == true)
        {
            float randomPosX = Random.Range(-5f, 6f);
            int randomPowerup = Random.Range(0, _powerUps.Length);
            GameObject powerup = Instantiate(_powerUps[randomPowerup], new Vector3(randomPosX, 7, 0), Quaternion.identity);

            float randomTime = Random.Range(3, 8);
            yield return new WaitForSeconds(randomTime);


        }
       
    }

    private GameObject[] leftOverPowerups;
    private GameObject[] leftOverEnemies;

    int itemID;
    public void OnPlayerDeath()
    {
        _spawnPowerup = false;
        _spawnEnemy = false;
<<<<<<< Updated upstream
=======
        _spawnCollectable = false;
        _startWave = false;
>>>>>>> Stashed changes
        

        leftOverPowerups = GameObject.FindGameObjectsWithTag("Powerup");
        leftOverEnemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject p in leftOverPowerups)
        {
            Destroy(p);
        }
        foreach (GameObject e in leftOverEnemies)
        {
            Destroy(e, 2f);
        }
    }

    
}
