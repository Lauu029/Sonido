using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SchedEvent : MonoBehaviour
{
    private AudioSource head;
    private AudioSource tail;
    private AudioSource end;
    //public AudioClip sound01, sound02;
    public AudioClip[] pcmDataHeads, pcmDataTails, pcmCasing;
    private int nHeads, nTails, nCasing;
    public float overlapTime = 0.2f;

    void Awake()
    {
        nHeads = pcmDataHeads.Length;
        nTails = pcmDataTails.Length;
        nCasing = pcmCasing.Length;
        head = gameObject.AddComponent<AudioSource>();
        tail = gameObject.AddComponent<AudioSource>();
        end = gameObject.AddComponent<AudioSource>();
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            int h = Random.Range(0, nHeads), t = Random.Range(0, nTails), e = Random.Range(0, nCasing); ;
            head.clip = pcmDataHeads[h];
            tail.clip = pcmDataTails[t];
            end.clip = pcmCasing[e];

            double clipLength = (double)head.clip.samples / head.pitch;
            double clipLength2 = (double)tail.clip.samples / tail.pitch;

            int sRATE = AudioSettings.outputSampleRate;
            Debug.Log($"head {h} length {clipLength}  p tail {t}  sRATE: {sRATE}");

            FadeOut(head.clip);
            FadeIn(tail.clip);
            head.Play();
            tail.PlayScheduled(AudioSettings.dspTime + clipLength / sRATE);
            end.PlayScheduled(AudioSettings.dspTime + clipLength2 / sRATE + clipLength / sRATE);
        }
    }
    void FadeIn(AudioClip clip)
    {
        int sRATE = AudioSettings.outputSampleRate;
        int samplesOverlapTime = Mathf.RoundToInt(sRATE * overlapTime);

        float[] data = new float[samplesOverlapTime];

        int pos = clip.samples - samplesOverlapTime;

        clip.GetData(data, pos);

        for (int i = 0; i < data.Length; i++)
        {
            float tiempo = (float)i / sRATE;
            data[i] = Mathf.Sqrt(tiempo / overlapTime);
        }

        clip.SetData(data, pos);
    }
    void FadeOut(AudioClip clip)
    {
        int sRATE = AudioSettings.outputSampleRate;
        int samplesOverlapTime = Mathf.RoundToInt(sRATE * overlapTime);

        float[] data = new float[samplesOverlapTime];

        int pos = clip.samples - samplesOverlapTime;

        clip.GetData(data, pos);

        for (int i = 0; i < data.Length; i++)
        {
            float tiempo = (float)i / sRATE;
            data[i] = Mathf.Sqrt((overlapTime - tiempo) / overlapTime);
        }

        clip.SetData(data, pos);
    }
}
