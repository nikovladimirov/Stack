using Behaviours;
using Helpers;
using UnityEngine;

namespace Data
{
    public class CubeData
    {
        public CubeData()
        {
            
        }
        public CubeData(CubeBehaviour cubeBehaviour)
        {
            Position = cubeBehaviour.Position;
            Scale = cubeBehaviour.Scale;
            Color = ColorConverter.ColorToHex(cubeBehaviour.Color);
        }

        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public string Color { get; set; }
        
    }
}