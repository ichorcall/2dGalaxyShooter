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
    //0 = normal
    //1 = laser enemy
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

    //make an enemy that comes in side to side from the top, then stops to shoot a laser below. rare enemy
    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _explosionAudio = GameObject.Find("ExplosionAudio").GetComponent<AudioSource>();

        if(_enemyID == 0)
        {
            StartCoroutine(FireLaser());
        }



        if (_uiManager == null)
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

        if (spawnLaser == true)
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

            if (_enemyID == 0)
            {
                _fireLaser = false;

                _anim.SetTrigger("OnEnemyDeath");              
                Destroy(this.gameObject, 2f);               
            }
            else if (_enemyID == 1)
            {
                _stopLaserMovement = true;

                GameObject explosion = Instantiate(_explosion, this.transform.position, Quaternion.identity);
                Destroy(explosion, 2f);

                Destroy(_laserbeamCol);
                foreach (GameObject l in _laserbeams)
                {
                    Destroy(l.gameObject);
                }               
                GetComponent<SpriteRenderer>().enabled = false;          
                
                Destroy(_parent, 2f);              
            }

            _coll.enabled = false;
            _explosionAudio.Play();
            _speed = 0;
            _uiManager.AddScore(10);
        }


        if (other.gameObject.tag == "Player")
        {
            _fireLaser = false;
            Player player = other.gameObject.GetComponent<Player>();
            
            if(player != null)
            {
<<<<<<< Updated upstream
                player.Damage();
=======
                player.Damage(true);
>>>>>>> Stashed changes
                _uiManager.ChangeLives(player._lives);
            }

            if(_enemyID == 0)
            {
                _anim.SetTrigger("OnEnemyDeath");
                _coll.enabled = false;

                _explosionAudio.Play();

                _speed = 0;
                Destroy(this.gameObject, 2f);
            }
            
        }
    }
}
