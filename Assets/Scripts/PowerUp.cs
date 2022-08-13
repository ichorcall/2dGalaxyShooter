using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    [SerializeField]
    private float _speed = 3f;

    [SerializeField]
    private int _powerupID;
    //0 tripleshot
    //1 speed
    //2 shield
    //3 ammo
    //4 hp
    //5 homing
    [SerializeField]
    private AudioSource _powerupAudio;

    [SerializeField]
    private AudioSource _explosionAudio;
    [SerializeField]
    private GameObject _explosion;

    private Player _player;
    private bool _moveToPlayer = false;

    [SerializeField]
    private GameObject _nameSprite;

    private void Start()
    {
        _powerupAudio = GameObject.Find("PowerupAudio").GetComponent<AudioSource>();
        if (_powerupAudio == null) Debug.LogError("Powerup audio is null");

        _explosionAudio = GameObject.Find("ExplosionAudio").GetComponent<AudioSource>();
        if (_explosionAudio == null) Debug.LogError("_explosionAudio is null");

        _player = FindObjectOfType<Player>();
        if (_player == null) Debug.LogError("player is null");
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -6)
        {
            Destroy(this.gameObject);
        }

        if(Input.GetKeyDown(KeyCode.C))
        {
            _moveToPlayer = true;
        }

        if(_moveToPlayer == true)
        {
            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, _speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            Player player = other.transform.GetComponent<Player>();
            if(player != null)
            {
                player.PowerupActive(_powerupID);
            }

            _powerupAudio.Play();

            Destroy(this.gameObject);
        }

        if(other.gameObject.tag == "Laser")
        {
            if (other.gameObject.GetComponent<Laser>().enemyLaser == false) return;

            GameObject explosion = Instantiate(_explosion, this.transform.position, Quaternion.identity);
            Destroy(explosion, 2f);
            _explosionAudio.Play();

            _speed = 0;
            
            if(_nameSprite != null)
            {
                _nameSprite.GetComponent<SpriteRenderer>().enabled = false;
            }

            GetComponent<SpriteRenderer>().enabled = false;

            GetComponent<CircleCollider2D>().enabled = false;
            Destroy(this.gameObject, 2f);
        }
    }
}
