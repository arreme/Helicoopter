using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Helicoopter
{
    public class LevelInitializer : MonoBehaviour
    {
        [SerializeField] private Transform[] levelSpawns;
        [SerializeField] private GameObject playerPrefab;
        private void Awake()
        {
            var configs = S_PlayerInputManager.Instance.GetPlayerConfigs();
            
            var result = levelSpawns;
            result.Shuffle();
            for(int i = 0; i < configs.Length; i++)
            {
                var player = Instantiate(playerPrefab,result[i]);
                player.GetComponent<PlayerInitializer>().InitializePlayer(configs[i]);

                if (GameState.Instance != null)
                {
                    GameState.Instance.Players.Add(player);
                }
            }
        }
    }
    
    static class Shuffler
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            System.Random random = new System.Random();
            int n = list.Count;
            while (n > 1)
            {
                n--;
                int k = random.Next(n + 1);
                T a = list[k];
                T b = list[n];
                (a, b) = (b, a);
            }
        }
    }
}
