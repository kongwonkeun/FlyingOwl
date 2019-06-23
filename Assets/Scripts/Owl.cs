using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : MonoBehaviour
{
    public Transform branch;
    Transform spawnPoint;
    Transform newBranch;
    int speedSide = 6;
    int speedJump = 14;
    int gravity = 24;
    Vector3 dir = Vector3.zero;
    float maxY = 0;
    float score = 0;
    int birdCount = 0;
    int giftCount = 0;
    int giftScore = 0;
    bool isDead = false;
    bool isMobile = false;

    // Start is called before the first frame update
    void Start()
    {
        isMobile = Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
        Cursor.visible = false; // Screen.showCursor = false;
        spawnPoint = GameObject.Find("SpawnPoint").transform;
        newBranch = Instantiate(branch, spawnPoint.position, Quaternion.identity) as Transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead) return;
        CheckBranch();
        MoveOwl();
        SetCamera();
    }

    void CheckBranch() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.45f)) {
            if (hit.transform.tag == "BRANCH") {
                dir.y = speedJump;
            }
        }
    }

    void MoveOwl() {
        Vector3 view = Camera.main.WorldToScreenPoint(transform.position);
        if (view.y < -30) {
            isDead = true;
            return;
        }
        dir.x = 0;
        if (isMobile) {
            //
        } else {
            float key = Input.GetAxis("Horizontal");
            if ((key < 0 && view.x > 30) || (key > 0 && view.x < Screen.width -30)) {
                dir.x = key * speedSide;
            }
        }
        dir.y -= gravity * Time.deltaTime;
        transform.Translate(dir * Time.smoothDeltaTime);

        int n = (dir.y > 0) ? 2 : 1;
        GetComponent<MeshRenderer>().material.mainTexture = Resources.Load("Images/owl" + n) as Texture2D;
    }

    void SetCamera() {
        if (transform.position.y > maxY) {
            maxY = transform.position.y;
            Camera.main.transform.position = new Vector3(0, maxY - 1.6f, -5);
        }
        float dist = spawnPoint.position.y - newBranch.position.y;
        if (dist >= 3) {
            Vector3 pos = spawnPoint.position;
            pos.x = Random.Range(-7f, 7f) * 0.5f;
            newBranch = Instantiate(branch, pos, Quaternion.identity) as Transform;
            int x = (Random.Range(0, 2) == 0) ? -1 : 1;
            int y = (Random.Range(0, 2) == 0) ? -1 : 1;
            newBranch.GetComponent<MeshRenderer>().material.mainTextureScale = new Vector2(x, y);
            newBranch.localScale = new Vector3(Random.Range(1.2f, 2), 0.7f, 1);
        }
    }

}
