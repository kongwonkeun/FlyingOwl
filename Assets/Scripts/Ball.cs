using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject b;
    float v = 50;
    float th = 60;
    float t = 0;
    float g = 9.80665f;
    float h = 0;
    float d = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (h < 0) return;
        float r = th * Mathf.Deg2Rad;
        h = v * t * Mathf.Sin(r) - 0.5f * g * t * t;
        d = v * t * Mathf.Cos(r);
        Vector3 pos = new Vector3(d, h, 0);
        Instantiate(b, pos, Quaternion.identity);
        t += 0.1f;
    }

}
