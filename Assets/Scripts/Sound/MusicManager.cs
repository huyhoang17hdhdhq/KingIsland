using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    public AudioSource walking;
    public AudioSource farmtree;
    public AudioSource farmFiled;
    public AudioSource cow;
    public AudioSource chicken;
    public AudioSource lootItems;
    public AudioSource health;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RunSound()=> walking.Play();
    public void StopSound()=> walking.Stop();
    public void FarmTreeSound()=> farmtree.Play();
    public void FarmFieldSound()=> farmFiled.Play();
    public void CowSound()=> cow.Play();
    public void ChickenSound()=> chicken.Play();
    public void LootItemsSound()=> lootItems.Play();
    public void HealthSound()=> health.Play();

}