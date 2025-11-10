using UnityEngine;
using UnityEngine.UI;

public class AddCoinTest : MonoBehaviour
{
    [Header("Button để test cộng coin")]
    public Button addCoinButton;

    [Header("Số coin sẽ cộng mỗi lần bấm")]
    public int addAmount = 10;
    public PlayerInventory playerInventory;

    private void Start()
    {
        // Gắn sự kiện cho button
        if (addCoinButton != null)
            addCoinButton.onClick.AddListener(OnAddCoinClicked);
        else
            Debug.LogWarning("⚠️ Chưa gán button test coin!");
    }

    private void OnAddCoinClicked()
    {
        // Gọi singleton ItemManager để cộng coin
        if (PlayerInventory.Instance != null)
            PlayerInventory.Instance.AddCoin(addAmount);
        else
            Debug.LogError("❌ Không tìm thấy ItemManager trong scene!");
    }
}
