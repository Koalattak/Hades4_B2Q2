using System.Collections.Generic;
using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class ArenaManager : MonoBehaviour
    {
        [SerializeField] private List<EnemyController> _enemiesToKill;
        private int _enemyCounter;
        [SerializeField] private List<Transform> _pathsToBlock;
        [SerializeField] private GameObject _wallToSpawn;
        [SerializeField] private List<GameObject> _spawnedWalls;
        private bool _isBlocking;

        private void OnTriggerEnter(Collider other)
        {
            if (_isBlocking) return;
            if (!other.transform.root.TryGetComponent<PlayerController>(out _)) return;
            _isBlocking = true;
            
            //Spawn Walls
            foreach(Transform wallTransform in _pathsToBlock)
            {
                GameObject spawnedWall = Instantiate(_wallToSpawn, wallTransform.position, Quaternion.identity);
                _spawnedWalls.Add(spawnedWall);
            }
            Debug.Log("Test");
        }

        void Start()
        {
            foreach(EnemyController enemy in _enemiesToKill)
            {
                if (enemy != null)
                {
                    enemy.GetHealth().OnDeath += RemoveEnemyFromList;
                }
            }
            _enemyCounter = _enemiesToKill.Count;
            _isBlocking = false;

        }

        private void RemoveEnemyFromList()
        {
            _enemyCounter--;
            if (_enemyCounter > 0) return;
            DestroyWalls();

        }

        private void DestroyWalls()
        {
            foreach(GameObject wall in _spawnedWalls)
            {
                Destroy(wall);
            }
        }
    }
}
