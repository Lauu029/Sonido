//Clase mixer para mezclar varias pistas de audio

using System.Collections;
using UnityEngine;
public class Mixer : MonoBehaviour
{
    //clips a mezclar
    public AudioClip[] clips;

    //numero maximo de instancias de canales (audio sources)
    public int maxInstances = 8;

    //array de canales
    private AudioSource[] sources;

    //intervalo de tiempo entre sonidos
    [Range(1f, 3f)]
    public float minTime = 1f, maxTime = 3f;

    //volumen de los sonidos
    [Range(0f, 1f)]
    public float minVol = 0.1f, maxVol = 0.7f;

    //flag para activar/desactivar el modo de reproduccion
    private bool enablePlayMode;

    void Start()
    {
        //inicializamos el array de canales
        sources = new AudioSource[maxInstances];

        for (int i = 0; i < maxInstances; i++)
        {
            sources[i] = gameObject.AddComponent<AudioSource>();

            sources[i].playOnAwake = false;
            sources[i].loop = false;
        }
    }

    //Esto en caso de que quisieramos activar/desactivar el modo de reproduccion con teclas
    // void Update()
    // {

    //     if (Input.GetKeyDown(KeyCode.Alpha1) && !enablePlayMode)
    //     {
    //         enablePlayMode = true;
    //         StartCoroutine("Waitforit");
    //     }

    //     else if (Input.GetKeyDown(KeyCode.Alpha2) && enablePlayMode)
    //     {
    //         enablePlayMode = false;
    //         StopCoroutine("Waitforit");
    //     }

    // }

    //corutina para reproducir sonidos
    IEnumerator Waitforit()
    {
        // tiempo de espera aleatorio en el intervalo [minTime,maxTime]
        float waitTime = Random.Range(minTime, maxTime);
        yield return new WaitForSeconds(waitTime);

        // buscamos canal libre
        int ch = 0;
        while (ch < maxInstances && sources[ch].isPlaying) ch++;
        if (ch < maxInstances)
        { // si hay canal libre lo utilizamos
            int current = Random.Range(0, clips.Length);
            sources[ch].clip = clips[current];
            sources[ch].pitch = Random.Range(0.8f, 1.2f);
            sources[ch].volume = Random.Range(minVol, maxVol);
            sources[ch].panStereo = Random.Range(-1f, 1f);
            sources[ch].Play();
        }

        if (enablePlayMode)
            StartCoroutine("Waitforit");
    }

    public void Play()
    {
        if (enablePlayMode) return;
        enablePlayMode = true;
        StartCoroutine("Waitforit");
    }

    public void Stop()
    {
        if (!enablePlayMode) return;
        enablePlayMode = false;
        StopCoroutine("Waitforit");
    }
}

