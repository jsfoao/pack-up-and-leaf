using System;
using UnityEngine;

public class ParticleEmitter : MonoBehaviour
{
    [SerializeField][Tooltip("Avoid duplicate IDs")] 
    private Particle[] particles;
    
    public void Play(int id)
    {
        Particle particle = Lookup(id);
        if (particle == null) { return; }
        particle.ParticleSystem.Play();
    }
    
    public void Spawn(int id)
    {
        Particle particle = Lookup(id);
        if (particle == null) { return; }
        ParticleSystem instance = Instantiate(particle.ParticleSystem, transform.position, Quaternion.identity);
        instance.Play();
        Destroy(instance.gameObject, particle.timer);
    }

    private Particle Lookup(int id)
    {
        foreach (Particle particle in particles)
        {
            if (particle.ID == id)
            {
                if (particle.ParticleSystem == null) { return null; }
                return particle;
            }
        }
        return null;
    }

    private void OnValidate()
    {
        for (int i = 0; i < particles.Length; i++)
        {
            particles[i].ID = i;
        }
    }
}

[Serializable]
public class Particle
{
    public int ID;
    public ParticleSystem ParticleSystem;
    public float timer;

    public Particle()
    {
        timer = 1f;
    }
}
