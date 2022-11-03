using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MisilleBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(deleteBullet());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        Astroids asteroids = other.GetComponent<Astroids>();
        if (asteroids == null) { return; }
        asteroids.Destroyed();
        Destroy(gameObject);
    }

    IEnumerator deleteBullet()
    {
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
