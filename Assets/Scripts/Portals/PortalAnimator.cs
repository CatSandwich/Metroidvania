using Portal;
using UnityEngine;

namespace Portals
{
    public class PortalAnimator : MonoBehaviour
    {
        public PortalType Type;
        public SpriteRenderer SR;
        public int Speed;
        
        private PortalScriptable _animation;
        private int _animationSize;
        private int _current;
        private int _delay;
        private readonly string[] _animationPaths = {"BlueLarge", "BlueSmall", "GreenLarge", "GreenSmall", "PinkLarge", "PinkSmall", "YellowLarge", "YellowSmall"};
        // Start is called before the first frame update
        void Awake()
        {
            _animation = Resources.Load<PortalScriptable>($"PortalScriptables/{_animationPaths[(int)Type]}");
            _animationSize = _animation.Frames.Length;
        }

        // Update is called once per frame
        void Update()
        {
            _delay += Speed;
            if (_delay < 60) return;

            _delay = 0;
            
            SR.sprite = _animation.Frames[_current++];
            if (_current == _animationSize) _current = 0;
        }

        public enum PortalType
        {
            BlueLarge,
            BlueSmall,
            GreenLarge,
            GreenSmall,
            PinkLarge,
            PinkSmall,
            YellowLarge,
            YellowSmall
        }
    }
}
