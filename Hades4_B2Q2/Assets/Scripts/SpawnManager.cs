using UnityEngine;

namespace MaquestiauxMark.Hades
{
    public class SpawnManager : MonoBehaviour
    {
        public static SpawnManager s_spawnManagerInstance;
        private void Awake()
        {
            if (s_spawnManagerInstance == null)
            {
                s_spawnManagerInstance = this;
            }
        }

        static Transform s_lastCheckpointReached;

        public static Transform GetCheckpoint() { return s_lastCheckpointReached; }

        public static void SetCheckpoint(Transform checkpointReached)
        {
            s_lastCheckpointReached = checkpointReached;
        }
    }
}
