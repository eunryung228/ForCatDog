using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name; // 사운드 이름
    public AudioClip clip; // 사운드 파일
    private AudioSource source; // 사운드 플레이어
    public bool loop;
    public float volume;

    public void SetSource(AudioSource src)
    {
        source = src;
        source.clip = clip;
        source.loop = loop;
        source.volume = volume;
    }

    public void play(float v)
    {
        source.volume = v;
        source.Play();
    }

    public void stop()
    {
        source.Stop();
    }
}

public class AudioManager : MonoBehaviour
{
    static public AudioManager Instance;
    private AudioSource m_source;

    [SerializeField]
    public Sound[] sounds;


    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
    }

    public void Play(string nm)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (nm == sounds[i].name)
            {
                sounds[i].play(sounds[i].volume);
                break;
            }
        }
    }

    public void Stop(string nm)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            if (nm == sounds[i].name)
            {
                sounds[i].stop();
            }
        }
    }

    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObject = new GameObject(sounds[i].name);
            sounds[i].SetSource(soundObject.AddComponent<AudioSource>());
            soundObject.transform.SetParent(this.transform);
        }
    }
}