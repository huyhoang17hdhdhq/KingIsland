using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Tham chiếu đến Joystick")]
    public Joystick joystick;
    [Header("Tốc độ di chuyển")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private TreeManager currentTree;

    private bool isMoving = false;

    

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        float moveX = joystick.Horizontal();
        float moveY = joystick.Vertical();
        moveInput = new Vector2(moveX, moveY);
        bool currentlyMoving = moveInput.magnitude > 0.1f;

        if (currentlyMoving)
        {
            animator.SetTrigger("nowFarming");
            animator.SetTrigger("nowWalk");

            if (!isMoving)
            {
                MusicManager.Instance.RunSound();
                isMoving = true;
            }
        }
        else
        {
            if (isMoving)
            {
                animator.SetTrigger("nowIdle");
                MusicManager.Instance.StopSound();
                isMoving = false;
            }
            
        }

        if (moveInput.magnitude > 1)
            moveInput.Normalize();

        if (moveInput.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1);
        else if (moveInput.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1);
    }

    void FixedUpdate()
    {
        rb.velocity = moveInput * moveSpeed;
    }

    

    public void OnFarmAnimationEnd()
    {
        animator.SetTrigger("EndFarmingMotion");
        animator.SetTrigger("nowFarming");
        if (currentTree != null)
        {
            currentTree.Chop();
        }
    }
}