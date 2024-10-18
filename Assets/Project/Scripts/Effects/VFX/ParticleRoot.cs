using UnityEngine;

namespace Project.Scripts.Effects.VFX
{
    public class ParticleRoot : MonoBehaviour
    {
        [SerializeField] private ParticleSystem rootParticle;

        public ParticleSystem Particle => rootParticle;
    }
}