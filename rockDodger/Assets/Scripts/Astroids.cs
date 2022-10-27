using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Astroids : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        PlayerHealth hp = other.GetComponent<PlayerHealth>();
        if(hp == null){ return; }
        hp.Crash();
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
