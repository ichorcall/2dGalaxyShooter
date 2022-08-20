using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _enemyPrefab;
    [SerializeField]
    private float[] _enemyWeights;
    private float _totalEnemyWeight = 0;
    
    [SerializeField]
    private GameObject[] _collectables;
    //0 ammo
    //1 hp
    
    private bool _spawnEnemy = true;
    private bool _spawnPowerup = true;
    private bool _spawnCollectable = true;

    [SerializeField]
    private GameObject _enemyContainer;

    [SerializeField]
    private GameObject _homingLaserPowerup;

    private UIManager _uiManager;

    [SerializeField]
    private float _spawnTime = 4f;
    [SerializeField]
    private int _waveNumber = 1;
    private bool _startWave = true;

    [SerializeField]
    private GameObject[] _powerUps;
    [SerializeField]
    private float[] _powerupWeights;
    private float _totalWeight = 0;

    [SerializeField]
    private GameObject _boss;
    [SerializeField]
    private GameObject _bossHPBar;
    private bool _restart = false;
    private bool _bossHere = false;



    public void Start()
    {

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI manager is null");
        }
        
        foreach (float w in _powerupWeights)
        {
            _totalWeight += w;
            //we should get a sum of 47
        }

        foreach (float w in _enemyWeights)
        {
            _totalEnemyWeight += w;
            //we should get a sum of 47
        }
    }

    public void Update()
    {
        if(_boss == null)
        {
            _bossHere = false;
        }

        if (_bossHere == true)
        {
            _startWave = false;
            _spawnEnemy = false;
        }
        else
        {
            _startWave = true;        
            _spawnEnemy = true;

            if(_boss == null)
            {
                if(_restart == false)
                {
                    _restart = true;
                    _uiManager.AddScore(100);

                    StartCoroutine(EnemyWave());
                    StartCoroutine(SpawnEnemyRoutine());
                }              
            }         
        }
    }


    public void StartSpawning()
    {
        StartCoroutine(SpawnPowerupRoutine());
        StartCoroutine(SpawnEnemyRoutine());
        StartCoroutine(SpawnCollectablesRoutine());
        StartCoroutine(EnemyWave());
        _uiManager.ChangeWaveCount(_waveNumber);
    }

    
    public IEnumerator EnemyWave()
    {
        while(_startWave == true)
        {
            yield return new WaitForSeconds(20f);
            _spawnTime /= 1.2f;
            _waveNumber += 1;
            _uiManager.ChangeWaveCount(_waveNumber);

            if(_waveNumber == 5)
            {
                _bossHere = true;
                _boss.SetActive(true);
                _bossHPBar.SetActive(true);

                leftOverEnemies = GameObject.FindGameObjectsWithTag("Enemy");
                foreach (GameObject e in leftOverEnemies)
                {
                    if(e.name.Contains("Boss"))
                    {
                        //do nothing
                    }
                    else
                    {
                        Destroy(e);
                    }                 
                }

                _startWave = false;
            }
        }
    }

    public IEnumerator SpawnEnemyRoutine()
    {
        yield return new WaitForSeconds(2f);

        while(_spawnEnemy == true)
        {
            float randomPosX = Random.Range(-5f, 6f);

            float cumulativeTotal = 0;
            float randomValue = Random.Range(0, 100);

            for (int i = 0; i <= _enemyWeights.Length; i++)
            {
                float enemyChance = (100 / _totalEnemyWeight) * _enemyWeights[i];
    
                cumulativeTotal += enemyChance;

                if (cumulativeTotal >= randomValue)
                {
                    GameObject enemy = Instantiate(_enemyPrefab[i], new Vector3(randomPosX, 7, 0), Quaternion.identity);
                    enemy.transform.parent = _enemyContainer.transform;
                    break;
                }
            }

            yield return new WaitForSeconds(_spawnTime);
        }
    }
    
    public IEnumerator SpawnPowerupRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_spawnPowerup == true)
        {
            float randomPosX = Random.Range(-5f, 6f);

            float cumulativeTotal = 0;
            float randomValue = Random.Range(0, 100); 

            for (int i = 0; i <= _powerupWeights.Length; i++) 
            {               
                //totalWeight is now 47
                float powerupChance = (100 / _totalWeight) * _powerupWeights[i];
                //for tripleshot: 2.13 * 10 = 21.3%
                //for speed: 2.13 * 10 = 21.3% 
                //for shield: 2.13 * 10 = 21.3%
                //for homing: 2.13 * 5 = 10.65%
                //for bug: 2.13 * 12 = 25.56%

                cumulativeTotal += powerupChance;          

                if (cumulativeTotal >= randomValue)
                {
                    GameObject powerup = Instantiate(_powerUps[i], new Vector3(randomPosX, 7, 0), Quaternion.identity);
                    break;
                }
            }
            

            float randomTime = Random.Range(3, 8);
            yield return new WaitForSeconds(randomTime);
        }    
    }

    public IEnumerator SpawnCollectablesRoutine()
    {
        yield return new WaitForSeconds(2f);

        while (_spawnCollectable == true)
        {
            float randomPosX = Random.Range(-5f, 6f);

            float hpChance = .2f;
            float ammoChance = .8f;

            float collectableChance = Random.Range(0, hpChance + ammoChance);
            if(collectableChance < hpChance)
            {
                GameObject HP = Instantiate(_collectables[1], new Vector3(randomPosX, 7, 0), Quaternion.identity);
            }
            else if(collectableChance < hpChance + ammoChance && collectableChance > hpChance)
            {
                GameObject Ammo = Instantiate(_collectables[0], new Vector3(randomPosX, 7, 0), Quaternion.identity);
            }

            /*
            float[,] collectableChanceTable = { { .2f, 1f }, { .8f, 0f } };
            float cumulativeWeight = 0;
            for(int i = 0; i < collectableChanceTable.Length; i++)
            {
                cumulativeWeight += i;
                if(collectableChance < cumulativeWeight)
                {
                    float value = collectableChanceTable[0 , i];
                }
            }
            */

            /*
            int rareCollectableChance = Random.Range(0, 10);
            if (rareCollectableChance >= 0 && rareCollectableChance < 2)
            {
                GameObject HP = Instantiate(_collectables[1], new Vector3(randomPosX, 7, 0), Quaternion.identity);
            }
            else if(rareCollectableChance >= 2 && rareCollectableChance <= 10)
            {
                GameObject Ammo = Instantiate(_collectables[0], new Vector3(randomPosX, 7, 0), Quaternion.identity);
            }     
            */

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
        _spawnCollectable = false;
        
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
