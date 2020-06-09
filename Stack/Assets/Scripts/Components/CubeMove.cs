using DefaultNamespace.Enums;
using Unity.Entities;

namespace DefaultNamespace.Components
{
    [GenerateAuthoringComponent]
    public struct CubeMove : IComponentData
    {
        public Direction Direction;
    }
}