using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.Scripts.Entity.Player.EventArgs;

namespace Assets.Scripts.Entity.Hitbox
{
    public class ReceiveHitEventArgs : MetroidEventArgs
    {
        public int Damage;

        public ReceiveHitEventArgs(int damage)
        {
            Damage = damage;
        }
    }
}
