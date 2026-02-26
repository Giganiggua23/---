using UnityEngine;

public class ObjMovement : MonoBehaviour
{
    public float minY = 1f;
    public float maxY = 10f;
    public float speed = 2f;
    public bool coinflag;

    private float targetY;

    void Start()
    {
        targetY = Random.Range(minY, maxY);
    }

    void Update()
    {
        Movement();
    }
    void Movement()
    {
        transform.position = new Vector3(
                   transform.position.x - speed * Time.deltaTime,
                   Mathf.MoveTowards(transform.position.y, targetY, speed * Time.deltaTime),
                   transform.position.z
               );

        if (transform.position.y == targetY)
        {
            targetY = Random.Range(minY, maxY);
        }
    }
    void OnTriggerEnter2D(Collider2D player)
    {
        if (player.CompareTag("Player") && coinflag)
        {
            Destroy(gameObject, 0.2f);
        }

    }
}
