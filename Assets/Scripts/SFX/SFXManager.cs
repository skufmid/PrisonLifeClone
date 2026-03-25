using UnityEngine;
using System.Collections.Generic;

public enum SFXType
{
    Mine,
    Dib,
    DingDong,
}

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;
    private AudioSource source;

    [SerializeField] private SFXLibrary sfxLibrary;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        source = GetComponent<AudioSource>();
    }

    public void Play(AudioSource source, SFXType type)
    {
        AudioClip clip = GetClip(type);
        if (source == null) return;
        if (clip == null) return;

        source.PlayOneShot(clip);
    }

    public void PlaySelf(SFXType type)
    {
        Play(source, type);
    }

    private AudioClip GetClip(SFXType type)
    {
        return type switch
        {
            SFXType.Mine => sfxLibrary.Mine,
            SFXType.Dib => sfxLibrary.Dib,
            SFXType.DingDong => sfxLibrary.DingDong,
            _ => null
        };
    }
}