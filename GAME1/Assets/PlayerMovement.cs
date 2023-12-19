using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using UnityEngine.Experimental.Rendering.Universal; // Import this namespace for Light2D


public class PlayerMovement : MonoBehaviour
{
    // Start is called before the first frame update

    public float moveSpeed = 0.5f;
    public float walkSpeed = 0.5f;
    public float sprintSpeed = 1f;
    public float staminaMax = 10f;
    public float staminaRegenRate = 0.5f;

    private float currentStamina;
    private bool isWalking = true;
    private bool isSprinting = false;
    private bool isHiding = false;
    private Vector3 hidingStartPosition; 
    
    private bool isLighten = false;

    // Reference to the Light2D component
    private Light2D playerLight;

    public Rigidbody2D rb;
    public Animator animator;

    [SerializeField]
    private Button PauseButton;

    [SerializeField]
    private Button resumeButton;

    [SerializeField]
    private dialogueBox DialogueBox;
    public dialogueBox dialogueBox => DialogueBox;
    public IInteractible Interactible { get; set; }

    private bool isPaused = false;


    Vector2 movement;
    Vector3 moveDir;

    void Start()
    {
        // Assuming the Light2D component is in a child object of this GameObject
        playerLight = GetComponentInChildren<Light2D>();
        currentStamina = staminaMax;

        // Set isLighten to false
        isLighten = false;

        // Disable the Light2D component
        if (playerLight != null)
        {
            playerLight.enabled = false;
        }

        // Set the "isLighten" parameter in the animator to false
        if (animator != null)
        {
            animator.SetBool("isLighten", isLighten);
        }

        // Add a listener for the resume button click event
        if (resumeButton != null)
        {
            resumeButton.onClick.AddListener(ResumeGame);
        }
    }



    void ResumeGame()
    {
        isPaused = false;
        Time.timeScale = 1f;

        // Optionally, you can do other things when the game is unpaused, like hiding the pause menu UI
    }

    void PauseGame()
    {
        // Pause the game logic here
        Time.timeScale = 0f; // Set the time scale to 0 to pause the game

        // Optionally, you can do other things when the game is paused, like showing the pause menu UI
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueBox.isOpen) return;
        //Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (movement.x != 0  || movement.y != 0)
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            if (!isWalking)
            {
                isWalking = true;
                animator.SetBool("isMoving", isWalking);
            }
        }
        else
        {
            if (isWalking)
            {
                isWalking = false;
                animator.SetBool("isMoving", isWalking);
                StopMoving();
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isLighten)
            {
                // If isLighten is true, set it back to false
                isLighten = false;

                // Disable the Light2D component
                if (playerLight != null)
                {
                    playerLight.enabled = false;
                }

                // Trigger your animation or reset any other related variables
                animator.SetBool("isLighten", isLighten);
            }
            else
            {
                // If isLighten is false, set it to true
                isLighten = true;

                // Enable the Light2D component
                if (playerLight != null)
                {
                    playerLight.enabled = true;
                }

                // Trigger your animation or perform other actions related to right mouse button click
                animator.SetBool("isLighten", isLighten);
            }
        }

        // Check for interaction with hiding spots
        if (Input.GetKeyDown(KeyCode.E))
        {
            Interactible?.Interact(this);
            if (isHiding)
            {
                EndHiding();
            }
            else
            {
                TryToHide();
            }
        }

        animator.SetFloat("Speed", movement.sqrMagnitude); 
        Sprinting();

        if (isSprinting)
        {
            currentStamina -= Time.deltaTime * (staminaMax / 5f); // Adjust the divisor to control sprint duration
            if (currentStamina < 1)
            {
                currentStamina = 0;
                isSprinting = false;
            }
        }
        else
        {
            currentStamina += Time.deltaTime * staminaRegenRate;
            currentStamina = Mathf.Clamp(currentStamina, 0, staminaMax);
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }


        void TogglePause()
        {
            if (PauseButton != null)
                PauseButton.onClick.Invoke();
            {
                isPaused = !isPaused;

                if (isPaused)
                {
                    PauseGame();
                }
                else
                {
                    // Don't need to call UnpauseGame here, as it's handled by the "Resume" button click
                }
            }
        }

        moveDir = new Vector3(movement.x, movement.y).normalized;
    }

    private void StopMoving()
    {
        rb.velocity = Vector3.zero;
    }

    private void FixedUpdate()

    {
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }



    void Sprinting()
    {
        if (Input.GetKey(KeyCode.RightShift) && !isHiding && currentStamina > 1)
        {
            isSprinting = true;
            moveDir = new Vector3(movement.x, movement.y).normalized;
            moveSpeed = sprintSpeed;
        }
        else
        {
            isSprinting = false;
            moveDir = Vector3.zero;
            moveSpeed = walkSpeed;
        }
    }

    void TryToHide()
    {
        // Raycast to check if there is a hiding spot in front of the player
        RaycastHit2D hit = Physics2D.Raycast(transform.position, moveDir, 0.5f, LayerMask.GetMask("HidingSpot"));

        if (hit.collider != null)
        {
            StartHiding();
        }
    }

    void StartHiding()
    {
        isHiding = true;
        hidingStartPosition = transform.position;

        // Deactivate components and make the player invisible
        GetComponent<SpriteRenderer>().enabled = false;
        GetComponent<Collider2D>().enabled = false;
        // Add more components to deactivate as needed

        // Optionally perform additional hiding actions, e.g., play hiding animation
    }

    void EndHiding()
    {
        // Reactivate components and make the player visible
        GetComponent<SpriteRenderer>().enabled = true;
        GetComponent<Collider2D>().enabled = true;
        // Add more components to reactivate as needed

        // Reappear the player at the last hiding position
        transform.position = hidingStartPosition;

        // Optionally perform additional actions after ending hiding, e.g., play end hiding animation

        isHiding = false;
    }

}
