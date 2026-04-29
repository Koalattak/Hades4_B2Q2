using System.Collections.Generic;
using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class ArenaManager : MonoBehaviour
    {
        [SerializeField] private EnemyController _enemyToSpawn;
        [SerializeField] private PlayerController _playerRef;
        private List<EnemyController> _enemiesToKill = new();
        private int _enemyCounter;
        [SerializeField] private List<Transform> _pathsToBlock;
        [SerializeField] private List<Transform> _enemyTransforms;
        [SerializeField] private GameObject _wallToSpawn;
        [SerializeField] private List<GameObject> _spawnedWalls;
        [SerializeField] private CameraController _camera;
        [SerializeField] private Transform _cameraTransform;
        [SerializeField] private Transform _chechpointTransform;
        private bool _isBlocking = false;
        private bool _isCompleted = false;


        private void OnTriggerEnter(Collider other)
        {
            if (_isBlocking || _isCompleted) return;
            if (!other.transform.root.TryGetComponent<PlayerController>(out _)) return;
            _isBlocking = true;
            _playerRef.OnReset += DestroyWalls;
            //Set Checkpoint
            SpawnManager.SetCheckpoint(_chechpointTransform);

            //Spawn Walls
            foreach (Transform wallTransform in _pathsToBlock)
            {
                GameObject spawnedWall = Instantiate(_wallToSpawn, wallTransform.position, Quaternion.identity);
                _spawnedWalls.Add(spawnedWall);
            }
            _camera.MakeCameraStatic(_cameraTransform);
            SpawnEnemies();
        }

        private void SpawnEnemies()
        {
            _enemyCounter = 0;
            foreach (Transform enemyTransform in _enemyTransforms)
            {
                if (enemyTransform != null)
                {
                    EnemyController spawnedEnemy = Instantiate(_enemyToSpawn, enemyTransform.position, enemyTransform.rotation);
                    spawnedEnemy.InitialiseEnemy(_playerRef);
                    _enemiesToKill.Add(spawnedEnemy);
                    spawnedEnemy.GetHealth().OnDeath += RemoveEnemyFromList;
                    _enemyCounter++;
                }
            }
        }

        private void RemoveEnemyFromList()
        {
            _enemyCounter--;
            if (_enemyCounter > 0) return;
            DestroyWalls();
            _isCompleted = true;
        }

        private void DestroyWalls()
        {
            foreach (GameObject wall in _spawnedWalls)
            {
                Destroy(wall);
            }
            foreach (EnemyController enemy in _enemiesToKill)
            {
                if (enemy != null && enemy.gameObject)
                {
                    enemy.GetHealth().OnDeath -= RemoveEnemyFromList;
                    enemy.EnemyDeath();
                }
            }


            _isBlocking = false;
            _camera.FollowPlayer();
            _playerRef.OnReset -= DestroyWalls;

        }

        //private void Update()
        //{
        //    if (_isBlocking)
        //    {
        //        _camera.MakeCameraStatic(_cameraTransform);
        //    }
        //}
    }
}
