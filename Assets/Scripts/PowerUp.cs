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
    [SerializeField]
    private AudioSource _powerupAudio;

    private void Start()
    {
        _powerupAudio = GameObject.Find("PowerupAudio").GetComponent<AudioSource>();
        if (_powerupAudio == null) Debug.LogError("Powerup audio is null");
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);

        if(transform.position.y <= -6)
        {
            Destroy(this.gameObject);
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
    }
}
