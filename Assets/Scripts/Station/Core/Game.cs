using UnityEngine;
using System.Collections;

namespace Station
{
    public class Game : MonoBehaviour
    {

        public static GameStates gameState;

        void Start()
        {
            gameState = GameStates.GAME;
        }

        void Update()
        {

        }
    }
}