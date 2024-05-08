using UnityEngine;

public interface IPlayParticleSystem
{
    public void PlayParticleSystem(ParticleSystem instantiateParticleSystem,Vector3 positionToPlay, Quaternion rotation, Vector3 particleScale);
}

