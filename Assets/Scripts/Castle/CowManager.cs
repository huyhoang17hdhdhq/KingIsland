using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowManager : Castle
{
    private Animator animator;
    protected override ItemType RewardType => ItemType.Milk;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError($"[CowManager] Không tìm thấy Animator trên GameObject: {gameObject.name}", this);
        }
    }

    protected override void Update()
    {
        base.Update();

        if (animator != null)
        {
            if (isFilling)                   
                animator.SetTrigger("Trigger_ReadyMilk");
            else
                animator.SetTrigger("Trigger_Walking");
        }
    }

    protected override void SpawnReward()
    {
        MusicManager.Instance.CowSound();
        base.SpawnReward();
        Debug.Log("Bò đã cho sữa");

        
    }
}