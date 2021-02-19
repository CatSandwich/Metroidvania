using System;
using Portal;
using UnityEngine;

namespace Portals
{
    public class PortalAnimator : MonoBehaviour
    {
        public SpriteRenderer SR;

        void Start()
        {
            _tilt = new Vector2(0, 0);
            _startScale = SR.transform.localScale;
        }
        
        // Update is called once per frame
        void FixedUpdate()
        {
            _updateRotation();
            _updateScale();
        }

        public float RotateSpeed;
        public Vector2 TiltMagnitude;
        public Vector2 TiltSpeed;
        private Vector2 _tilt;
        private void _updateRotation()
        {
            if ((_tilt.x += TiltSpeed.x) == 360) _tilt.x = 0;
            if ((_tilt.y += TiltSpeed.y) == 360) _tilt.y = 0;
            
            _rotate(SR, 0, 0, RotateSpeed, RotateType.Add);
            _rotate(SR, _sin(_tilt.x) * TiltMagnitude.x, _sin(_tilt.y) * TiltMagnitude.y, null, RotateType.Set);
        }

        public Vector2 ScaleSpeed;
        public Vector2 ScaleMagnitude;
        private Vector2 _scale;
        private Vector3 _startScale;
        private void _updateScale()
        {
            if ((_scale.x += ScaleSpeed.x) == 360) _scale.x = 0;
            if ((_scale.y += ScaleSpeed.y) == 360) _scale.y = 0;
            
            var x = _startScale.x * _mod(_scale.x, ScaleMagnitude.x);
            var y = _startScale.y * _mod(_scale.y, ScaleMagnitude.y);
            
            SR.transform.localScale = new Vector3(x, y, _startScale.z);
        }

        private void _rotate(SpriteRenderer sr, float? x, float? y, float? z, RotateType type)
        {
            var euler = sr.transform.eulerAngles;
            euler = type switch
            {
                RotateType.Add      => new Vector3(euler.x + x ?? 0, euler.y + y ?? 0, euler.z + z ?? 0),
                RotateType.Subtract => new Vector3(euler.x - x ?? 0, euler.y - y ?? 0, euler.z - z ?? 0),
                RotateType.Set      => new Vector3(x ?? euler.x, y ?? euler.y, z ?? euler.z)
            };
            sr.transform.eulerAngles = euler;
        }

        private float _sin(float f) => (float) Math.Sin(f / 180 * Math.PI);
        private float _mod(float degree, float magnitude) => _sin(degree) * magnitude + 1;
        
        enum RotateType
        {
            Add,
            Subtract,
            Set
        }
    }
}
