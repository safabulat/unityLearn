using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindTime : MonoBehaviour
{
    public bool isRewinding = false;
    List<Vector3> positions;
    void Start()
    {
        positions = new List<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
            StartRewinding();
        if (Input.GetKeyUp(KeyCode.Return))
            StopRewinding();
        
    }

    private void FixedUpdate()
    {
        if (isRewinding)
        {
            Rewind();
        }
        else
        {
            Record();
        }
    }
    public void StartRewinding()
    {
        isRewinding = true;
    }
    public void StopRewinding()
    {
        isRewinding = false;
    }
    public void Rewind()
    {
        transform.position = positions[0];
        positions.RemoveAt(0);
    }
    public void Record()
    {
        positions.Insert(0, transform.position);
    }

}
