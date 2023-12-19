using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMovement : MonoBehaviour
{
    public float speed;
    public float checkRadius;
    public float attackRadius;
    public float losRadius;

    public bool shouldRotate;
    public bool isHiding;

    public LayerMask whatIsPlayer;
    public LayerMask whatIsObstacle;

    private Transform target;
    private Rigidbody2D rb;
    private Animator anim;

    public Vector2 movement;
    public Vector3 dir;

    public float Health
    {
        set
        {
            bossHealth = value;

            if (bossHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
        get
        {
            return bossHealth;
        }
    }
    public float bossHealth = 2;

    private bool isInChaseRange;
    private bool isInAttackRange;
    private bool hasLineOfSight;
    public float chaseDuration = 5f; // Set the chase duration in seconds
    private float chaseTimer;

    private Vector2 initialPosition;
    private bool isReturningToInitialPosition;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        target = GameObject.FindWithTag("Player").transform;
        initialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isHiding)
        {
            anim.SetBool("isRunning", isInChaseRange);
            anim.SetBool("isIdle", isInAttackRange);
            isInChaseRange = Physics2D.OverlapCircle(transform.position, checkRadius, whatIsPlayer);
            isInAttackRange = Physics2D.OverlapCircle(transform.position, attackRadius, whatIsPlayer);

            hasLineOfSight = Physics2D.Raycast(transform.position, dir.normalized, losRadius, whatIsObstacle);

            dir = target.position - transform.position;
            float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
            dir.Normalize();
            movement = dir;
            if (shouldRotate)
            {
                anim.SetFloat("Horizontal", dir.x);
                anim.SetFloat("Vertical", dir.y);
            }

            if (isInChaseRange && !isInAttackRange && !hasLineOfSight && !isHiding)
            {
                MoveCharacter(movement);
                chaseTimer += Time.deltaTime;

                // Check if the chase duration has been reached
                if (chaseTimer >= chaseDuration)
                {
                    // Stop chasing and reset the timer
                    isInChaseRange = false;
                    chaseTimer = 0f;
                }
            }

            if (isReturningToInitialPosition)
            {
                // Calculate direction to the initial position
                Vector2 returnDir = (initialPosition - (Vector2)transform.position).normalized;

                // Move towards the initial position
                MoveCharacter(returnDir);

                // Check if close to the initial position
                float distanceToInitial = Vector2.Distance(transform.position, initialPosition);
                if (distanceToInitial < 0.1f)
                {
                    // Reached the initial position, stop returning
                    isReturningToInitialPosition = false;
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (isInChaseRange && !isInAttackRange && !hasLineOfSight && !isHiding)
        {
            MoveCharacter(movement);
        }
        if (isInAttackRange)
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void MoveCharacter(Vector2 dir)
    {
        rb.MovePosition((Vector2)transform.position + (dir * speed * Time.deltaTime));
    }

    void OnHit(float damage)
    {
        bossHealth -= damage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnHit(1); // Enemy takes 1 damage
        }
    }

    // Call this method to reset the enemy to its initial position
    void ResetToInitialPosition()
    {
        transform.position = initialPosition;
    }
}
