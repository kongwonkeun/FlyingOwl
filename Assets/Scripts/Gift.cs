using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Gift : MonoBehaviour
{
    public Transform score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 view = Camera.main.WorldToScreenPoint(transform.position);
        if (view.y < -30) Destroy(gameObject);
    }

    void GetGift() {
        GetComponent<AudioSource>().Play();
        int n = int.Parse(transform.tag.Substring(4, 1));
        GameObject canvas = GameObject.Find("Canvas");
        Transform obj = Instantiate(score) as Transform;
        obj.transform.SetParent(canvas.transform);
        obj.GetComponent<Text>().text = string.Format("{0:+#,0}", n * 500);
        obj.GetComponent<Text>().color = new Vector4(0, 0.3f, 0, 1);
        Vector3 pos = Camera.main.WorldToScreenPoint(transform.position);
        obj.position = pos;
        Destroy(gameObject);
    }

}
