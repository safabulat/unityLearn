using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationTry : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(UpdateXSeconds), 0.3f, 1f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(1f, 1f, 1f);
    }

    void UpdateXSeconds()
    {

    }
}
