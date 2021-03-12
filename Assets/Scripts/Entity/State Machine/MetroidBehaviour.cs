namespace Entity.State_Machine
{
    public abstract class MetroidBehaviour
    {
        public bool IsLoaded
        {
            get => _isLoaded;
            set
            {
                if (value == true) _tryLoad();
                else _tryUnload();
            }
        }
        private bool _isLoaded;

        protected StateMachineContext Context;

        public void Init(StateMachineContext c)
        {
            Context = c;
        }
        
        public virtual void Update()
        {
            
        }
        
        protected virtual void Load()
        {
            
        }

        protected virtual void Unload()
        {
            
        }

        private void _tryLoad()
        {
            if (_isLoaded) return;
            _isLoaded = true;
            Load();
        }

        private void _tryUnload()
        {
            if (!_isLoaded) return;
            _isLoaded = false;
            Unload();
        }
    }
}