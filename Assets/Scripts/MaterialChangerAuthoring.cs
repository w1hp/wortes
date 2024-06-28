using Unity.Entities;
using UnityEngine;

public class MaterialChangerAuthoring : MonoBehaviour
{
    public GameObject Character;

	class MaterialChangerBaker : Baker<MaterialChangerAuthoring>
    {
        public override void Bake(MaterialChangerAuthoring authoring)
        {
			var entity = GetEntity(TransformUsageFlags.Dynamic);
           
            AddComponent(entity, new MaterialChanger
            {
				Character = GetEntity(authoring.Character, TransformUsageFlags.Dynamic)
            });
        }
    }
}
public struct MaterialChanger : IComponentData
{
    public Entity Character;
    public bool CanBuild;
}
