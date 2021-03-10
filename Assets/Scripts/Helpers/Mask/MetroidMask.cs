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
        public bool HasLayer => _hasLayer ??= _containsLayer(_go.layer);
        private bool? _hasLayer;
        public bool HasTag => _hasTag ??= _containsTag(_go.tag);
        private bool? _hasTag;
        #endregion

        #region Fields

        private readonly MetroidLayerMask _layerMask;
        private readonly MetroidTagMask _tagMask;

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
            _layerMask = layerMask ?? ~0;
            _tagMask = tagMask ?? new string[]{};
            _go = go;
        }
        #endregion

        #region Modifiers
        public void AddLayer(string layer) => _layerMask.Add(layer);
        public void AddLayer(int layer) => _layerMask.Add(layer);
        public void AddTag(string tag) => _layerMask.Add(tag);

        public void RemoveLayer(string layer) => _layerMask.Remove(layer);
        public void RemoveLayer(int layer) => _layerMask.Remove(layer);
        public void RemoveTag(string tag) => _tagMask.Remove(tag);

        private bool _containsLayer(int layer) => _layerMask.Contains(layer);
        private bool _containsLayer(string layer) => _layerMask.Contains(layer);
        private bool _containsTag(string tag) => _tagMask.Contains(tag);

        private void _resetCache()
        {
            _hasBoth = null;
            _hasEither = null;
            _hasLayer = null;
            _hasTag = null;
        }
        #endregion

        #region Implicit Operators
        public static implicit operator MetroidLayerMask(MetroidMask mask) => mask._layerMask;
        public static implicit operator MetroidTagMask(MetroidMask mask) => mask._tagMask;
        #endregion
    }
}
