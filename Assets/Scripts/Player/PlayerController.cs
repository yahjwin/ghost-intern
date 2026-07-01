using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("이동 속도")]
    public float walkSpeed = 4f;
    public float runSpeed = 7f;

    [Header("귀신 스프라이트")]
    public SpriteRenderer ghostRenderer;
    public Sprite ghostFrontSprite;
    public Sprite ghostBackSprite;

    [Header("둥둥 떠다니기")]
    public Transform ghostFloatTarget;
    public float floatSpeed = 3f;
    public float floatHeight = 0.15f;

    [Header("이동 사운드")]
    public AudioSource moveAudioSource;

    private Rigidbody rb;
    private Vector3 moveDirection;
    private Vector3 ghostStartLocalPosition;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        if (ghostFloatTarget != null)
            ghostStartLocalPosition = ghostFloatTarget.localPosition;

        SetGhostFront();
    }

    void Update()
    {
        float moveX = Input.GetAxisRaw("Horizontal");
        float moveZ = Input.GetAxisRaw("Vertical");

        moveDirection = new Vector3(moveX, 0f, moveZ).normalized;

        UpdateGhostSprite(moveX, moveZ);
        FloatGhost();

        HandleMoveSound();
    }

    void FixedUpdate()
    {
        float currentSpeed =
            Input.GetKey(KeyCode.LeftShift)
            ? runSpeed
            : walkSpeed;

        Vector3 move =
            moveDirection *
            currentSpeed *
            Time.fixedDeltaTime;

        rb.MovePosition(rb.position + move);

        if (moveDirection != Vector3.zero)
        {
            transform.forward = moveDirection;
        }
    }

    void HandleMoveSound()
    {
        if (moveAudioSource == null)
            return;

        if (moveDirection != Vector3.zero)
        {
            if (!moveAudioSource.isPlaying)
            {
                moveAudioSource.Play();
            }
        }
        else
        {
            if (moveAudioSource.isPlaying)
            {
                moveAudioSource.Stop();
            }
        }
    }

    void UpdateGhostSprite(float moveX, float moveZ)
    {
        if (ghostRenderer == null)
            return;

        if (moveZ > 0)
        {
            SetGhostBack();
        }
        else if (moveZ < 0)
        {
            SetGhostFront();
        }
        else if (moveX > 0)
        {
            SetGhostFront();
            ghostRenderer.flipX = false;
        }
        else if (moveX < 0)
        {
            SetGhostFront();
            ghostRenderer.flipX = true;
        }
    }

    void SetGhostFront()
    {
        if (ghostRenderer != null && ghostFrontSprite != null)
        {
            ghostRenderer.sprite = ghostFrontSprite;
            ghostRenderer.flipX = false;
        }
    }

    void SetGhostBack()
    {
        if (ghostRenderer != null && ghostBackSprite != null)
        {
            ghostRenderer.sprite = ghostBackSprite;
            ghostRenderer.flipX = false;
        }
    }

    void FloatGhost()
    {
        if (ghostFloatTarget == null)
            return;

        Vector3 pos = ghostStartLocalPosition;
        pos.y += Mathf.Sin(Time.time * floatSpeed) * floatHeight;
        ghostFloatTarget.localPosition = pos;
    }
}