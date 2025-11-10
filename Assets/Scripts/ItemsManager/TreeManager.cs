using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TreeManager : MonoBehaviour
{
    [Header("Thanh máu / năng lượng của cây")]
    public Image fillBar;

    [Header("Tốc độ giảm (0.1 = giảm 10%)")]
    public float decreaseAmount = 0.1f;

    [Header("Prefab gỗ sẽ rơi ra khi cây bị chặt xong")]
    public GameObject lumberPrefab;

    [Header("Vị trí spawn gỗ (nếu để trống sẽ dùng vị trí cây)")]
    public Transform spawnPoint;

    

    public void ReduceFill()
    {
        if (fillBar != null)
        {
            fillBar.fillAmount -= decreaseAmount;

            if (fillBar.fillAmount <= 0)
            {
                fillBar.fillAmount = 0;
                Die();
            }
        }
    }

    private void Die()
    {
        if (lumberPrefab != null)
        {
            
            Vector3 spawnPos = spawnPoint ? spawnPoint.position : transform.position;

            
            Instantiate(lumberPrefab, spawnPos, Quaternion.identity);
        }



    }
}

