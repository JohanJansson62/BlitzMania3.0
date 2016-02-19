using System.Collections;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class WheelEffects : MonoBehaviour
{
    public Transform m_skidTrailPrefab;
    public static Transform m_skidTrailsDetachedParent;
    public ParticleSystem m_skidParticles;
    public bool Skidding { get; private set; }
    public bool PlayingAudio { get; private set; }


    private AudioSource m_audioSource;
    private Transform m_skidTrail;
    private WheelCollider m_wheelCollider;


    private void Start()
    {
        m_skidParticles = transform.root.GetComponentInChildren<ParticleSystem>();

        //if (skidParticles == null)
        //{
        //    Debug.LogWarning(" no particle system found on car to generate smoke particles");
        //}
        //else
        //{
        //    skidParticles.Stop();
        //}

        m_wheelCollider = GetComponent<WheelCollider>();
        m_audioSource = GetComponent<AudioSource>();
        PlayingAudio = false;

        if (m_skidTrailsDetachedParent == null)
        {
            m_skidTrailsDetachedParent = new GameObject("Skid Trails - Detached").transform;
        }
    }


    public void EmitTyreSmoke()
    {
        m_skidParticles.transform.position = transform.position - transform.up * m_wheelCollider.radius;
        if (!Skidding)
        {
            StartCoroutine(StartSkidTrail());
        }
    }


    public void PlayAudio()
    {
        m_audioSource.Play();
        PlayingAudio = true;
    }


    public void StopAudio()
    {
        m_audioSource.Stop();
        PlayingAudio = false;
    }


    public IEnumerator StartSkidTrail()
    {
        Skidding = true;
        m_skidTrail = Instantiate(m_skidTrailPrefab);
        while (m_skidTrail == null)
        {
            yield return null;
        }
        m_skidTrail.parent = transform;
        m_skidTrail.localPosition = -Vector3.up * m_wheelCollider.radius;
    }


    public void EndSkidTrail()
    {
        if (!Skidding)
        {
            return;
        }
        Skidding = false;
        m_skidTrail.parent = m_skidTrailsDetachedParent;
        Destroy(m_skidTrail.gameObject, 10);
    }
}
