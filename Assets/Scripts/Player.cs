using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpawnManager _spawnManager;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;
    [SerializeField]
    private int _lives = 3;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is null");
        }


        transform.position = new Vector3(0, -2f, 0);
    }

    void Update()
    {
        CalculateMovement();

        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            FireLaser();
        }

    }


    //game running for 1 second. 1+.5 = 1.5 canfire. AT THIS VERY MOMENT, canfire is GREATER (1.5) than gametime (1 second), but wait .6 seconds later, canfire is 1.5 seconds old, and gametime is 1.6 seconds old, which means we can fire again. 
    void CalculateMovement()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(horizontalInput, verticalInput);
        transform.Translate(dir * Time.deltaTime * _speed);

        //for y:
        if (transform.position.y >= 0f)
        {
            transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
        }
        else if (transform.position.y <= -3.8f)
        {
            transform.position = new Vector3(transform.position.x, -3.8f, transform.position.z);
        }

        //transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, -3.8f, 0), 0);

        //for x:
        if (transform.position.x >= 11.5f)
        {
            transform.position = new Vector3(-11.5f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -11.5f)
        {
            transform.position = new Vector3(11.5f, transform.position.y, transform.position.z);
        }
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, new Vector3(this.transform.position.x, this.transform.position.y + 1.05f), Quaternion.identity);

    }

    public void Damage()
    {
        _lives -= 1;

        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            Destroy(this.gameObject);
        }
    }
}
