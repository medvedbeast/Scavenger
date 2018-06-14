using UnityEngine;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
namespace Space
{
    public class Sector
    {
        public List<SectorObject> map = new List<SectorObject>();
        public int width;
        public int height;

        static System.Random random = new System.Random();

        int cloudCount = 7;
        int starCount = 1;
        int planetCount = 10;
        int spaceStationCount = 5;
        int asteroidCount = 200;
        int junkCount = 15;

        public Sector(int width, int height)
        {
            int i;
            this.width = width;
            this.height = height;

            Fill(SectorObjects.START_POSITION);

            for (i = 0; i < random.Next(1, cloudCount); i++)
            {
                Fill(SectorObjects.CLOUDS);
            }
            random.Next();

            for (i = 0; i < random.Next(1, starCount); i++)
            {
                Fill(SectorObjects.STAR);
            }
            random.Next();

            for (i = 0; i < random.Next(1, planetCount); i++)
            {
                Fill(SectorObjects.PLANET);
            }
            random.Next();

            for (i = 0; i < random.Next(1, spaceStationCount); i++)
            {
                Fill(SectorObjects.SPACE_STATION);
            }
            random.Next();

            for (i = 0; i < random.Next(150, asteroidCount); i++)
            {
                Fill(SectorObjects.ASTEROID);
            }
            random.Next();

            for (i = 0; i < random.Next(1, junkCount); i++)
            {
                Fill(SectorObjects.JUNK);
            }

        }

        void Fill(SectorObjects type)
        {
            float x, z;
            System.Random random = new System.Random();
            do
            {
                x = random.Next(-(width / 2), (width / 2));
                z = random.Next(-(height / 2), (height / 2));
            }
            while (SectorObject.Occupied(map, x, z) == true);
            map.Add(new SectorObject(type, x, z));
        }

        public void GenerateScene()
        {
            System.Random r = new System.Random();

            for (int i = 0; i < map.Count; i++)
            {
                float x = map[i].x;
                float y = map[i].y;
                float z = map[i].z;
                switch (map[i].type)
                {
                    case SectorObjects.START_POSITION:
                        {
                            GameObject ship = GameObject.Instantiate(Resources.Load("Prefabs/Ships/SCA-01")) as GameObject;
                            ship.transform.position = new Vector3(0, 1, 0);
                            ship.name = "SCA-01";
                            ship.transform.parent = GameObject.Find("Ships").transform;
                            break;
                        }
                    case SectorObjects.CLOUDS:
                        {
                            GameObject cloud = GameObject.Instantiate(Resources.Load("Prefabs/Clouds/CLOUD-0" + random.Next(1, 7))) as GameObject;
                            cloud.transform.position = new Vector3(x, -95 - i, z);
                            cloud.transform.localScale = new Vector3(16, 1, 9);
                            cloud.name = "CLOUD";
                            cloud.transform.parent = GameObject.Find("Clouds").transform;
                            break;
                        }
                    case SectorObjects.STAR:
                        {
                            GameObject star = GameObject.Instantiate(Resources.Load("Prefabs/Stars/STAR-01")) as GameObject;
                            star.transform.position = new Vector3(x, -20, z);
                            star.transform.localScale = new Vector3(50, 50, 50);
                            star.name = "STAR_" + i;
                            star.transform.parent = GameObject.Find("Stars").transform;
                            break;
                        }
                    case SectorObjects.PLANET:
                        {
                            GameObject planet = GameObject.Instantiate(Resources.Load("Prefabs/Planets/PLA-01")) as GameObject;
                            planet.transform.position = new Vector3(x, -30, z);
                            int scale = r.Next(15, 45);
                            planet.transform.localScale = new Vector3(scale, scale, scale);
                            planet.name = "PLA_" + i;
                            planet.transform.parent = GameObject.Find("Planets").transform;
                            break;
                        }
                    case SectorObjects.SPACE_STATION:
                        {
                            GameObject station = GameObject.Instantiate(Resources.Load("Prefabs/SpaceStations/STA-01")) as GameObject;
                            station.transform.position = new Vector3(x, -10, z);
                            station.name = "STA_" + i;
                            station.transform.parent = GameObject.Find("SpaceStations").transform;
                            break;
                        }
                    case SectorObjects.JUNK:
                        {
                            GameObject junk = GameObject.Instantiate(Resources.Load("Prefabs/Junk/JUN-01")) as GameObject;
                            junk.transform.position = new Vector3(x, 1, z);
                            junk.transform.localScale = new Vector3(2, 2, 2);
                            junk.name = "JUN_" + i;
                            junk.transform.parent = GameObject.Find("Junk").transform;
                            break;
                        }
                    case SectorObjects.ASTEROID:
                        {
                            GameObject asteroid = GameObject.Instantiate(Resources.Load("Prefabs/Asteroids/AST-01")) as GameObject;
                            asteroid.transform.SetParent(GameObject.Find("Asteroids").transform);
                            asteroid.transform.position = new Vector3(x, -30, z);
                            int scale = r.Next(1, 20);
                            asteroid.transform.localScale = new Vector3(scale, scale, scale);
                            asteroid.name = "AST_" + i;
                            break;
                        }

                }
            }

            GameObject s = GameObject.Find("SCA-01");
            CameraController.Assign(s);
            Game.gameState = GameStates.GAME;
        }

        public bool Contains(Vector3 point)
        {
            if (point.x <= -width / 2 || point.z <= -height / 2 || point.x >= width / 2 || point.z >= height / 2)
            {
                return false;
            }
            return true;
        }
    }
}