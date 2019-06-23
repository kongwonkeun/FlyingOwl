using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sky : MonoBehaviour
{
    float speed = 0.03f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float offset = speed * Time.time;
        GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(offset, 0);
    }
}
