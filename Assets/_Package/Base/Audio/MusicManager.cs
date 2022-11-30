﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Music
{
    Login,
    Main,
}

public class MusicManager : MonoSingletonGlobal<MusicManager>
{
    [System.Serializable]
    public class MusicTable
    {
        public Music music;
        public AudioClip clip;
    }

    [SerializeField] private MusicTable[] musics;
    [SerializeField] AudioSource audioSource;
    private Coroutine corChangeVolume, corChangeSound;
    private Dictionary<Music, AudioClip> musicDics = new Dictionary<Music, AudioClip>();

    protected override void Awake()
    {
        base.Awake();
        foreach (var _s in musics)
        {
            musicDics.Add(_s.music, _s.clip);
        }
    }

    private IEnumerator Start()
    {
        //yield return new WaitUntil(() => RuntimeStorageData.SOUND != null);

        if (audioSource == null)
            audioSource = GetComponent<AudioSource>();
        audioSource.mute = !RuntimeStorageData.Sound.isSound;

        if (musics.Length == 0)
            yield break;

        PlaySound(Music.Login);
    }

    public void PlaySound(Music sound)
    {
        PauseSound();
        audioSource.clip = ConverToClip(sound);
        audioSource.loop = true;
        audioSource.Play();
    }

    public void PauseSound()
    {
        audioSource.Pause();
    }

    public void UnPauseSound()
    {
        audioSource.UnPause();
    }

    //public void ChangeSound(Music sound)
    //{
    //    if (corChangeSound != null)
    //    {
    //        StopCoroutine(corChangeSound);
    //    }
    //    corChangeSound = StartCoroutine(RunChangeSound(sound));
    //}
    //IEnumerator RunChangeSound(Music sound)
    //{
    //    float speed = Time.deltaTime * 3;
    //    float target1 = .02f;
    //    while (audioSource.volume > target1)
    //    {
    //        audioSource.volume -= speed;
    //        yield return null;
    //    }
    //    audioSource.volume = target1;
    //    audioSource.clip = ConverToClip(sound);
    //    audioSource.Play();

    //    float target2 = 1f;
    //    while (audioSource.volume < target2)
    //    {
    //        audioSource.volume += speed;
    //        yield return null;
    //    }
    //    audioSource.volume = target2;

    //}

    //public void ChangeVolume(float target)
    //{
    //    if (corChangeVolume != null)
    //    {
    //        StopCoroutine(corChangeVolume);
    //    }
    //    if (corChangeSound != null)
    //    {
    //        StopCoroutine(corChangeSound);
    //    }
    //    corChangeVolume = StartCoroutine(RunChangeVolume(target));
    //}
    //public void SetVolume(float target)
    //{
    //    audioSource.volume = target;
    //}
    //IEnumerator RunChangeVolume(float target)
    //{
    //    float speed = target - audioSource.volume;
    //    speed = speed / 20;
    //    if (speed > 0)
    //    {
    //        while (audioSource.volume < target)
    //        {
    //            audioSource.volume += speed;
    //            yield return null;
    //        }
    //    }
    //    else if (speed < 0)
    //    {
    //        while (audioSource.volume > target)
    //        {
    //            audioSource.volume += speed;
    //            yield return null;
    //        }
    //    }
    //    audioSource.volume = target;
    //}



    AudioClip ConverToClip(Music sound)
    { 
        if (musicDics.ContainsKey(sound))
            return musicDics[sound];
        return null;
    }

    /// <summary>
    /// V==1  mở âm thanh
    /// </summary>
    /// <param name="v"></param>
    public void Turn(bool isEnble)
    {
        audioSource.mute = !isEnble;
    }

    //public float Volume
    //{
    //    set
    //    {
    //        ChangeVolume(value);
    //    }
    //}
    //public void PlayMusic(Music ID)
    //{
    //    ChangeSound(ID);
    //}
}
