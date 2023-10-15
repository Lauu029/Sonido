using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class IntermitentSound : MonoBehaviour
{
    [Range(0f, 30f)]
    public float minTime, maxTime;  // intervalo temporal de lanzamiento
    [Range(0f, 1.1f)]
    public float spatialBlend;
    public AudioClip[] pcmData;
    public bool enablePlayMode;

    //traffic_pad
    public AudioClip traffic_pad;
    public AudioClip[] passing_;
    public AudioClip[] train_;
    public AudioClip[] horn_siren;

    public AudioSource traffic_pad_speaker;
    public AudioSource[] passing_speaker;
    public AudioSource[] train_speaker;
    public AudioSource[] horn_speaker;

    //chatter_pad
    public AudioClip chatter_pad;
    public AudioClip[] chatter_;

    public AudioSource chatter_pad_speaker;
    public AudioSource[] chatter_speakers;

    [Range(0.0f, 1.0f)]
    public float Itraffic, Ichatter;

    void Awake()
    {
        passing_speaker = new AudioSource[passing_.Length];
        train_speaker = new AudioSource[train_.Length];
        horn_speaker = new AudioSource[horn_siren.Length];

        chatter_speakers = new AudioSource[chatter_.Length];

    }

    void Start()
    {
        traffic_pad_speaker = gameObject.AddComponent<AudioSource>();
        traffic_pad_speaker.playOnAwake = true;
        traffic_pad_speaker.loop = false;
        traffic_pad_speaker.spatialBlend = 0.0f;
        traffic_pad_speaker.clip = traffic_pad;
        traffic_pad_speaker.volume = Itraffic;

        chatter_pad_speaker = gameObject.AddComponent<AudioSource>();
        chatter_pad_speaker.playOnAwake = true;
        chatter_pad_speaker.loop = false;
        chatter_pad_speaker.spatialBlend = 0.0f;
        chatter_pad_speaker.clip = chatter_pad;
        chatter_pad_speaker.volume = Ichatter;

        for (int i = 0; i < passing_speaker.Length; i++)
        {
            passing_speaker[i] = gameObject.AddComponent<AudioSource>();
            passing_speaker[i].playOnAwake = false;
            passing_speaker[i].volume = 0.1f;
        }

        for (int i = 0; i < train_speaker.Length; i++)
        {
            train_speaker[i] = gameObject.AddComponent<AudioSource>();
            train_speaker[i].playOnAwake = false;
            train_speaker[i].volume = 0.1f;
        }

        for (int i = 0; i < horn_speaker.Length; i++)
        {
            horn_speaker[i] = gameObject.AddComponent<AudioSource>();
            horn_speaker[i].playOnAwake = false;
            horn_speaker[i].volume = 0.1f;
        }

        for (int i = 0; i < chatter_speakers.Length; i++)
        {
            chatter_speakers[i] = gameObject.AddComponent<AudioSource>();
            chatter_speakers[i].playOnAwake = false;
            chatter_speakers[i].volume = 0.1f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //if (!enablePlayMode)
        //{
        //    Debug.Log("NotPlaying");
        //    if (Input.GetKeyDown(KeyCode.Alpha1))
        //    {
        //        enablePlayMode = true;
        //        StartCoroutine("Waitforit");
        //    }
        //}
        //else if (enablePlayMode)
        //    if (Input.GetKeyDown(KeyCode.Alpha2)) StopSound();
    }

    IEnumerator traffic()
    {
        if (Itraffic < 0.2)
        {
            foreach (AudioSource source in passing_speaker)
            {
                if (source.isPlaying)
                    source.Stop();
            }

            foreach (AudioSource source in train_speaker)
            {
                if (source.isPlaying)
                    source.Stop();
            }
        }
        else // >= 0.2
        {
            foreach (AudioSource source in passing_speaker)
            {
                if (source.isPlaying)
                    source.volume += 0.05f;
            }

            foreach (AudioSource source in train_speaker)
            {
                if (source.isPlaying)
                    source.volume += 0.05f;
            }
        }

        if (Itraffic < 0.5)
        {
            foreach (AudioSource source in horn_speaker)
            {
                if (source.isPlaying)
                    source.Stop();
            }
        }
        //else //>=0.5
        //{

        //}
    }

    IEnumerator Waitforit()
    {
        // tiempo de espera aleatorio en el intervalo [minTime,maxTime]
        float waitTime = Random.Range(minTime, maxTime);
        Debug.Log(waitTime);

        // miramos si hay un clip asignado al source (sirve para la primera vez q se ejecuta)
        if (_Speaker01.clip == null)
            // waitfor seconds suspende la coroutine durante waitTime
            yield return new WaitForSeconds(waitTime);

        // cuando hay clip se añade la long del clip + el tiempo de espera para esperar entre lanzamientos
        else
            yield return new WaitForSeconds(_Speaker01.clip.length + waitTime);

        // si esta activado reproducimos sonido
        if (enablePlayMode) PlaySound();
    }

    void PlaySound()
    {
        SetSourceProperties(pcmData[Random.Range(0, pcmData.Length)], minVol, maxVol, distRand, maxDist, spatialBlend);
        _Speaker01.Play();
        Debug.Log("back in it");
        StartCoroutine("Waitforit");
    }



    public void SetSourceProperties(AudioSource audioSource, AudioClip audioData, float SpatialBlend, float volume)
    {
        audioSource.loop = false;
        audioSource.spatialBlend = spatialBlend;
        audioSource.clip = audioData;
        audioSource.volume = volume;
    }




    void StopSound()
    {
        enablePlayMode = false;
        Debug.Log("stop");
    }
}

