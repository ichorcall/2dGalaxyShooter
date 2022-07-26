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

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UIManager>();
        _explosionAudio = GameObject.Find("ExplosionAudio").GetComponent<AudioSource>();


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

        StartCoroutine(FireLaser());
    }
    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.up * Time.deltaTime * _speed);

        if(transform.position.y < -6)
        {
            _randomPosX = Random.Range(-6.5f, 6.5f);
            transform.position = new Vector3(_randomPosX, 7, 0);
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

            _fireLaser = false;
            Destroy(other.gameObject);

            _anim.SetTrigger("OnEnemyDeath");
            _coll.enabled = false;

            _explosionAudio.Play();

            Debug.Log("DEBUG TEST: Enemy has been destroyed!");

            _speed = 0;
            Destroy(this.gameObject, 2f);
            _uiManager.AddScore(10);           
        }

        if(other.gameObject.tag == "Player")
        {
            _fireLaser = false;
            Player player = other.gameObject.GetComponent<Player>();
            
            if(player != null)
            {
                player.Damage(false);
                _uiManager.ChangeLives(player._lives);
            }

            _anim.SetTrigger("OnEnemyDeath");
            _coll.enabled = false;

            _explosionAudio.Play();

            _speed = 0;
            Destroy(this.gameObject, 2f);
        }
    }
}
