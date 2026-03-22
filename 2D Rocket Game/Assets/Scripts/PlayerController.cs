using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour
{
    public float thrustForce = 10f;
    public float maxSpeed = 5f;
    public float scoreMultiplier = 10f;
    
    private float score = 0f;
    private float elapsedTime = 0f;
    private float highScore = 0f;

    private Label scoreText;
    private Label highScoreText;
    private Button restartButton;
    public UIDocument uiDocument;
    public GameObject boosterFlame;
    public GameObject explosionEffect;


    Rigidbody2D rb;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Assigning UI elements to variables through query by name and then turning them off
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");
        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighscoreLabel");
        highScoreText.style.display = DisplayStyle.None;
        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;//Adds the ReloadScene function to the action list for clicking the restart button

        rb = GetComponent<Rigidbody2D>();
        highScore = PlayerPrefs.GetFloat("Highscore");


    }

    // Update is called once per frame
    void Update()
    {
       UpdateScore();
       MovePlayer();
    }

    void UpdateScore()
    {
        elapsedTime += Time.deltaTime; 
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
    }

    void MovePlayer()
    {
        if (Mouse.current.leftButton.isPressed)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
            Vector2 direction = (mousePos - transform.position).normalized;
            transform.up = direction;
            rb.AddForce(direction * thrustForce);
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            boosterFlame.SetActive(true);
        }
        else if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            boosterFlame.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        restartButton.style.display = DisplayStyle.Flex;
        highScoreText.style.display = DisplayStyle.Flex;
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("Highscore", highScore);
        }
        highScoreText.text = "High Score: " + highScore;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

