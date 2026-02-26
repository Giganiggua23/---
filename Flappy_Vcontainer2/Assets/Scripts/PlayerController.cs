using UnityEngine;
using UnityEngine.SceneManagement;
using VContainer;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float jumpForce = 6f;

    [Header("Effects")]
    [SerializeField] private ParticleSystem trailParticles;

    [Header("Spawn Settings")]
    [SerializeField] private Vector3 startPosition = new Vector3(-2.5f, 0f, 0f);

    private Rigidbody2D rb;
    private bool isAlive = true;
    private ScoreManager scoreManager;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    [Inject]
    public void Construct(ScoreManager scoreManager)
    {
        this.scoreManager = scoreManager;
    }

    private void Update()
    {
        if (!isAlive) return;

        if (DetectJumpInput())
        {
            Jump();
        }
    }

    private bool DetectJumpInput()
    {
        return Input.GetMouseButtonDown(0) ||
               Input.GetKeyDown(KeyCode.Space) ||
               DetectTouchInput();
    }

    private bool DetectTouchInput()
    {
        return Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began;
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0f);
        rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);

        if (trailParticles != null)
        {
            trailParticles.Play();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            scoreManager.AddScore(1);
        }
        if (other.CompareTag("Enemy"))
        {
            HandleDeath();
        }
    }

    private void HandleDeath()
    {
        isAlive = false;

        if (AdsService.Instance != null)
        {
            AdsService.Instance.ShowRewarded(() =>
            {
                ResetGame();
            });
        }
        else
        {
            ResetGame();
        }
    }

    private void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void ResetPlayer()
    {
        transform.position = startPosition;
        transform.rotation = Quaternion.identity;
        rb.linearVelocity = Vector2.zero;
        isAlive = true;
    }
}