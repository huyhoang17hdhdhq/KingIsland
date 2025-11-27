using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeManager : MonoBehaviour
{
    [Header("Select")]
    public GameObject selectMarker;


    [Header("Prefab gỗ sẽ rơi ra khi cây bị chặt xong")]
    public GameObject lumberPrefab;

    [Header("Vị trí spawn gỗ (nếu để trống sẽ dùng vị trí cây)")]
    public Transform spawnPoint;

   
    public void Select()
    {
        selectMarker.gameObject.SetActive (true);
    }
    public void Chop()
    {
        Die();
    }

    private void Die()
    {
        if (lumberPrefab != null)
        {
            
            Vector3 spawnPos = spawnPoint ? spawnPoint.position : transform.position;

            Instantiate(lumberPrefab, spawnPos, Quaternion.identity);
        };

        gameObject.SetActive(false);
    }
}
