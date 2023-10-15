// '1' empieza la reproducción del sonido intermitente
// '2' lo para

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Mixer : MonoBehaviour
{
    public AudioClip[] clips;
    public int maxInstances;
    private AudioSource[] sources;
    public float minTime, maxTime;
    public bool spatialBlend;

    [Range(0.0f, 1.0f)]
    public float IVolume;

    private bool enablePlayMode;
    // Start is called before the first frame update
    void Start()
    {
        sources = new AudioSource[maxInstances];

        for (int i = 0; i < maxInstances; i++)
        {
            if (spatialBlend)
            {
                GameObject gameObject = new GameObject($"AudioSource{i}");
                gameObject.transform.parent = transform;
                sources[i] = gameObject.AddComponent<AudioSource>();
            }
            else
            {
                sources[i] = gameObject.AddComponent<AudioSource>();
            }
            sources[i].playOnAwake = false;
            sources[i].loop = false;
            sources[i].spatialBlend = 0.7f;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!enablePlayMode)
        {
            Debug.Log("NotPlaying");
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                enablePlayMode = true;
                StartCoroutine("Waitforit");
            }
        }
        else if (enablePlayMode)
        {
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                enablePlayMode = false;
                Debug.Log("stop");
            }
        }

    }

    IEnumerator Waitforit()
    {
        // tiempo de espera aleatorio en el intervalo [minTime,maxTime]
        float waitTime = Random.Range(minTime, maxTime);
        Debug.Log($"Waittime: {waitTime}");

        // buscamos canal libre
        int ch = 0;
        while (ch < maxInstances && sources[ch].isPlaying) ch++;
        if (ch < maxInstances)
        { // si hay canal libre lo utilizamos
            int current = Random.Range(0, clips.Length);
            sources[ch].clip = clips[current];
            sources[ch].pitch = Random.Range(0.8f, 1.2f);
            sources[ch].volume = Random.Range(Mathf.Clamp(IVolume - 0.1f, 0f, 1f), Mathf.Clamp(IVolume + 0.1f, 0f, 1f));

            if (spatialBlend)
                sources[ch].transform.position = transform.parent.position + new Vector3(Random.Range(-5f, 5f), Random.Range(0, 5f), Random.Range(-5f, 5f));

            Debug.Log($"Canal {ch} Sample {current} pitch {sources[ch].pitch} volume {sources[ch].volume}");
        }
        else Debug.Log($"Canal no disponible");

        if (ch == maxInstances)
            // waitfor seconds suspende la coroutine durante waitTime
            yield return new WaitForSeconds(waitTime);

        // cuando hay clip se añade la long del clip + el tiempo de espera para esperar entre lanzamientos
        else
            yield return new WaitForSeconds(sources[ch].clip.length + waitTime);

        // si esta activado reproducimos sonido
        if (enablePlayMode)
        {
            sources[ch].Play();
            Debug.Log("back in it");
            StartCoroutine("Waitforit");
        }
    }
}
