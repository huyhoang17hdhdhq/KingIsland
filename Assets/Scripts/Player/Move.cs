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

        if (moveInput.magnitude > 0.1f)
        {
            animator.SetTrigger("nowFarming");
            animator.SetTrigger("nowWalk");
        }
        else
        {
            animator.SetTrigger("nowIdle");
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

    // ĐÃ XÓA SẠCH 2 HÀM OnTriggerEnter2D và OnTriggerExit2D NHƯ MÀY YÊU CẦU!!!

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