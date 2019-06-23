using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball_2 : MonoBehaviour
{
    int speedSide = 6;
    int speedJump = 16;
    int gravity = 30;
    Vector3 dir = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {
            dir.y = speedJump;
        }
        Vector3 view = Camera.main.WorldToScreenPoint(transform.position);
        dir.x = 0;
        float key = Input.GetAxis("Horizontal");
        if ((key < 0 && view.x > 30) || (key > 0 && view.x < Screen.width - 30)) {
            dir.x = key * speedSide;
        }

        // dir.x = Input.GetAxis("Horizontal") * speedSide;

        dir.y -= gravity * Time.deltaTime;
        transform.Translate(dir * Time.smoothDeltaTime);
        Vector3 pos = transform.position;
        if (pos.y < -3) {
            pos.y = -3;
            transform.position = pos;
        }
    }

}
