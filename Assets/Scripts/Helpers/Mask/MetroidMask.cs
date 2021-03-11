using JetBrains.Annotations;
using UnityEngine;

namespace Helpers.Mask
{
    class MetroidMask
    {
        #region Cached Comparisons
        public bool HasBoth => _hasBoth ??= HasTag && HasLayer;
        private bool? _hasBoth;
        public bool HasEither => _hasEither ??= HasTag || HasLayer;
        private bool? _hasEither;
        public bool HasLayer => _hasLayer ??= LayerMask.Contains(_go.layer);
        private bool? _hasLayer;
        public bool HasTag => _hasTag ??= TagMask.Contains(_go.tag);
        private bool? _hasTag;
        #endregion

        #region Fields

        public readonly MetroidLayerMask LayerMask;
        public readonly MetroidTagMask TagMask;

        public GameObject GO
        {
            get => _go;
            set
            {
                _resetCache();
                _go = value;
            }
        }
        private GameObject _go;
        #endregion
        
        #region Constructors
        public MetroidMask([CanBeNull] MetroidLayerMask layerMask, [CanBeNull] MetroidTagMask tagMask = null, GameObject go = null)
        {
            LayerMask = layerMask ?? ~0;
            TagMask = tagMask ?? new string[]{};
            _go = go;
        }
        #endregion

        #region Modifiers
        public void AddLayer(string layer) => LayerMask.Add(layer);
        public void AddLayer(int layer) => LayerMask.Add(layer);
        public void AddTag(string tag) => LayerMask.Add(tag);

        public void RemoveLayer(string layer) => LayerMask.Remove(layer);
        public void RemoveLayer(int layer) => LayerMask.Remove(layer);
        public void RemoveTag(string tag) => TagMask.Remove(tag);

        private void _resetCache()
        {
            _hasBoth = null;
            _hasEither = null;
            _hasLayer = null;
            _hasTag = null;
        }
        #endregion

        #region Implicit Operators
        public static implicit operator MetroidLayerMask(MetroidMask mask) => mask.LayerMask;
        public static implicit operator MetroidTagMask(MetroidMask mask) => mask.TagMask;
        #endregion
    }
}
