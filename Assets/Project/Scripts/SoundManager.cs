using UnityEngine;
using System.Collections.Generic;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Settings")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float throttleTime = 0.05f;

    [Header("Audio Library")]
    [SerializeField] private List<SoundEffect> soundLibrary;

    private Dictionary<string, float> lastPlayedTimes = new Dictionary<string, float>();

    [System.Serializable]
    public struct SoundEffect
    {
        public string name;
        public AudioClip clip;
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        if (audioSource == null)
        {
            audioSource = GetComponent<AudioSource>();
        }
    }

    public void PlaySound(string soundName, bool doPitchShift = false)
    {
        if (lastPlayedTimes.TryGetValue(soundName, out float lastTime))
        {
            if (Time.time < lastTime + throttleTime) return;
        }

        SoundEffect effect = soundLibrary.Find(s => s.name == soundName);

        if (effect.clip == null)
        {
            Debug.LogWarning($"Sound: {soundName} not found in library!");
            return;
        }

        lastPlayedTimes[soundName] = Time.time;
        audioSource.pitch = doPitchShift ? Random.Range(0.9f, 1.1f) : 1f;
        audioSource.PlayOneShot(effect.clip);
    }

    public void PlaySound(int index, bool doPitchShift = false)
    {
        if (index < 0 || index >= soundLibrary.Count) return;

        string soundName = soundLibrary[index].name;
        if (lastPlayedTimes.TryGetValue(soundName, out float lastTime))
        {
            if (Time.time < lastTime + throttleTime) return;
        }

        lastPlayedTimes[soundName] = Time.time;
        audioSource.pitch = doPitchShift ? Random.Range(0.9f, 1.1f) : 1f;
        audioSource.PlayOneShot(soundLibrary[index].clip);
    }
}