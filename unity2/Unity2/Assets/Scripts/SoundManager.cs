using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip[] clips;
    public float minTime, maxTime;
    public float spatialBlend;
    public float Itraffic, Ichatter;
    public AudioSource[] sources;
    public AudioSource freeSource;
    public int numSorurces;
    // Start is called before the first frame update
    void Start()
    {
        sources = new AudioSource[numSorurces];
        for (int i = 0; i < numSorurces; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();
            sources[i].playOnAwake = false;
            sources[i].loop = false;
            sources[i].spatialBlend = 0.0f;
            sources[i].clip = clips[i];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            // buscamos canal libre
            int ch = 0;
            while (ch < maxInstances && sources[ch].isPlaying) ch++;
            if (ch < maxInstances)
            { // si hay canal libre lo utilizamos
                int current = Random.Range(0, clips.Length);
                sources[ch].clip = clips[current];
                sources[ch].pitch = Random.Range(0.8f, 1.2f);
                Debug.Log($"Canal {ch} Sample {current} pitch {sources[0].pitch}");
                sources[ch].Play();
            }
            else Debug.Log($"Canal no disponible");
        }
    }

    void PlaySound()
    {
        SetSourceProperties(clips[Random.Range(0, clips.Length)], minVol, maxVol, distRand, maxDist, spatialBlend);
        //_Speaker01.Play();
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
}
