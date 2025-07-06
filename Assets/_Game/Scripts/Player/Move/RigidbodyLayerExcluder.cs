using UnityEngine;

namespace Descensus
{
    /// <summary>
    /// Its needed for when game over player falls off platforms and will not collide with it
    /// </summary>
    public class RigidbodyLayerExcluder
    {
        private readonly Rigidbody2D _rigidbody;

        public RigidbodyLayerExcluder(Rigidbody2D rigidbody)
        {
            _rigidbody = rigidbody;
        }

        public void ExcludeLayers(LayerMask mask)
        {
            _rigidbody.excludeLayers |= mask;
        }

        public void IncludeLayers(LayerMask mask)
        {
            _rigidbody.excludeLayers &= ~mask;
        }

        public void SetExcludedLayers(LayerMask mask)
        {
            _rigidbody.excludeLayers = mask;
        }

        public void ClearExcludedLayers()
        {
            _rigidbody.excludeLayers = 0; // equivalent to "Nothing"
        }

        public bool IsLayerExcluded(int layer)
        {
            return (_rigidbody.excludeLayers & (1 << layer)) != 0;
        }
    }
}
