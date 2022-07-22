using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private SpawnManager _spawnManager;
    private GameManager _gameManager;

    [SerializeField]
    private GameObject _laserPrefab;
    [SerializeField]
    private GameObject _shieldVisual;
    [SerializeField]
    private GameObject _tripleShotPrefab;
    
    [SerializeField]
    private float _fireRate = 0.15f;
    private float _canFire = -1f;

    [SerializeField]
    public int _lives = 3;

    [SerializeField]
    private float _speed = 5f;
    [SerializeField]
    private float _speedBoostMultiplier = 2f;

    private bool _isTripleShot = false;
    private bool _isShield = false;

    [SerializeField]
    private GameObject _rightEngineFire;
    [SerializeField]
    private GameObject _leftEngineFire;

    [SerializeField]
    private AudioSource _laserAudio;
    [SerializeField]
    private AudioSource _explosionAudio;
    [SerializeField]
    private GameObject _explosion;

    [SerializeField]
    private GameObject _thruster;
    private Color _thrusterCol;

    private UIManager _uiManager;

    private bool _speedUp = false;

    void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
        if (_spawnManager == null)
        {
            Debug.LogError("The spawn manager is null");
        }

        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("The Game manager is null");
        }

        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        if (_uiManager == null)
        {
            Debug.LogError("UI manager is null");
        }

        transform.position = new Vector3(0, -2f, 0);

        _thrusterCol = _thruster.GetComponent<SpriteRenderer>().color;
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
        if (transform.position.x >= 6.5f)
        {
            transform.position = new Vector3(-6.5f, transform.position.y, transform.position.z);
        }
        else if (transform.position.x <= -6.5f)
        {
            transform.position = new Vector3(6.5f, transform.position.y, transform.position.z);
        }


        //increasing our speed with left shift
        if(Input.GetKeyDown(KeyCode.LeftShift))
        {
            _speedUp = true;
            _speed *= _speedBoostMultiplier;                      
        }
        else if(Input.GetKeyUp(KeyCode.LeftShift))
        {
            _speedUp = false;
            _speed /= _speedBoostMultiplier;         
        }

        ChangeThruster();
    }

    void ChangeThruster()
    {
        if(_speedUp == true)
        {
            _thrusterCol.a = Mathf.Lerp(_thrusterCol.a, 1f, .1f);
        }
        else if(_speedUp == false)
        {
            _thrusterCol.a = Mathf.Lerp(_thrusterCol.a, .5f, .1f);
        }

        _thruster.GetComponent<SpriteRenderer>().color = _thrusterCol;
    }

    void FireLaser()
    {
        _canFire = Time.time + _fireRate;
        Instantiate(_laserPrefab, new Vector3(this.transform.position.x, this.transform.position.y + 1.05f), Quaternion.identity);

        if(_isTripleShot == true)
        {
            Instantiate(_tripleShotPrefab, new Vector3(this.transform.position.x, this.transform.position.y), Quaternion.identity);
        }

        _laserAudio.Play();
    }

    public void PowerupActive(int powerupID)
    {
        
        switch (powerupID)
        {
            case 0:
                _isTripleShot = true;
                break;
            case 1:
                _speed *= _speedBoostMultiplier;
                break;
            case 2:
                _isShield = true;
                _shieldVisual.SetActive(true);
                break;
            default:
                Debug.Log("No powerup");
                break;
        }       

        StartCoroutine(PowerupCooldown(powerupID));           
    }

    public IEnumerator PowerupCooldown(int powerupID)
    {
        float waitTime = 0f;

        if(powerupID == 0)
        {
            waitTime = 5f;
        }
        else if(powerupID == 1)
        {
            waitTime = 3f;
        }

        yield return new WaitForSeconds(waitTime);

        switch (powerupID)
        {
            case 0:
                _isTripleShot = false;
                break;
            case 1:
                _speed /= _speedBoostMultiplier;
                break;
        }
        
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag == "Laser")
        {
            if (other.GetComponent<Laser>().enemyLaser == false) return;
            Destroy(other.gameObject);

            GameObject explosion = Instantiate(_explosion, this.transform.position, Quaternion.identity);
            Destroy(explosion, 3f);
            _explosionAudio.Play();

            Damage();
            _uiManager.ChangeLives(_lives);
        }
    }

    public void Damage()
    {
        if(_isShield == false)
        {
            _lives -= 1;
        }
        else if(_isShield == true)
        {
            _shieldVisual.SetActive(false);
            _isShield = false;         
        }
        

        if(_lives == 2)
        {
            _rightEngineFire.SetActive(true);
        }
        else if(_lives == 1)
        {
            _leftEngineFire.SetActive(true);
        }
        else 
        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            _gameManager.GameOver();
            _explosionAudio.Play();
            Destroy(this.gameObject);
        }
    }

}
