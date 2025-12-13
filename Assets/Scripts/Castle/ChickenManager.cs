using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChickenManager : Castle
{
    private Animator animator;
    protected override ItemType RewardType => ItemType.Egg;


    private void Awake()
    {
        animator = GetComponent<Animator>();


        if (animator == null)
        {
            Debug.LogError($"[CowManager] Không tìm thấy Animator trên GameObject: {gameObject.name}. Hãy thêm component Animator!", this);
        }
    }
    protected override void Update()
    {
        base.Update();

        if (animator != null)
        {
            if (isFilling)
                animator.SetTrigger("Trigger_ReadyEgg");
            else
                animator.SetTrigger("Trigger_Walking");
        }
    }

    protected override void SpawnReward()
    {
        animator.SetTrigger("Trigger_Fly");
        MusicManager.Instance.ChickenSound();
        base.SpawnReward();
        Debug.Log("Gà đã đẻ trứng!");
    }
    
}
