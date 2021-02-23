using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.Scripts.Entity.Hitbox
{
    public class SendHitEventArgs
    {
        public int Damage;

        public SendHitEventArgs(int damage)
        {
            Damage = damage;
        }
    }
}
