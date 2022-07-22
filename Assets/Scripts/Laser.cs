using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _speed = 8f;

    
    public bool enemyLaser;
    // Update is called once per frame
    void Update()
    {
        if(enemyLaser == false)
        {
            PlayerLaser();
        }
        else if(enemyLaser == true)
        {
            EnemyLaser();
        }
                   
    }

    void PlayerLaser()
    {
        transform.Translate(Vector3.up * Time.deltaTime * _speed);

        if (this.transform.position.y >= 8)
        {
            if (this.transform.parent != null) //using .gameObject gives nullreferenceexception
            {
                var parentObj = this.transform.parent;
                Destroy(parentObj.gameObject);
            }

            Destroy(this.gameObject);
        }
    }

    void EnemyLaser()
    {
        transform.Translate(Vector3.down * Time.deltaTime * _speed);

        if (this.transform.position.y <= -8)
        {
            if (this.transform.parent != null) //using .gameObject gives nullreferenceexception
            {
                var parentObj = this.transform.parent;
                Destroy(parentObj.gameObject);
            }

            Destroy(this.gameObject);
        }
    }
}


