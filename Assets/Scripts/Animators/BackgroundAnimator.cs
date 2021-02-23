using System.Linq;
using UnityEngine;

namespace Animators
{
    public class BackgroundAnimator : MonoBehaviour
    {
        public int Repeats;
        public GameObject[] Layers;
    
        private GameObject[,] _leftClones;
        private GameObject[,] _rightClones;

        private float _width;
        private GameObject _player;
        private Vector3[] _startPositions;
        // Start is called before the first frame update
        void Start()
        {
            _player = GameObject.FindWithTag("Player");
            _startPositions = Layers.Select(l => l.transform.position).ToArray();
        
            _width = Layers.FirstOrDefault()?.GetComponent<SpriteRenderer>().bounds.size.x ?? 0f;
            _leftClones = new GameObject[Layers.Length, Repeats];
            _rightClones = new GameObject[Layers.Length, Repeats];
            for (var l = 0; l < Layers.Length; l++)
            {
                for (var r = 0; r < Repeats; r++)
                {
                    _leftClones[l, r] = Instantiate(Layers[l]);
                    _rightClones[l, r] = Instantiate(Layers[l]);
                
                    _leftClones[l, r].transform.SetParent(Layers[l].transform);
                    _rightClones[l, r].transform.SetParent(Layers[l].transform);

                    _leftClones[l, r].transform.localPosition = new Vector3(-_width * (r + 1), 0, 0);
                    _rightClones[l, r].transform.localPosition = new Vector3(_width * (r + 1), 0, 0);
                }
            }
        }
    
        // Update is called once per frame
        void Update()
        {
            for (var i = 0; i < Layers.Length; i++)
            {
                var l = Layers[i];
                var v3 = _startPositions[i];
                v3.x += _player.transform.position.x * i / (Layers.Length - 1);
                v3.y += _player.transform.position.y * i / (Layers.Length - 1);
                l.transform.position = v3;
            }
        }
    }
}
