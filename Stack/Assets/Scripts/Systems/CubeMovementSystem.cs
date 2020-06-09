using DefaultNamespace.Components;
using DefaultNamespace.Enums;
using Unity.Entities;
using Unity.Jobs;
using Unity.Transforms;

namespace DefaultNamespace.Systems
{
    public class CubeMovementSystem : JobComponentSystem
    {
        protected override JobHandle OnUpdate(JobHandle inputDeps)
        {
            float deltaTime = Time.DeltaTime;
            var jobHandle = Entities.ForEach((ref Translation translation, in CubeMove moveCube) =>
            {
                switch (moveCube.Direction)
                {
                    case Direction.X:
                        translation.Value.x += deltaTime;
                        break;
                    case Direction.Z:
                        translation.Value.z += deltaTime;
                        break;
                }
            }).Schedule(inputDeps);
            return jobHandle;
        }
    }
}