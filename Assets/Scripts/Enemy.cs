using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 4f;
    private float _randomPosX;

    private UIManager _uiManager;

    private Animator _anim;
    private BoxCollider2D _coll;

    [SerializeField]
    private AudioSource _explosionAudio;

    [SerializeField]
    private GameObject _laserPrefab;
    private bool _fireLaser = true;

    [SerializeField]
    private int _enemyID;
    //0 normal
    //1 laser
    public bool relocate = false;
    [SerializeField]
    private GameObject _parent;
    [SerializeField]
    private GameObject _laserbeamColPrefab;
    [SerializeField]
    private GameObject[] _laserbeams;
    private GameObject _laserbeamCol;
    public bool spawnLaser = false;
    private bool _stopLaserMovement = false;
    [SerializeField]
    private GameObject _explosion;

    [SerializeField]
    private GameObject _enemyShield;

    [SerializeField]
    private GameObject _detectionPrefab;
    private GameObject _detection;
    [SerializeField]
    private GameObject _sparks;
    private Player _player;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _explosionAudio = GameObject.Find("ExplosionAudio").GetComponent<AudioSource>();

        if(_enemyID == 1)
        {
            _fireLaser = false;
        }
        else if(_enemyID == 0)
        {
            StartCoroutine(FireLaser());

            //enemy shields
            int randomShieldChance = Random.Range(0, 10);
            if(randomShieldChance >= 0 && randomShieldChance < 5)
            {
                _enemyShield.SetActive(true);
            }
        }
        else if(_enemyID == 2)
        {
            _detection = Instantiate(_detectionPrefab);
        }

        if(_uiManager == null)
        {
            Debug.LogError("UI manager is null");
        }

        if(_explosionAudio == null)
        {
            Debug.LogError("Explosion audio is null");
        }

        _anim = this.GetComponent<Animator>();
        if(_anim == null)
        {
            Debug.LogError("Anim in null");
        }

        _coll = this.GetComponent<BoxCollider2D>();
        if (_coll == null)
        {
            Debug.LogError("coll in null");
        }

        _player = FindObjectOfType<Player>();
        if (_player == null)
        {
            Debug.LogError("player in null");
        }

    }
    // Update is called once per frame
    void Update()
    {
        switch (_enemyID)
        {
            case 0:
                EnemyMovement();
                break;
            case 1:
                if(_stopLaserMovement == false)
                {
                    LaserEnemyMovement();
                }
                break;
            case 2:
                AggroMovement();
                EnemyMovement();
                break;
        }
    }

    public void AggroMovement()
    {
        _detection.transform.position = this.transform.position;

        if (_detection.GetComponent<Detection>().playerDetected == true)
        {
            Vector3 targetDir = _player.gameObject.transform.position - transform.position;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg + 90f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), .02f);

            float distance = Vector2.Distance(_player.transform.position, this.transform.position);

            if(distance < 5)
            {
                _speed = 10 - distance;
            }
        }
        else if(_detection.GetComponent<Detection>().playerDetected == false)
        {
            Quaternion normalRot = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
            transform.rotation = Quaternion.Lerp(transform.rotation, normalRot, .02f);
            _speed = 4f;
        }        
    }

    public void EnemyMovement()
    {
        transform.Translate(-Vector3.up * Time.deltaTime * _speed);

        if (transform.position.y < -6)
        {
            _randomPosX = Random.Range(-6.5f, 6.5f);
            transform.position = new Vector3(_randomPosX, 7, 0);
        }
    }

    public void LaserEnemyMovement()
    {
        if(relocate == true)
        {
            _randomPosX = Random.Range(-6.5f, 6.5f);
            _parent.transform.position = new Vector3(_randomPosX, 7, 0);
            relocate = false;
        }

        if(spawnLaser == true)
        {
            if(_laserbeamCol == null)
            {
                _laserbeamCol = Instantiate(_laserbeamColPrefab, transform.position, Quaternion.identity);
            }
        }
        else if(spawnLaser == false)
        {
            if(_laserbeamCol != null)
            {
                Destroy(_laserbeamCol);
            }
        }
    }

    IEnumerator FireLaser()
    {
        while(_fireLaser == true)
        {
            GameObject laser = Instantiate(_laserPrefab, new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z), Quaternion.identity);
            yield return new WaitForSeconds(Random.Range(1f, 5f));
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Laser")
        {
            if (other.GetComponent<Laser>().enemyLaser == true) return;
            Destroy(other.gameObject);

            if(_enemyID == 0)
            {
                if(_enemyShield.activeSelf == true)
                {
                    _enemyShield.SetActive(false);
                    return;
                }
                else if(_enemyShield.activeSelf == false)
                {
                    _fireLaser = false;
                    _anim.SetTrigger("OnEnemyDeath");
                    Destroy(this.gameObject, 2f);
                }              
            }
            else if(_enemyID == 1)
            {
                _stopLaserMovement = true;

                Destroy(_laserbeamCol);
                foreach(GameObject l in _laserbeams)
                {
                    Destroy(l.gameObject);
                }
                GetComponent<SpriteRenderer>().enabled = false;
            }
            else if(_enemyID == 2)
            {
                _sparks.SetActive(false);
            }

            if(_enemyID == 1 || _enemyID == 2)
            {
                GameObject explosion = Instantiate(_explosion, this.transform.position, Quaternion.identity);
                Destroy(explosion, 2f);

                GetComponent<SpriteRenderer>().enabled = false;

                Destroy(_parent, 2f);
            }
            
            _coll.enabled = false;
            _explosionAudio.Play();
            _speed = 0;
            _uiManager.AddScore(10);           
        }



        if(other.gameObject.tag == "Player")
        {
            if(_enemyID == 0)
            {
                if (_enemyShield.activeSelf == true)
                {
                    _enemyShield.SetActive(false);

                    GameObject explosion = Instantiate(_explosion, this.transform.position, Quaternion.identity);
                    Destroy(explosion, 2f);
                    _explosionAudio.Play();
                }
                else if (_enemyShield.activeSelf == false)
                {
                    _fireLaser = false;

                    _anim.SetTrigger("OnEnemyDeath");
                    _coll.enabled = false;

                    _explosionAudio.Play();

                    _speed = 0;
                    Destroy(this.gameObject, 2f);
                }
            }
            else if(_enemyID == 2)
            {
                GameObject explosion = Instantiate(_explosion, this.transform.position, Quaternion.identity);
                Destroy(explosion, 2f);
                _explosionAudio.Play();

                _sparks.SetActive(false);
                GetComponent<SpriteRenderer>().enabled = false;
                _coll.enabled = false;

                _speed = 0;
                Destroy(this.gameObject, 2f);
            }
            
            Player player = other.gameObject.GetComponent<Player>();

            if (player != null)
            {
                player.Damage(true);
                _uiManager.ChangeLives(player._lives);
            }
        }
    }
}
