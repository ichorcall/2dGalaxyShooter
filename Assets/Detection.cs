using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detection : MonoBehaviour
{

    public bool playerDetected = false;
    public bool laserDetected = false;
    public GameObject incomingLaser;
    public void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            playerDetected = true;
        }

        if(other.gameObject.tag == "Laser")
        {
            if (other.GetComponent<Laser>().enemyLaser == true) return;
            {
                laserDetected = true;
                incomingLaser = other.gameObject;
            }
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            playerDetected = false;
        }

        if (other.gameObject.tag == "Laser")
        {
            if (other.GetComponent<Laser>().enemyLaser == true) return;
            {
                laserDetected = false;
            }
        }
    }

}
