using System;
using UnityEngine;
using Random = UnityEngine.Random;

[RequireComponent(typeof(CarController))]
public class CarAudio : MonoBehaviour
{
    // This script reads some of the car's current properties and plays sounds accordingly.
    // The engine sound can be a simple single clip which is looped and pitched, or it
    // can be a crossfaded blend of four clips which represent the timbre of the engine
    // at different RPM and Throttle state.

    // the engine clips should all be a steady pitch, not rising or falling.

    // when using four channel engine crossfading, the four clips should be:
    // lowAccelClip : The engine at low revs, with throttle open (i.e. begining acceleration at very low speed)
    // highAccelClip : Thenengine at high revs, with throttle open (i.e. accelerating, but almost at max speed)
    // lowDecelClip : The engine at low revs, with throttle at minimum (i.e. idling or engine-braking at very low speed)
    // highDecelClip : Thenengine at high revs, with throttle at minimum (i.e. engine-braking at very high speed)

    // For proper crossfading, the clips pitches should all match, with an octave offset between low and high.


    public enum EngineAudioOptions // Options for the engine audio
    {
        Simple, // Simple style audio
        FourChannel // four Channel audio
    }

    public EngineAudioOptions engineSoundStyle = EngineAudioOptions.FourChannel;// Set the default audio options to be four channel
    public AudioClip m_lowAccelClip;                                              // Audio clip for low acceleration
    public AudioClip m_lowDecelClip;                                              // Audio clip for low deceleration
    public AudioClip m_highAccelClip;                                             // Audio clip for high acceleration
    public AudioClip m_highDecelClip;                                             // Audio clip for high deceleration
    public float m_pitchMultiplier = 1f;                                          // Used for altering the pitch of audio clips
    public float m_lowPitchMin = 1f;                                              // The lowest possible pitch for the low sounds
    public float m_lowPitchMax = 6f;                                              // The highest possible pitch for the low sounds
    public float m_highPitchMultiplier = 0.25f;                                   // Used for altering the pitch of high sounds
    public float m_maxRolloffDistance = 500;                                      // The maximum distance where rollof starts to take place
    public float m_dopplerLevel = 1;                                              // The mount of doppler effect used in the audio
    public bool m_useDoppler = true;                                              // Toggle for using doppler

    private AudioSource m_lowAccel; // Source for the low acceleration sounds
    private AudioSource m_lowDecel; // Source for the low deceleration sounds
    private AudioSource m_highAccel; // Source for the high acceleration sounds
    private AudioSource m_highDecel; // Source for the high deceleration sounds
    private bool m_startedSound; // flag for knowing if we have started sounds
    private CarController m_carController; // Reference to car we are controlling


    private void StartSound()
    {
        // get the carcontroller ( this will not be null as we have require component)
        m_carController = GetComponent<CarController>();

        // setup the simple audio source
        m_highAccel = SetUpEngineAudioSource(m_highAccelClip);

        // if we have four channel audio setup the four audio sources
        if (engineSoundStyle == EngineAudioOptions.FourChannel)
        {
            m_lowAccel = SetUpEngineAudioSource(m_lowAccelClip);
            m_lowDecel = SetUpEngineAudioSource(m_lowDecelClip);
            m_highDecel = SetUpEngineAudioSource(m_highDecelClip);
        }

        // flag that we have started the sounds playing
        m_startedSound = true;
    }


    private void StopSound()
    {
        //Destroy all audio sources on this object:
        foreach (var source in GetComponents<AudioSource>())
        {
            Destroy(source);
        }

        m_startedSound = false;
    }


    // Update is called once per frame
    private void Update()
    {
        // get the distance to main camera
        float camDist = (Camera.main.transform.position - transform.position).sqrMagnitude;

        // stop sound if the object is beyond the maximum roll off distance
        if (m_startedSound && camDist > m_maxRolloffDistance * m_maxRolloffDistance)
        {
            StopSound();
        }

        // start the sound if not playing and it is nearer than the maximum distance
        if (!m_startedSound && camDist < m_maxRolloffDistance * m_maxRolloffDistance)
        {
            StartSound();
        }

        if (m_startedSound)
        {
            // The pitch is interpolated between the min and max values, according to the car's revs.
            float pitch = ULerp(m_lowPitchMin, m_lowPitchMax, m_carController.Revs);

            // clamp to minimum pitch (note, not clamped to max for high revs while burning out)
            pitch = Mathf.Min(m_lowPitchMax, pitch);

            if (engineSoundStyle == EngineAudioOptions.Simple)
            {
                // for 1 channel engine sound, it's oh so simple:
                m_highAccel.pitch = pitch * m_pitchMultiplier * m_highPitchMultiplier;
                m_highAccel.dopplerLevel = m_useDoppler ? m_dopplerLevel : 0;
                m_highAccel.volume = 1;
            }
            else
            {
                // for 4 channel engine sound, it's a little more complex:

                // adjust the pitches based on the multipliers
                m_lowAccel.pitch = pitch * m_pitchMultiplier;
                m_lowDecel.pitch = pitch * m_pitchMultiplier;
                m_highAccel.pitch = pitch * m_highPitchMultiplier * m_pitchMultiplier;
                m_highDecel.pitch = pitch * m_highPitchMultiplier * m_pitchMultiplier;

                // get values for fading the sounds based on the acceleration
                float accFade = Mathf.Abs(m_carController.AccelInput);
                float decFade = 1 - accFade;

                // get the high fade value based on the cars revs
                float highFade = Mathf.InverseLerp(0.2f, 0.8f, m_carController.Revs);
                float lowFade = 1 - highFade;

                // adjust the values to be more realistic
                highFade = 1 - ((1 - highFade) * (1 - highFade));
                lowFade = 1 - ((1 - lowFade) * (1 - lowFade));
                accFade = 1 - ((1 - accFade) * (1 - accFade));
                decFade = 1 - ((1 - decFade) * (1 - decFade));

                // adjust the source volumes based on the fade values
                m_lowAccel.volume = lowFade * accFade;
                m_lowDecel.volume = lowFade * decFade;
                m_highAccel.volume = highFade * accFade;
                m_highDecel.volume = highFade * decFade;

                // adjust the doppler levels
                m_highAccel.dopplerLevel = m_useDoppler ? m_dopplerLevel : 0;
                m_lowAccel.dopplerLevel = m_useDoppler ? m_dopplerLevel : 0;
                m_highDecel.dopplerLevel = m_useDoppler ? m_dopplerLevel : 0;
                m_lowDecel.dopplerLevel = m_useDoppler ? m_dopplerLevel : 0;
            }
        }
    }


    // sets up and adds new audio source to the gane object
    private AudioSource SetUpEngineAudioSource(AudioClip clip)
    {
        // create the new audio source component on the game object and set up its properties
        AudioSource source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = 0;
        source.loop = true;

        // start the clip from a random point
        source.time = Random.Range(0f, clip.length);
        source.Play();
        source.minDistance = 5;
        source.maxDistance = m_maxRolloffDistance;
        source.dopplerLevel = 0;
        return source;
    }


    // unclamped versions of Lerp and Inverse Lerp, to allow value to exceed the from-to range
    private static float ULerp(float from, float to, float value)
    {
        return (1.0f - value) * from + value * to;
    }
}
