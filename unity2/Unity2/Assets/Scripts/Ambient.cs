using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ambient : MonoBehaviour
{
    [SerializeField]
    [Range(0.0f, 1.0f)]
    private float ITraffic = 0.1f, IChatter = 0.1f;

    private Mixer passing, train, horn, siren, chatter;

    [SerializeField]
    private float minTime = 1f, maxTime = 3f;

    [SerializeField]
    public float minVol = 0.1f, maxVol = 0.7f;

    [SerializeField]
    private int maxInstancesPerMixer = 8;

    [SerializeField]
    private AudioClip[] passingClips, trainClips, hornClips, sirenClips, chatterClips;

    private AudioSource traffic_pad, chatter_pad;

    [SerializeField]
    private AudioClip traffic_pad_clip, chatter_pad_clip;

    [SerializeField]
    private float probabilityMultiplier = 0.05f, volumeMultiplier = 0.1f;
    void Awake()
    {
        passing = SetMixer(passingClips, ITraffic);
        train = SetMixer(trainClips, ITraffic);
        horn = SetMixer(hornClips, ITraffic);
        siren = SetMixer(sirenClips, ITraffic);
        chatter = SetMixer(chatterClips, IChatter);

        traffic_pad = gameObject.AddComponent<AudioSource>();
        traffic_pad.playOnAwake = true;
        traffic_pad.loop = true;
        traffic_pad.volume = ITraffic;
        traffic_pad.clip = traffic_pad_clip;
        traffic_pad.Play();

        chatter_pad = gameObject.AddComponent<AudioSource>();
        chatter_pad.playOnAwake = true;
        chatter_pad.loop = true;
        chatter_pad.volume = IChatter;
        chatter_pad.clip = chatter_pad_clip;
        chatter_pad.Play();

    }


    void Update()
    {
        Traffic();

        Chatter();

    }

    private void Chatter()
    {
        chatter_pad.volume = IChatter;

        if (IChatter < 0.5)
            chatter.Stop(); //para el mixer

        else //>=0.5
        {
            //aumenta el volumen y la proobabilidad de que suene
            chatter.minVol = Mathf.Clamp(minVol + IChatter * volumeMultiplier, 0.0f, 1.0f);
            chatter.maxVol = Mathf.Clamp(maxVol + IChatter * volumeMultiplier, 0.0f, 1.0f);
            chatter.minTime = minTime - IChatter * probabilityMultiplier;
            chatter.maxTime = maxTime - IChatter * probabilityMultiplier;

            //reproduce
            chatter.Play();
        }
    }

    private void Traffic()
    {
        traffic_pad.volume = ITraffic;

        if (ITraffic < 0.2)
        {
            //para los mixers
            passing.Stop();
            train.Stop();

        }
        else // >= 0.2
        {
            //aumenta volumen y probabilidad de que suenen
            passing.minVol = Mathf.Clamp(minVol + ITraffic * volumeMultiplier, 0.0f, 1.0f);
            passing.maxVol = Mathf.Clamp(maxVol + ITraffic * volumeMultiplier, 0.0f, 1.0f);
            passing.minTime = minTime - ITraffic * probabilityMultiplier;
            passing.maxTime = maxTime - ITraffic * probabilityMultiplier;

            train.minVol = Mathf.Clamp(minVol + ITraffic * volumeMultiplier, 0.0f, 1.0f);
            train.maxVol = Mathf.Clamp(maxVol + ITraffic * volumeMultiplier, 0.0f, 1.0f);
            train.minTime = minTime - ITraffic * probabilityMultiplier / 2;
            train.maxTime = maxTime - ITraffic * probabilityMultiplier / 2;

            //reproduce
            passing.Play();
            train.Play();
        }

        if (ITraffic < 0.5)
        {
            //para los mixers
            horn.Stop();
            siren.Stop();
        }
        else //>=0.5
        {
            //aumenta probabilidad de que suenen
            horn.minTime = minTime - ITraffic * probabilityMultiplier;
            horn.maxTime = maxTime - ITraffic * probabilityMultiplier;

            siren.minTime = minTime - ITraffic * probabilityMultiplier;
            siren.maxTime = maxTime - ITraffic * probabilityMultiplier;

            //reproduce
            horn.Play();
            siren.Play();
        }
    }

    private Mixer SetMixer(AudioClip[] clips, float intensity)
    {
        Mixer mixer = gameObject.AddComponent<Mixer>();
        mixer.clips = clips;
        mixer.maxInstances = maxInstancesPerMixer;
        mixer.minTime = minTime - intensity * probabilityMultiplier;
        mixer.maxTime = maxTime - intensity * probabilityMultiplier;
        mixer.minVol = Mathf.Clamp(minVol + intensity * volumeMultiplier, 0.0f, 1.0f);
        mixer.maxVol = Mathf.Clamp(maxVol + intensity * volumeMultiplier, 0.0f, 1.0f);
        return mixer;

    }
}
