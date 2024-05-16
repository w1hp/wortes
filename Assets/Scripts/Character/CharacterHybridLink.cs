using System;
using Unity.Collections;
using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

[Serializable]
public class CharacterHybridData : IComponentData
{
    public GameObject MeshPrefab;
}

[Serializable]
public class CharacterHybridLink : ICleanupComponentData
{
    public GameObject Object;
    public Animator Animator;
}