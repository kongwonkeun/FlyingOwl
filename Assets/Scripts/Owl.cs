using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Owl : MonoBehaviour
{
    public Transform branch;
    public Transform bird;
    public Transform gift;
    public AudioClip gameOver;
    public GUISkin skin;

    Transform spawnPoint;
    Transform newBranch;
    int speedSide = 8;
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
        if (isDead) {
            return;
        }
        CheckBranch();
        MoveOwl();
        SetCamera();
        MakeBirdAndGift();
        score = maxY * 1000 - birdCount * 1000 + giftScore;
    }

    private void OnTriggerEnter(Collider colli) {
        if (colli.tag == "BIRD") {
            if (colli.transform.eulerAngles.z != 0) {
                return;
            }
            birdCount++;
            colli.SendMessage("DropBird", SendMessageOptions.DontRequireReceiver);
        }
        if (colli.tag.Contains("GIFT")) {
            int n = int.Parse(colli.tag.Substring(4, 1));
            giftScore = n * 500;
            giftCount++;
            colli.SendMessage("GetGift", SendMessageOptions.DontRequireReceiver);
        }
    }

    private void OnGUI() {
        GUI.skin = skin;
        float x1 = 20;
        float x2 = Screen.width / 2 - 50;
        float x3 = Screen.width - 100;
        int size = (isMobile) ? 36 : 20;
        // string s1 = string.Format("<color=navy><size={0:0}>Score : {1:#,0}</size></color>", size, score);
        // string s2 = string.Format("<color=red><size={0:0}>Bird : {1:#,0}</size></color>", size, birdCount);
        // string s3 = string.Format("<color=#004000><size={0:0}>Gift : {1:#,0}</size></color>", size, giftCount);
        string s1 = string.Format("<size={0:0}>Score : {1:#,0}</size>", size, score);
        string s2 = string.Format("<size={0:0}>Bird : {1:#,0}</size>", size, birdCount);
        string s3 = string.Format("<size={0:0}>Gift : {1:#,0}</size>", size, giftCount);
        // GUI.Label(new Rect(x1, 10, 200, 50), s1);
        // GUI.Label(new Rect(x2, 10, 200, 50), s2);
        // GUI.Label(new Rect(x3, 10, 200, 50), s3);
        OutlineText(x1, 20, s1, "navy");
        OutlineText(x2, 20, s2, "red");
        OutlineText(x3, 20, s3, "#004000");
        if (!isDead) {
            return;
        }
        if (Camera.main.GetComponent<AudioSource>().clip != gameOver) {
            Camera.main.GetComponent<AudioSource>().clip = gameOver;
            Camera.main.GetComponent<AudioSource>().loop = false;
            Camera.main.GetComponent<AudioSource>().Play();
        }
        Cursor.visible = true;
        float x = Screen.width / 2;
        float y = Screen.height / 2;
        if (GUI.Button(new Rect(x - 80, y - 50, 160, 50), "Play Again")) {
            SceneManager.LoadScene("scene_1");
        }
        if (GUI.Button(new Rect(x - 80, y + 50, 160, 50), "Quit Game?")) {
            Application.Quit();
        }
    }

    void CheckBranch() {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 0.45f)) {
            if (hit.transform.tag == "BRANCH") {
                dir.y = speedJump;
                hit.transform.GetComponent<AudioSource>().Play();
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

    void MakeBirdAndGift() {
        if (Random.Range(0, 1000) < 970) {
            return;
        }
        Vector3 pos = new Vector3(0, 0, 0.3f);
        pos.y = maxY + Random.Range(2f, 4f);
        if (Random.Range(0, 100) < 50) {
            pos.x = -10;
            Instantiate(bird, pos, Quaternion.identity);
        } else {
            int n1 = GameObject.FindGameObjectsWithTag("GIFT1").Length;
            int n2 = GameObject.FindGameObjectsWithTag("GIFT2").Length;
            int n3 = GameObject.FindGameObjectsWithTag("GIFT3").Length;
            if (n1 + n2 + n3 >= 5) {
                return;
            }
            pos.x = Random.Range(-4f, 4f);
            pos.y = maxY + Random.Range(3f, 4f);
            pos.z = 0.4f;
            Transform obj = Instantiate(gift, pos, Quaternion.identity) as Transform;
            int n = Random.Range(1, 4);
            SpriteRenderer render = obj.GetComponent<SpriteRenderer>();
            render.sprite = Resources.Load<Sprite>("Images/gift" + n);
            obj.tag = "GIFT" + n;
        }
    }

    void OutlineText(float x, float y, string text, string color) {
        string str = string.Format("<color=white>{0:a}</color>", text);
        GUI.Label(new Rect(x - 2, y, 300, 50), str);
        GUI.Label(new Rect(x, y - 2, 300, 50), str);
        GUI.Label(new Rect(x + 2, y, 300, 50), str);
        GUI.Label(new Rect(x, y + 2, 300, 50), str); 
        str = string.Format("<color={1:a}>{0:a}</color>", text, color);
        GUI.Label(new Rect(x, y, 300, 50), str);
    }

}
