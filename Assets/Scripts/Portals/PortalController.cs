using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalController : MonoBehaviour
{
    public static string CurrentLevel;
    public string DestinationLevel;
    
    private GameObject _player;

    public static void Init()
    {
        SceneManager.sceneLoaded += _sceneLoaded;
        CurrentLevel = SceneManager.GetActiveScene().name;
    }
    
    public void Enter()
    {
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
        var player = FindObjectOfType<PlayerController>().gameObject;

        var v3 = portal.transform.position;

        var portalHeight = portal.GetComponent<SpriteRenderer>().bounds.size.y;
        var playerHeight = player.GetComponent<SpriteRenderer>().bounds.size.y;

        v3.y -= (portalHeight - playerHeight) / 2;
        
        player.transform.position = v3;

        CurrentLevel = scene.name;
    }
}
