using UnityEngine;

namespace Helpers.Mask
{
    internal class MetroidLayerMask
    {
        public LayerMask Mask;

        #region Constructors
        public MetroidLayerMask(params string[] layers)
        {
            Mask = LayerMask.GetMask(layers);
        }
        public MetroidLayerMask(int mask)
        {
            Mask = mask;
        }
        #endregion

        #region Modifiers
        public bool Contains(string layer) => Contains(LayerMask.NameToLayer(layer));
        public bool Contains(int layer)
        {
            return (Mask & (1 << layer)) != 0;
        }

        public void Add(string layer) => Add(LayerMask.NameToLayer(layer));
        public void Add(int layer)
        {
            Mask |= (1 << layer);
        }

        public void Remove(string layer) => Remove(LayerMask.NameToLayer(layer));
        public void Remove(int layer)
        {
            Mask &= ~(1 << layer);
        }
        #endregion

        #region Implicit Operators
        public static implicit operator MetroidLayerMask(int layer) => new MetroidLayerMask(layer);
        public static implicit operator MetroidLayerMask(string layer) => new MetroidLayerMask(layer);
        public static implicit operator MetroidLayerMask(string[] layers) => new MetroidLayerMask(layers);
        public static implicit operator int(MetroidLayerMask mask) => mask.Mask;
        #endregion
    }
}
