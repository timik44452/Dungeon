using Game.Service;
using System.IO;

namespace Game.Terrain.Service
{
    public class LoadedWorldObjects
    {
        private const string map_path = @"Map";


        public LoadedWorldObjects()
        {
            CreateDirectionIfNotCreated();
        }

        private void CreateDirectionIfNotCreated()
        {
            if (Directory.Exists(map_path))
            {
                return;
            }

            Directory.CreateDirectory(map_path);
        }
    }
}
