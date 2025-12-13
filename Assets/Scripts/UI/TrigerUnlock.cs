 using UnityEngine;
using System.Collections.Generic;

public class TriggerUnlock : MonoBehaviour
{
    [Header("=== ĐẢO SẼ ĐƯỢ MỞ KHÓA ===")]
    [SerializeField] private List<GameObject> objectsToEnable = new List<GameObject>();  // Bật lên khi mở
    [SerializeField] private List<GameObject> objectsToDisable = new List<GameObject>(); // Tắt đi khi mở

    [Header("=== GIÁ MỞ KHÓA ===")]
    [SerializeField] private int unlockPrice = 1000;
    [SerializeField] private ResourceType costType = ResourceType.Gold;

    [Header("=== LƯU TRẠNG THÁI MỞ KHÓA ===")]
    [SerializeField] private ResourceType saveKey; // Ví dụ: UnlockedIsland_01, UnlockedIsland_02...

    [Header("=== NÚT MUA CHUNG (gắn 1 lần duy nhất) ===")]
    [SerializeField] private GameObject sharedBuyButton;

    private static TriggerUnlock currentActiveTrigger;

    private void Awake()
    {
        // Tải trạng thái đã mở khóa chưa
        if (ResourceManager.Instance.Get(saveKey) == 1)
        {
            EnableIsland();
            gameObject.SetActive(false); 
        }
        else
        {
            DisableIsland();
        }

        if (sharedBuyButton != null)
            sharedBuyButton.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        currentActiveTrigger = this;
        if (sharedBuyButton != null)
            sharedBuyButton.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        if (currentActiveTrigger == this)
        {
            currentActiveTrigger = null;
            if (sharedBuyButton != null)
                sharedBuyButton.SetActive(false);
        }
    }

    // GỌI TỪ NÚT MUA (gắn vào OnClick của sharedBuyButton)
    public static void TryUnlockCurrentIsland()
    {
        if (currentActiveTrigger == null) return;

        var trigger = currentActiveTrigger;

        // Kiểm tra đủ tiền
        if (!ResourceManager.Instance.TrySpend(trigger.costType, trigger.unlockPrice))
        {
            Debug.Log("Không đủ tiền mở đảo!");
            // Có thể hiện popup "Không đủ vàng!"
            return;
        }

        // MỞ ĐẢO
        trigger.EnableIsland();

        // Lưu trạng thái đã mở
        ResourceManager.Instance.Set(trigger.saveKey, 1);
        QuestManager.Instance.ReportUnlockIsland(trigger.saveKey);

        // Tắt trigger và nút mua
        trigger.gameObject.SetActive(false);
        if (trigger.sharedBuyButton != null)
            trigger.sharedBuyButton.SetActive(false);

        Debug.Log($"ĐÃ MỞ ĐẢO THÀNH CÔNG! Giá: {trigger.unlockPrice} {trigger.costType}");
    }

    // Bật/tắt các object theo list
    private void EnableIsland()
    {
        foreach (var obj in objectsToEnable)
            if (obj != null) obj.SetActive(true);

        foreach (var obj in objectsToDisable)
            if (obj != null) obj.SetActive(false);
    }

    private void DisableIsland()
    {
        foreach (var obj in objectsToEnable)
            if (obj != null) obj.SetActive(false);

        foreach (var obj in objectsToDisable)
            if (obj != null) obj.SetActive(true);
    }
}