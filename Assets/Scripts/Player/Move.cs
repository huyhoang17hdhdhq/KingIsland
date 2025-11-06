using UnityEngine;

public class Move : MonoBehaviour
{
    [Header("Tham chiếu đến Joystick")]
    public Joystick joystick;


    [Header("Tốc độ di chuyển")]
    public float moveSpeed = 5f;

    private Rigidbody2D rb;
    private Vector2 moveInput;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Lấy giá trị từ Joystick
        float moveX = joystick.Horizontal();
        float moveY = joystick.Vertical();

        moveInput = new Vector2(moveX, moveY);

        // Giới hạn tốc độ (nếu joystick chéo)
        if (moveInput.magnitude > 1)
            moveInput.Normalize();

        // 🔄 Quay mặt theo hướng di chuyển
        if (moveInput.x > 0.1f)
            transform.localScale = new Vector3(1, 1, 1); // Quay mặt phải
        else if (moveInput.x < -0.1f)
            transform.localScale = new Vector3(-1, 1, 1); // Quay mặt trái
    }

    void FixedUpdate()
    {
        // Di chuyển nhân vật theo joystick
        rb.velocity = moveInput * moveSpeed;
    }
}
