using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using Unity.Rendering;
using UnityEngine.Rendering;

public class MaterialChanger : IComponentData
{
    public Material material0;
    public Material material1;
    public uint frequency;
    public uint frame;
    public uint active;
}

[DisallowMultipleComponent]
public class MaterialChangerAuthoring : MonoBehaviour
{
    public Material material0;
    public Material material1;
    [RegisterBinding(typeof(MaterialChanger), "frequency")]
    public uint frequency;
    [RegisterBinding(typeof(MaterialChanger), "frame")]
    public uint frame;
    [RegisterBinding(typeof(MaterialChanger), "active")]
    public uint active;

    class MaterialChangerBaker : Baker<MaterialChangerAuthoring>
    {
        public override void Bake(MaterialChangerAuthoring authoring)
        {
            MaterialChanger component = new MaterialChanger();
            component.material0 = authoring.material0;
            component.material1 = authoring.material1;
            component.frequency = authoring.frequency;
            component.frame = authoring.frame;
            component.active = authoring.active;
            var entity = GetEntity(TransformUsageFlags.Dynamic);
            AddComponentObject(entity, component);
        }
    }
}
