using System.Runtime.InteropServices;
using JetBrains.Annotations;
using UnityEngine;

namespace Helpers.Mask
{
    internal class MetroidMask
    {
        #region Static MetroidMask Properties
        public static MetroidMask TerrainMask =>
            new MetroidMask(null, "Terrain");
        public static MetroidMask PlayerMask => 
            new MetroidMask("Player", "Player");
        #endregion

        #region Cached Comparisons
        public bool HasBoth => _hasBoth ??= HasTag && HasLayer;
        private bool? _hasBoth;
        public bool HasEither => _hasEither ??= HasTag || HasLayer;
        private bool? _hasEither;
        public bool HasLayer => _hasLayer ??= _containsLayer(_go.tag);
        private bool? _hasLayer;
        public bool HasTag => _hasTag ??= _containsTag(_go.tag);
        private bool? _hasTag;
        #endregion

        #region Fields
        public MetroidLayerMask LayerMask;
        public MetroidTagMask TagMask;

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
        public MetroidMask([CanBeNull] MetroidLayerMask layerMask, [CanBeNull] MetroidTagMask tagMask, GameObject go = null)
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

        private bool _containsLayer(int layer) => LayerMask.Contains(layer);
        private bool _containsLayer(string layer) => LayerMask.Contains(layer);
        private bool _containsTag(string tag) => TagMask.Contains(tag);

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
