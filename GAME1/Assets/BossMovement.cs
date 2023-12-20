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

    public FireBullets fireBullets;

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
            kuyHealth = value;

            if (kuyHealth <= 0)
            {
                Destroy(gameObject);
            }
        }
        get
        {
            return kuyHealth;
        }
    }
    public float kuyHealth = 10;

    private bool isInChaseRange;
    private bool isInAttackRange;
    private bool hasLineOfSight;

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

            if (fireBullets != null)
            {
                fireBullets.enabled = isInChaseRange && isInAttackRange;
            }

            hasLineOfSight = Physics2D.Raycast(transform.position, dir.normalized, losRadius, whatIsObstacle);


            if (!isInChaseRange && !isInAttackRange && !hasLineOfSight && !isReturningToInitialPosition)
            {
                // Player is not in chase range, not in attack range, and not hiding.
                // Start returning to the initial position.
                isReturningToInitialPosition = true;
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
        kuyHealth -= damage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            OnHit(1); // Enemy takes 1 damage
            MoveToInitialPosition(); // Move to initial position after colliding with player
        }
    }

    // Call this method to reset the enemy to its initial position
    void ResetToInitialPosition()
    {
        transform.position = initialPosition;
        isReturningToInitialPosition = false; // Reset the flag
    }

    // Call this method to initiate movement to the initial position
    void MoveToInitialPosition()
    {
        isReturningToInitialPosition = true;
    }
}
