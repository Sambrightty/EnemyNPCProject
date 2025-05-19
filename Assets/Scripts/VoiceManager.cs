using UnityEngine;

public class VoiceManager : MonoBehaviour
{
    public AudioClip alertedClip;
    public AudioClip hurtClip;
    public AudioClip retreatClip;

    private AudioSource audioSource;
    private float cooldown = 3f;
    private float lastPlayTime = -5f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayVoice(AudioClip clip)
    {
        if (clip == null || audioSource == null) return;

        if (Time.time - lastPlayTime >= cooldown)
        {
            audioSource.PlayOneShot(clip);
            lastPlayTime = Time.time;
        }
    }

    public void PlayAlertedVoice()
    {
        PlayVoice(alertedClip);
    }

    public void PlayHurtVoice()
    {
        PlayVoice(hurtClip);
    }

    public void PlayRetreatVoice()
    {
        PlayVoice(retreatClip);
    }
}
