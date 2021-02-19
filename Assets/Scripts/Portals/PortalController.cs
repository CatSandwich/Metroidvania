using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public static string CurrentLevel;
    public string DestinationLevel;
    
    private static GameObject _player;

    private static bool _isInit;
    public static void Init()
    {
        _isInit = true;
        SceneManager.sceneLoaded += _sceneLoaded;
        CurrentLevel = SceneManager.GetActiveScene().name;
        _player = GameObject.FindWithTag("Player");
    }

    public void Start()
    {
        if (!_isInit) Init();
    }
    
    public void Enter()
    {
        _player.GetComponent<PlayerController>().EntityInteract -= Enter;
        SceneManager.LoadScene(DestinationLevel);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        collision.gameObject.GetComponent<PlayerController>().EntityInteract += Enter;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.gameObject.CompareTag("Player")) return;
        collision.gameObject.GetComponent<PlayerController>().EntityInteract -= Enter;
    }

    private static void _sceneLoaded(Scene scene, LoadSceneMode sceneLoadInfo)
    {
        var portals = FindObjectsOfType<PortalController>();
        var portal = portals.First(p => p.DestinationLevel == CurrentLevel);

        var v3 = portal.transform.position;

        var portalHeight = portal.GetComponent<SpriteRenderer>().bounds.size.y;
        var playerHeight = _player.GetComponent<SpriteRenderer>().bounds.size.y;

        v3.y -= (portalHeight - playerHeight) / 2;
        v3.z = _player.transform.position.z;
        
        _player.transform.position = v3;

        CurrentLevel = scene.name;
    }
}
