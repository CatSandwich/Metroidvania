using Assets.Scripts.Entity.Player;
using Assets.Scripts.Entity.Player.EventArgs;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Entity
{
    public class PlayerInteractible : MonoBehaviour
    {
        private PlayerController Player => _player ??= GameObject.FindWithTag("Player").GetComponent<PlayerController>();
        private PlayerController _player;

        private bool _isInit = false;

        public virtual void PlayerInteract(object sender, EntityInteractEventArgs e)
        {
            Debug.LogError("PlayerInteract not overridden in PlayerInteractible");
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if(!_isInit) _initPlayerInteractible();
            if (!other.CompareTag("Player")) return;
            Player.EntityInteract += PlayerInteract;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (!other.CompareTag("Player")) return;
            Player.EntityInteract -= PlayerInteract;
        }
        
        private void _initPlayerInteractible()
        {
            _isInit = true;
            SceneManager.sceneLoaded += _sceneLoadedPlayerInteractible;
        }

        private void _sceneLoadedPlayerInteractible(Scene s, LoadSceneMode m)
        {
            Player.EntityInteract -= PlayerInteract;
        }
    }
}
