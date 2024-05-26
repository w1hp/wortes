using Unity.Entities;
using UnityEngine;

public class MaterialChangerAuthoring : MonoBehaviour
{
	public bool isEnable;
	public bool isRed;

	class MaterialChangerBaker : Baker<MaterialChangerAuthoring>
    {
        public override void Bake(MaterialChangerAuthoring authoring)
        {
			var entity = GetEntity(TransformUsageFlags.Dynamic);
           
            AddComponent(entity, new MaterialChanger
            {
			    isEnable = authoring.isEnable,
                isRed = authoring.isRed
            });
        }
    }
}
public struct MaterialChanger : IComponentData
{
	public bool isEnable;
    public bool isRed;
}
