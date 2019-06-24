using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    int fontSize = 16;
    float speed = 0.05f;

    // Start is called before the first frame update
    void Start()
    {
        if (Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer) {
            fontSize = 30;
        }
        transform.GetComponent<Text>().fontSize = fontSize;
        StartCoroutine("Fadeout");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = speed * Time.deltaTime;
        transform.Translate(Vector3.up * distance);
    }

    IEnumerator Fadeout() {
        yield return new WaitForSeconds(0.5f);
        for (float a = 1; a >= 0; a -= 0.02f) {
            transform.GetComponent<Text>().material.color = new Vector4(1, 1, 1, a);
            yield return new WaitForFixedUpdate();
        }
        Destroy(gameObject);
    }

}
