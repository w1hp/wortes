//using Unity.Entities;
//using Unity.Mathematics;

//public class PlayerAuthoringChunk : UnityEngine.MonoBehaviour
//{
//}

//public class PlayerBaker : Baker<PlayerAuthoring>
//{
//    public override void Bake(PlayerAuthoring authoring)
//    {
//        var entity = GetEntity(TransformUsageFlags.Dynamic);
//        AddComponent(entity, new PlayerData
//        {
//            Position = authoring.transform.position
//        });
//    }
//}
