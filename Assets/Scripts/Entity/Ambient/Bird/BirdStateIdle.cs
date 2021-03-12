using Entity.State_Machine;

namespace Entity.Ambient.Bird
{
    public class BirdStateIdle : MetroidBehaviour
    {
        protected override void Load()
        {
            Context.Rigidbody.velocity *= 0f;
        }
    }
}