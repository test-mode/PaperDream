using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Audio;

namespace PaperDream
{
    public class BaseRainScript : MonoBehaviour
    {
        public Camera Camera;
        public bool FollowCamera = true;

        public AudioClip RainSoundLight;
        public AudioClip RainSoundMedium;
        public AudioClip RainSoundHeavy;
        public AudioMixerGroup RainSoundAudioMixer;

        [Range(0.0f, 1.0f)]
        public float RainIntensity;

        public ParticleSystem RainFallParticleSystem;
        public ParticleSystem RainExplosionParticleSystem;
        public ParticleSystem RainMistParticleSystem;

        [Range(0.0f, 1.0f)]
        public float RainMistThreshold = 0.5f;

        protected LoopingAudioSource audioSourceRainLight;
        protected LoopingAudioSource audioSourceRainMedium;
        protected LoopingAudioSource audioSourceRainHeavy;
        protected LoopingAudioSource audioSourceRainCurrent;

        protected Material rainMaterial;
        protected Material rainExplosionMaterial;
        protected Material rainMistMaterial;

        private float lastRainIntensityValue = -1.0f;

        private void CheckForRainChange()
        {
            if (lastRainIntensityValue != RainIntensity)
            {
                lastRainIntensityValue = RainIntensity;
                if (RainIntensity <= 0.01f)
                {
                    if (audioSourceRainCurrent != null)
                    {
                        audioSourceRainCurrent.Stop();
                        audioSourceRainCurrent = null;
                    }
                    if (RainFallParticleSystem != null)
                    {
                        ParticleSystem.EmissionModule e = RainFallParticleSystem.emission;
                        e.enabled = false;
                        RainFallParticleSystem.Stop();
                    }
                    if (RainMistParticleSystem != null)
                    {
                        ParticleSystem.EmissionModule e = RainMistParticleSystem.emission;
                        e.enabled = false;
                        RainMistParticleSystem.Stop();
                    }
                }
                else
                {
                    LoopingAudioSource newSource;
                    if (RainIntensity >= 0.67f)
                    {
                        newSource = audioSourceRainHeavy;
                    }
                    else if (RainIntensity >= 0.33f)
                    {
                        newSource = audioSourceRainMedium;
                    }
                    else
                    {
                        newSource = audioSourceRainLight;
                    }
                    if (audioSourceRainCurrent != newSource)
                    {
                        if (audioSourceRainCurrent != null)
                        {
                            audioSourceRainCurrent.Stop();
                        }
                        audioSourceRainCurrent = newSource;
                        audioSourceRainCurrent.Play(0.05f);
                    }
                    if (RainFallParticleSystem != null)
                    {
                        ParticleSystem.EmissionModule e = RainFallParticleSystem.emission;
                        e.enabled = RainFallParticleSystem.GetComponent<Renderer>().enabled = true;
                        if (!RainFallParticleSystem.isPlaying)
                        {
                            RainFallParticleSystem.Play();
                        }
                        ParticleSystem.MinMaxCurve rate = e.rateOverTime;
                        rate.mode = ParticleSystemCurveMode.Constant;
                        rate.constantMin = rate.constantMax = RainFallEmissionRate();
                        e.rateOverTime = rate;
                    }
                    if (RainMistParticleSystem != null)
                    {
                        ParticleSystem.EmissionModule e = RainMistParticleSystem.emission;
                        e.enabled = RainMistParticleSystem.GetComponent<Renderer>().enabled = true;
                        if (!RainMistParticleSystem.isPlaying)
                        {
                            RainMistParticleSystem.Play();
                        }
                        float emissionRate;
                        if (RainIntensity < RainMistThreshold)
                        {
                            emissionRate = 0.0f;
                        }
                        else
                        {
                            // must have RainMistThreshold or higher rain intensity to start seeing mist
                            emissionRate = MistEmissionRate();
                        }
                        ParticleSystem.MinMaxCurve rate = e.rateOverTime;
                        rate.mode = ParticleSystemCurveMode.Constant;
                        rate.constantMin = rate.constantMax = emissionRate;
                        e.rateOverTime = rate;
                    }
                }
            }
        }

        protected virtual void Start()
        {

#if DEBUG

            if (RainFallParticleSystem == null)
            {
                Debug.LogError("Rain fall particle system must be set to a particle system");
                return;
            }

#endif

            if (Camera == null)
            {
                Camera = Camera.main;
            }

            audioSourceRainLight = new LoopingAudioSource(this, RainSoundLight, RainSoundAudioMixer);
            audioSourceRainMedium = new LoopingAudioSource(this, RainSoundMedium, RainSoundAudioMixer);
            audioSourceRainHeavy = new LoopingAudioSource(this, RainSoundHeavy, RainSoundAudioMixer);

            if (RainFallParticleSystem != null)
            {
                ParticleSystem.EmissionModule e = RainFallParticleSystem.emission;
                e.enabled = false;
                Renderer rainRenderer = RainFallParticleSystem.GetComponent<Renderer>();
                rainRenderer.enabled = false;
                rainMaterial = new Material(rainRenderer.material);
                rainMaterial.EnableKeyword("SOFTPARTICLES_OFF");
                rainRenderer.material = rainMaterial;
            }
            if (RainExplosionParticleSystem != null)
            {
                ParticleSystem.EmissionModule e = RainExplosionParticleSystem.emission;
                e.enabled = false;
                Renderer rainRenderer = RainExplosionParticleSystem.GetComponent<Renderer>();
                rainExplosionMaterial = new Material(rainRenderer.material);
                rainExplosionMaterial.EnableKeyword("SOFTPARTICLES_OFF");
                rainRenderer.material = rainExplosionMaterial;
            }
            if (RainMistParticleSystem != null)
            {
                ParticleSystem.EmissionModule e = RainMistParticleSystem.emission;
                e.enabled = false;
                Renderer rainRenderer = RainMistParticleSystem.GetComponent<Renderer>();
                rainRenderer.enabled = false;
                rainMistMaterial = new Material(rainRenderer.material);
                if (UseRainMistSoftParticles)
                {
                    rainMistMaterial.EnableKeyword("SOFTPARTICLES_ON");
                }
                else
                {
                    rainMistMaterial.EnableKeyword("SOFTPARTICLES_OFF");
                }
                rainRenderer.material = rainMistMaterial;
            }
        }

        protected virtual void Update()
        {

#if DEBUG

            if (RainFallParticleSystem == null)
            {
                Debug.LogError("Rain fall particle system must be set to a particle system");
                return;
            }

#endif

            CheckForRainChange();
            audioSourceRainLight.Update();
            audioSourceRainMedium.Update();
            audioSourceRainHeavy.Update();
        }

        protected virtual float RainFallEmissionRate()
        {
            return (RainFallParticleSystem.main.maxParticles / RainFallParticleSystem.main.startLifetime.constant) * RainIntensity;
        }

        protected virtual float MistEmissionRate()
        {
            return (RainMistParticleSystem.main.maxParticles / RainMistParticleSystem.main.startLifetime.constant) * RainIntensity * RainIntensity;
        }

        protected virtual bool UseRainMistSoftParticles
        {
            get
            {
                return true;
            }
        }
    }

    public class LoopingAudioSource
    {
        public AudioSource AudioSource { get; private set; }
        public float TargetVolume { get; private set; }

        public LoopingAudioSource(MonoBehaviour script, AudioClip clip, AudioMixerGroup mixer)
        {
            AudioSource = script.gameObject.AddComponent<AudioSource>();

            if (mixer != null)
            {
                AudioSource.outputAudioMixerGroup = mixer;
            }

            AudioSource.loop = true;
            AudioSource.clip = clip;
            AudioSource.playOnAwake = false;
            AudioSource.volume = 0.0f;
            AudioSource.Stop();
            TargetVolume = 0.05f;
        }

        public void Play(float targetVolume)
        {
            if (!AudioSource.isPlaying)
            {
                AudioSource.volume = 0.0f;
                AudioSource.Play();
            }
            TargetVolume = targetVolume;
        }

        public void Stop()
        {
            TargetVolume = 0.0f;
        }

        public void Update()
        {
            if (AudioSource.isPlaying && (AudioSource.volume = Mathf.Lerp(AudioSource.volume, TargetVolume, Time.deltaTime)) == 0.0f)
            {
                AudioSource.Stop();
            }
        }
    }
}