using Entity.Hitbox;


namespace Entity.Enemy.Spider 
{ 
    public class SpiderReceiver : HitboxReceiver 
    {
        public SpiderController Controller;
        public override void TakeDamage(int damage) => Controller.TakeDamage(damage);

    } 

}

