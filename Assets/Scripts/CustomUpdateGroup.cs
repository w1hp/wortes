using Unity.Entities;
using Unity.Mathematics;
using Unity.Collections;
using Unity.Jobs;
using Unity.Burst;
using Unity.Jobs.LowLevel.Unsafe;


public partial class CustomUpdateGroup : ComponentSystemGroup
{
	public CustomUpdateGroup()
	{
		RateManager = new RateUtils.VariableRateManager(250, true);
	}
}