using UnityEngine;
using DG.Tweening;
public class PlayerController2d : MonoBehaviour
{
    public static PlayerController2d Instance;

    public float[] lanes = new float[] { -2f, 2f };
    public float[] particleposition = new float[] { -0.5f, 0.5f };
    private int currentLane = 0;

    public float forwardSpeed = 3f;

    public float speedIncreaseRate = 0.1f;

    public ParticleSystem wallcollisioneffect;
    public ParticleSystem obstaclecollisioneffect;
    public GameObject trailRenderer;

    public float spinSpeed = 40f;
  

    private void Awake()
    {
        Instance = this;
    }
    private void Start()
    {
        wallcollisioneffect.Stop();
        obstaclecollisioneffect.Stop();
       
    }

    public static bool IsPlayerStarted = false;

    void Update()
    {

        if (!GameManager.IsGameStart) return;
        
        HandleMovement();
        MoveForward();

        SpinPlayer();
    }

    void SpinPlayer()
    {
        transform.Rotate(0f, -spinSpeed, 0f);
    }

    void HandleMovement()
    {
        if (Input.GetMouseButtonDown(0) && !GameManager.IsGameOver)
        {
            IsPlayerStarted = true;
            currentLane++;
            if (currentLane >= lanes.Length)
                currentLane = 0;

            transform.DOMoveX(lanes[currentLane], .1f).SetEase(Ease.Linear);

            wallcollisioneffect.Play();           

        }
    }


    void MoveForward()
    {       
        if(!IsPlayerStarted)
        {
            return;
        }
        transform.Translate(Vector3.up * forwardSpeed * Time.deltaTime);
        IncreaseDifficulty();

    }


    void IncreaseDifficulty()
    {
        forwardSpeed += speedIncreaseRate * Time.deltaTime;
        forwardSpeed = Mathf.Clamp(forwardSpeed, 3f, 12f); // limit speed
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Obstacle") || collision.gameObject.CompareTag("Boom"))
        {
            if (GameManager.Instance.isRetrying)
            {
                collision.gameObject.SetActive(false);
                GameManager.Instance.isRetrying = false;
                return;

            }
            obstaclecollisioneffect.gameObject.SetActive(true);
            obstaclecollisioneffect.Play();
            IsPlayerStarted = false;
            GameManager.Instance.GameOver();
            CameraShake.Instance.Shake();
            ScoreManager.Instance.HighScore();
           
            Debug.Log("Collided with obstacle-GameOver!");
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Star"))
        {
            Debug.Log("Star collected!");
            collision.gameObject.SetActive(false);
            ScoreManager.Instance.AddStar();
        }
    }


}