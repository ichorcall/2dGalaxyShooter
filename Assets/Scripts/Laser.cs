using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Laser : MonoBehaviour
{

    [SerializeField]
    private float _speed = 8f;

    public bool enemyLaser;
    [SerializeField]
    private bool _homingLaser;
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


    public void OnTriggerStay2D(Collider2D other)
    {
        if (_homingLaser == false) return;

        if(other.gameObject.tag == "Enemy")
        {
            Vector3 targetDir = other.gameObject.transform.position - transform.position;
            float angle = Mathf.Atan2(targetDir.y, targetDir.x) * Mathf.Rad2Deg - 90f;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, Vector3.forward), .1f);
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


