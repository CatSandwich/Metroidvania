using System.Collections.Generic;
using System.Linq;

namespace Helpers.Mask
{
    internal class MetroidTagMask
    {
        public List<string> Mask;

        #region Constructors
        public MetroidTagMask(IEnumerable<string> mask)
        {
            Mask = new List<string>(mask);
        }
        #endregion

        #region Modifiers
        public bool Contains(string tag) => Mask.Contains(tag);
        public void Add(string tag) => Mask.Add(tag);
        public void Remove(string tag) => Mask.Remove(tag);
        #endregion

        #region Implicit Operators
        public static implicit operator MetroidTagMask(string s) => new MetroidTagMask(new[] {s});
        public static implicit operator MetroidTagMask(string[] s) => new MetroidTagMask(s);
        public static implicit operator MetroidTagMask(List<string> s) => new MetroidTagMask(s);
        #endregion
    }
}
