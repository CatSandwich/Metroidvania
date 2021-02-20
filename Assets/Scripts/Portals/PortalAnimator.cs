using System;
using System.ComponentModel;
using Portal;
using UnityEngine;

namespace Portals
{
    public class PortalAnimator : MonoBehaviour
    {
        public SpriteRenderer SR;

        public float RotateSpeed;

        public Vector2 TiltMagnitude;
        public Vector2 TiltSpeed;
        
        private Vector2 _tilt;

        public Vector2 ScaleSpeed;
        public Vector2 ScaleMagnitude;
        
        private Vector2 _scale;
        private Vector3 _startScale;
        
        void Start()
        {
            _tilt = new Vector2(0f, 0f);
            _startScale = SR.transform.localScale;
        }

        void FixedUpdate()
        {
            _updateRotation();
            _updateScale();
        }
        
        private void _updateRotation()
        {
            _tilt.x += TiltSpeed.x;
            _tilt.y += TiltSpeed.y;
            if (_tilt.x >= 360f) _tilt.x = 0f;
            if (_tilt.y >= 360f) _tilt.y = 0f;
            
            _rotate(SR.transform, 0, 0, RotateSpeed, RotateType.Add);
            _rotate(SR.transform, _sin(_tilt.x) * TiltMagnitude.x, _sin(_tilt.y) * TiltMagnitude.y, null, RotateType.Set);
        }

        private void _updateScale()
        {
            _scale.x += ScaleSpeed.x;
            _scale.y += ScaleSpeed.y;
            if (_scale.x >= 360f) _scale.x = 0f;
            if (_scale.y >= 360f) _scale.y = 0f;
            
            var x = _startScale.x * _mod(_scale.x, ScaleMagnitude.x);
            var y = _startScale.y * _mod(_scale.y, ScaleMagnitude.y);
            
            SR.transform.localScale = new Vector3(x, y, _startScale.z);
        }

        private void _rotate(Transform t, float? x, float? y, float? z, RotateType type)
        {
            var euler = t.eulerAngles;
            t.eulerAngles = type switch
            {
                RotateType.Add      => new Vector3(euler.x + x ?? 0f, euler.y + y ?? 0f, euler.z + z ?? 0f),
                RotateType.Subtract => new Vector3(euler.x - x ?? 0f, euler.y - y ?? 0f, euler.z - z ?? 0f),
                RotateType.Set      => new Vector3(x ?? euler.x, y ?? euler.y, z ?? euler.z),
                _ => throw new InvalidEnumArgumentException($"_rotate() given invalid type {type}.")
            };
        }

        private static float _sin(float f) => (float) Math.Sin(f / 180 * Math.PI);
        private static float _mod(float degree, float magnitude) => _sin(degree) * magnitude + 1;
        
        enum RotateType
        {
            Add,
            Subtract,
            Set
        }
    }
}
