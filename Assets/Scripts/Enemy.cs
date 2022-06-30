using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private float _speed = 4f;
    private float _randomPosX;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(-Vector3.up * Time.deltaTime * _speed);

        if(transform.position.y < -6)
        {
            _randomPosX = Random.Range(-9f, 9f);
            transform.position = new Vector3(_randomPosX, 7, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Laser")
        {
            Destroy(other.gameObject);
            Destroy(this.gameObject);
        }

        if(other.gameObject.tag == "Player")
        {
            Player player = other.gameObject.GetComponent<Player>();
            
            if(player != null)
            {
                player.Damage();
            }

            Destroy(this.gameObject);
        }
    }
}
