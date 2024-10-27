using UnityEngine;

public class AudioManager :Singleton<AudioManager>
{
    [SerializeField] private AudioSource effectSrc;
    [SerializeField] private AudioSource backGroundSrc;


    [SerializeField] private AudioClip backgroundMusic;

    
    private bool isBackgroundMusicMute = false;
    private bool isEffectsMute = false;

    private void Start()
    {
        PlayBG(backgroundMusic); // Optionally play default BG at the start
    }
    // Play background music
    public void PlayBG(AudioClip bgClip)
    {
        if (isBackgroundMusicMute || backgroundMusic == null) return;

        backGroundSrc.clip = bgClip;
        backGroundSrc.loop = true;
        backGroundSrc.Play();
    }

    // Stop background music
    public void StopBG()
    {
        backGroundSrc.Stop();
    }

    // Play sound effect
    public void PlayEffect(AudioClip effectClip)
    {
        if (isEffectsMute || effectClip == null) return;

        effectSrc.PlayOneShot(effectClip);
    }

    // Toggle background music on/off
    public void ToggleBG()
    {
        isBackgroundMusicMute = !isBackgroundMusicMute;

        if (isBackgroundMusicMute)
        {
            backGroundSrc.Pause();
        }
        else
        {
            backGroundSrc.UnPause();
        }
    }

    // Toggle sound effects on/off
    public void ToggleFX()
    {
        isEffectsMute = !isEffectsMute;
    }

    // Check if BG is muted
    public bool IsBackgroundMusicMute()
    {
        return isBackgroundMusicMute;
    }

    // Check if FX is muted
    public bool IsEffectsMute()
    {
        return isEffectsMute;
    }
}