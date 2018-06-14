using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;
using System.Reflection;
using Common;

namespace Space
{
    public class Game : MonoBehaviour
    {

        public static GameStates gameState = GameStates.MAIN_MENU;
        public static Sector sector;

        void Start()
        {
            UI.Show();
            UI.Hide();
            UI.Show("StartMenu");
        }

    }
}