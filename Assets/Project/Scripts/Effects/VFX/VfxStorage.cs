using UnityEngine;

namespace Project.Scripts.Effects.VFX
{
    [CreateAssetMenu(fileName = "VfxStorage", menuName = "Storage/VfxStorage", order = 2)]
    public class VfxStorage : ScriptableObject
    {
        public ParticleRoot[] BuildingsBuyVfx;
        public ParticleRoot[] WorkersBuyVfx;
    }
}