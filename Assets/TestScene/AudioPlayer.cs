using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using Assets.Global;

public class AudioPlayer: MonoBehaviour
{
    [SerializeField]
    AudioSource audioSourceAsset;
    [SerializeField]
    int sourcesAmount;

    Transform listener;

    Queue<AudioSource> sources;

    bool soundOn;

    Dictionary<string, AudioSource> loopAudio = new Dictionary<string, AudioSource>();

    void Start()
    {
        soundOn = bool.Parse(GameConfig.Get("Sound", "true"));

        sources = new Queue<AudioSource>();
        for (int i = 0; i < sourcesAmount; i++)
            sources.Enqueue((AudioSource)Instantiate(audioSourceAsset, transform));

    }
    public void Play(string file, Vector3 position)
    {
        if (soundOn)
        {
            var s = sources.Dequeue();
            s.Stop();
            s.transform.position = position;
            s.clip = Resources.Load<AudioClip>(@"Audio\" + file);
            s.Play();
            sources.Enqueue(s);
        }
    }

    public void PlayLoop(string file, Vector3 position)
    {
        if (soundOn)
        {
            AudioSource s;
            if (loopAudio.TryGetValue(file, out s))
            {
                s.transform.position = position;
            }
            else
            {
                s = (AudioSource)Instantiate(audioSourceAsset, transform);
                s.clip = Resources.Load<AudioClip>(@"Audio\" + file);
                s.loop = true;
                s.Play();
                loopAudio.Add(file, s);
            }
        }
    }
    public void StopLoop(string file)
    {
        AudioSource s;
        if (loopAudio.TryGetValue(file, out s))
        {
            s.Stop();
            Destroy(s.gameObject);
            loopAudio.Remove(file);
        }
    }

    public bool IsListener(Transform transform)
    {
        return ShipIdentification.IsThisShip(transform, listener);
    }

    public void SetListener(Transform transform)
    {
        listener = transform;
    }
}

