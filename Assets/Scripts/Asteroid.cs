using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    [SerializeField]
    private GameObject _explosion;

    private SpawnManager _spawnManager;

    [SerializeField]
    private AudioSource _explosionAudio;
    private void Start()
    {
        _spawnManager = GameObject.Find("SpawnManager").GetComponent<SpawnManager>();
    }
    void Update()
    {
        transform.Rotate(0, 0, 5f * Time.deltaTime);
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Laser")
        {
            this.gameObject.SetActive(false);
            Destroy(other.gameObject);
            GameObject explosion = Instantiate(_explosion, this.transform.position, Quaternion.identity);

            _explosionAudio.Play();

            Destroy(explosion, 3f);
            Destroy(this.gameObject, 3f);

            _spawnManager.StartSpawning();
        }
    }
}
