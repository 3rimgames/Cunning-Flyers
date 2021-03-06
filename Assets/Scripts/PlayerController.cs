﻿using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    private Rigidbody2D rb;

    public float Speed = 1f;

    public Looper looper;

    bool moveLeft = false;
    bool moveRight = false;

    public Text scoreText;
    public Text highScoreText;
    public Text coinText;

    public Text goScoreText;
    public Text goHighText;

    public Animator gameOverAnimator;
    public Animator menuAnimator;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody2D>();
        LoadPlayerStats();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameController.control.isDead == true && GameController.control.contPlay == true)
        {
            moveLeft = false;
            moveRight = false;
            GameController.control.isDead = false;
            GameController.control.contPlay = false;
            transform.position = GameController.control.lastPosition;
            transform.rotation = Quaternion.Euler(Vector3.zero);

            gameOverAnimator.SetBool("IsActive", false);
            menuAnimator.SetBool("IsActive", false);

            rb.gravityScale = 0.6f;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetMouseButtonDown(0))
        {
            if (moveLeft)
            {
                moveRight = true;
                moveLeft = false;
            }
            else
            {
                moveRight = false;
                moveLeft = true;
            }
        }        
    }

    void FixedUpdate()
    {
        if (GameController.control.isDead == false)
        {
            if (moveRight)
            {
                rb.AddForce(new Vector2(-0.1f, 0) * Speed);
            }
            if (moveLeft)
            {
                rb.AddForce(new Vector2(0.1f, 0) * Speed);
            }
        }
    }

    //On Collision detection
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Obstacle" && GameController.control.isDead == false)
        {
            GameController.control.isDead = true;

            rb.AddForce(new Vector2(0, 2f) * Speed);
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddTorque(2f);
            rb.gravityScale = 1;

            GameOver();
        }

        if (collision.gameObject.tag == "Wall" && GameController.control.isDead == false)
        {
            GameController.control.isDead = true;

            rb.AddForce(new Vector2(0, 2f) * Speed);
            rb.constraints = RigidbodyConstraints2D.None;
            rb.AddTorque(2f);
            rb.gravityScale = 1;

            GameOver();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Crate" && GameController.control.isDead == false)
        {
            looper.cratesPool.Enqueue(collider.gameObject);
            collider.gameObject.SetActive(false);

            UpdatePlayerStats();
        }
    }

    private void UpdatePlayerStats()
    {
        GameController.control.score++;
        GameController.control.coins++;

        scoreText.text = GameController.control.score.ToString();
        coinText.text = GameController.control.coins.ToString() + "$";  
    }

    private void LoadPlayerStats()
    {
        GameController.control.score = 0;
        GameController.control.Load();
        DisplayPlayerData();
    }

    private void DisplayPlayerData()
    {
        scoreText.text = GameController.control.score.ToString();
        highScoreText.text = "High " + GameController.control.highScore.ToString();
        coinText.text = GameController.control.coins.ToString() + "$";
    }

    private void GameOver()
    {
        Vector3 nextPos = transform.position;

        if(nextPos.x > 0)
        {
            nextPos.x = -2f;
        }
        else
        {
            nextPos.x = 2f;
        }

        GameController.control.lastPosition = nextPos;
        GameController.control.isDead = true;

        if (GameController.control.score > GameController.control.highScore)
        {
            GameController.control.highScore = GameController.control.score;
        }

        DisplayPlayerData();

        goScoreText.text = GameController.control.score.ToString();
        goHighText.text = GameController.control.highScore.ToString();

        gameOverAnimator.SetBool("IsActive", true);
        menuAnimator.SetBool("IsActive", true);

        GameController.control.Save();
    }

    private void PauseGame()
    {
        Time.timeScale = 0;
    }

    private void UnPauseGame()
    {
        Time.timeScale = 1;
    }
}
