using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private PlayerMoveController moveController;
    [SerializeField] private GameObject platMan;
    [SerializeField] private RectTransform HP;
    private float maxHP;
    [SerializeField] private GameObject[] UI;
    [SerializeField] private AudioClip hurtSound;
    [SerializeField] private AudioClip iceSound;
    [SerializeField] private AudioClip toolSound;
    [SerializeField] private float Damage;
    private Rigidbody2D rb;
    private bool validHurt = true;
    private bool validBreak = false;
    private float rate; //HP coverse rate
    private float bounceForce;
    private float healTime;
    [SerializeField] private float healValue;
    public bool gameOver;
    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 0f;
        gameOver = false;
        healTime = 0;
        bounceForce = 500f;
        HP.GetComponent<RectTransform>();
        maxHP = HP.sizeDelta.x;
        rate = HP.sizeDelta.x / 100;
        rb = transform.GetComponent<Rigidbody2D>();
    }
    // Update is called once per frame
    void Update()
    {
        if(!Physics2D.OverlapCircle(new Vector2 (transform.position.x, transform.position.y - 0.5f), 0.4f, 1<<6)){
            validHurt = true;
            validBreak = true;
        }
        if ((Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Space))){
            moveController.Jump();   
            GetComponent<Animator>().SetBool("jump", true);
        }
        else if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.Space)){
            moveController.StopJump();
        }
        //through the wall
        if(transform.position.x < 1.8f)
            transform.position = new Vector3(10.2f, transform.position.y, 0);
        else if(transform.position.x > 10.2f)
            transform.position = new Vector3(1.8f, transform.position.y, 0);
        if(transform.position.y <= -1 && !gameOver){
            AudioSource tmp = GetComponent<AudioSource>();
            tmp.clip = hurtSound;
            tmp.volume = 0.8f;
            tmp.Play();
            GameOver();
        }
        
        if(gameOver == false)
            healLife();
    }
    void FixedUpdate(){
        moveController.Move(Input.GetAxisRaw("Horizontal"));
    }
    
    private void OnCollisionEnter2D(Collision2D other) {
        if(other.gameObject.tag == "trap" && validHurt && other.contacts[0].normal == new Vector2(0f, 1f)){
            validHurt = false;
            AudioSource tmp = GetComponent<AudioSource>();
            tmp.clip = hurtSound;
            tmp.volume = 0.8f;
            tmp.Play();
            Hurt();
        }
        else if(other.gameObject.tag == "bounce" && other.contacts[0].normal == new Vector2(0f, 1f)){
            rb.AddForce(transform.up * bounceForce);
            GetComponent<Animator>().SetBool("jump", true);
            other.gameObject.GetComponent<Animator>().SetTrigger("bounce");
            other.gameObject.GetComponent<AudioSource>().Play();
        }
        else if(other.gameObject.tag == "ice" && other.contacts[0].normal == new Vector2(0f, 1f)){
            AudioSource tmp = GetComponent<AudioSource>();
            tmp.clip = iceSound;
            tmp.volume = 0.4f;
            if(other.gameObject.GetComponent<Platform>().counts >= 1 && validBreak){
                tmp.Play();
                other.gameObject.SetActive(false);
            }
            other.gameObject.GetComponent<Platform>().counts += 1;
            validBreak = false;
            other.gameObject.GetComponent<Animator>().SetTrigger("ice");
            if(other.gameObject.GetComponent<Platform>().counts == 1){
                tmp.Play();
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D other) {
        if(other.gameObject.tag == "clock"){
            AudioSource tmp = GetComponent<AudioSource>();
            tmp.clip = toolSound;
            tmp.volume = 1f;
            tmp.Play();
            Destroy(other.gameObject);
            GetComponent<PlayerMoveController>().moveSpeed -= 0.5f;
        }
    }
    private void Hurt(){
        GetComponent<Animator>().SetTrigger("hurt");
        float target = HP.sizeDelta.x - (Damage * rate);
        if(target >= 0)
            HP.sizeDelta = new Vector2 (target, HP.sizeDelta.y);
        else{
            GameOver();
        }
    }
    
    private void healLife(){
        healTime += Time.deltaTime;
        if(healTime >= 1f)
        {
            healTime = 0f;
            float target = HP.sizeDelta.x + (healValue * rate);
            if(target <= maxHP)
                HP.sizeDelta = new Vector2 (target, HP.sizeDelta.y);
            else
                HP.sizeDelta = new Vector2 (maxHP, HP.sizeDelta.y);
        }
    }
    private void GameOver()
    {
        gameOver = true;
        HP.sizeDelta = new Vector2 (0, HP.sizeDelta.y);
        platMan.GetComponent<AudioManager>().gameOverPlay();
        Time.timeScale = 0f;
        UI[1].SetActive(true);
    }
    public void Replay()
    {
        Time.timeScale = 1f;
        UnityEngine.SceneManagement.SceneManager.LoadScene("SampleScene");
    }
    public void Play()
    {
        Time.timeScale = 1f;
        UI[0].SetActive(false);
    }
}
