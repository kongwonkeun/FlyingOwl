using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bird : MonoBehaviour
{
    public Transform score;
    int imgCount = 6;
    int imgNum = 0;
    int imgPerSec = 0;
    float imgDelay = 0;
    float speed = 0;
    bool isDead = false;

    // Start is called before the first frame update
    void Start()
    {
        imgPerSec = Random.Range(10, 19);
        imgDelay = 1f / imgPerSec;
        speed = Random.Range(3f, 5f);
    }

    // Update is called once per frame
    void Update()
    {
        float distance = speed * Time.smoothDeltaTime;
        if (!isDead) {
            AnimationBird();
            transform.Translate(Vector3.right * distance);
        } else {
            transform.Translate(Vector3.down * distance, Space.World);
        }
        Vector3 view = Camera.main.WorldToScreenPoint(transform.position);
        if (view.x > Screen.width + 30 || view.y < -30) {
            Destroy(gameObject);
        }
    }

    void AnimationBird() {
        imgDelay -= Time.deltaTime;
        if (imgDelay > 0) return;
        imgNum = (int)Mathf.Repeat(++imgNum, imgCount);
        float offset = 1f / imgCount * imgNum;
        transform.GetComponent<MeshRenderer>().material.mainTextureOffset = new Vector2(offset, 0);
        imgDelay = 1f / imgPerSec;
    }

    void DropBird() {
        GetComponent<AudioSource>().Play();
        GameObject canvas = GameObject.Find("Canvas");
        Transform obj = Instantiate(score) as Transform;
        obj.transform.SetParent(canvas.transform);
        obj.GetComponent<Text>().text = "-1,000";
        obj.GetComponent<Text>().color = Color.red;
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        obj.position = pos;
        transform.eulerAngles = new Vector3(0, 0, 180);
        isDead = true;
    }

}
