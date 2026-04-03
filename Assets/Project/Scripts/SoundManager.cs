using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] sounds;

    private void Awake()
    {
        instance = this;
    }
    public void PlaySound(int sound, bool doPitchShift)
    {
        AudioClip clip = sounds[sound];

        if (doPitchShift)
        {
            audioSource.pitch = Random.Range(0.9f, 1.1f);
        }

        audioSource.PlayOneShot(clip);
    }
}
