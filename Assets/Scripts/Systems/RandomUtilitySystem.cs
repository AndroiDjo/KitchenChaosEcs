using Unity.Entities;
using Unity.Collections;
using Unity.Jobs.LowLevel.Unsafe;

/// <summary>
/// Utility system which populates and handle disposing for game randoms
/// </summary>
[UpdateInGroup(typeof(InitializationSystemGroup))]
public partial class RandomsUtilitySystem : SystemBase
{
    /// <summary>
    /// Array of randoms
    /// </summary>
    /// <remarks>
    /// Use [NativeDisableParallelForRestriction] attribute for this array in your jobs for write back<br/>
    /// Use [NativeSetThreadIndex] _nativeThreadIndex as indexer for that array
    /// </remarks>
    public static NativeArray<Unity.Mathematics.Random> Randoms;

    protected override void OnCreate()
    {
        Randoms = new NativeArray<Unity.Mathematics.Random>(JobsUtility.MaxJobThreadCount, Allocator.Persistent, NativeArrayOptions.UninitializedMemory);
        for (int i = 0; i < JobsUtility.MaxJobThreadCount; i++) {
            uint r = 0;
            while (r == 0) {
                r = (uint)UnityEngine.Random.Range(int.MinValue, int.MaxValue);
            }
            Randoms[i] = new Unity.Mathematics.Random(r);
        }
        Enabled = false;
    }

    protected override void OnDestroy()
    {
        if (Randoms.IsCreated)
            Randoms.Dispose();
    }

    protected override void OnUpdate() { }
}