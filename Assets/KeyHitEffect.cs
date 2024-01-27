using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyHitEffect : MonoBehaviour
{

    public float lifeTime = .15f;
    public float startTime;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(startTime + lifeTime < Time.time)
        {
            Destroy(gameObject);
        }
    }
}
