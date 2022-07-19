using CorployGame.entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace CorployGame.util
{
    class NpcWave
    {
        List<AiAgent> NpcList;
        int SpawnCount;
        TimeSpan SpawnInterval;
        TimeSpan WaveTime;
        public bool WaveEnded;

        public NpcWave(List<AiAgent> npcList, TimeSpan spawnInterval)
        {
            NpcList = npcList;
            SpawnCount = 0;
            SpawnInterval = spawnInterval;
            WaveTime = new TimeSpan(0);
            WaveEnded = false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="elapsedTime"></param>
        /// <returns AiAgent> returns AiAgent if a new agent has spawned. </returns>
        public AiAgent SpawnCheck(TimeSpan elapsedTime)
        {
            if (WaveEnded || NpcList.Count < 1) return null; // Return null if last AiAgent has already been spawned, or if there are no NPC's in list.

            WaveTime += elapsedTime;

            double intervalCount = WaveTime / SpawnInterval;
            int checkNumber = (int)intervalCount; // Quick alternative to rounding down.

            if(checkNumber > SpawnCount)
            {
                // Ensures that this step won't be repeated for already spawned agents. 
                if (SpawnCount >= NpcList.Count)
                {
                    WaveEnded = true;
                    return null;
                }

                AiAgent latestAgent = NpcList[SpawnCount];
                latestAgent.ShouldSpawn = true;
                
                SpawnCount++; // Would be counterintiutive to have a SpawnCount higher than the amount of AiAgents in the list.

                return latestAgent; // Only return AiAgent if it should spawn and become active in current cycle.
            }

            return null; // Nothing has been spawned, return null.
        }
    }
}
