using System.Collections;
using UnityEngine;

public class BGMManager : MonoBehaviour
{
    static public BGMManager instance;
    public AudioClip[] clips;
    private AudioSource source;
    private WaitForSeconds waitTime = new WaitForSeconds(0.01f);


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
    }

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    public void Play(int track)
    {
        StopAllCoroutines();
        source.clip = clips[track];
        source.Play();
        StartCoroutine(FadeInMusicCoroutine());
    }

    public void Stop()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    public void FadeInMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeInMusicCoroutine());
    }

    IEnumerator FadeInMusicCoroutine()
    {
        for (float i = 0f; i <= 1.0f; i += 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    public void FadeOutMusic()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOutMusicCoroutine());
    }

    IEnumerator FadeOutMusicCoroutine()
    {
        for (float i = 1.0f; i >= 0f; i -= 0.01f)
        {
            source.volume = i;
            yield return waitTime;
        }
    }

    public void ChangeMusic(int t)
    {
        StartCoroutine(ChangeMusicCoroutine(track: t));
    }

    IEnumerator ChangeMusicCoroutine(int track)
    {
        yield return StartCoroutine(FadeOutMusicCoroutine());
        Play(track);
    }
}