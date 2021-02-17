using UnityEngine;

namespace Portal
{
    [CreateAssetMenu(fileName = "PortalColour", menuName = "ScriptableObjects/PortalColour", order = 1)]
    public class PortalScriptable : ScriptableObject
    {
        public Sprite[] Frames;
    }
}
