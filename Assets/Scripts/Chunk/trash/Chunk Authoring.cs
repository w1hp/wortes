//using Unity.Entities;
//using Unity.Mathematics;

//public class ChunkAuthoring : UnityEngine.MonoBehaviour
//{
//    public float activationDistance = 50f;
//}

//public class ChunkBaker : Baker<ChunkAuthoring>
//{
//    public override void Bake(ChunkAuthoring authoring)
//    {
//        var entity = GetEntity(TransformUsageFlags.Dynamic);
//        AddComponent(entity, new ChunkData
//        {
//            Position = authoring.transform.position,
//            ActivationDistance = authoring.activationDistance,
//            IsActive = false
//        });
//    }
//}
