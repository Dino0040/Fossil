using UnityEngine;
namespace Fossil
{
    public static class LayerMaskExtensions
    {
        static public bool ContainsLayer(this LayerMask self, int layer)
        {
            return (self.value & (1 << layer)) != 0;
        }

        static public LayerMask AddLayer(this LayerMask self, int layer)
        {
            return self | (1 << layer);
        }

        static public LayerMask RemoveLayer(this LayerMask self, int layer)
        {
            return self & ~(1 << layer);
        }
    }
}