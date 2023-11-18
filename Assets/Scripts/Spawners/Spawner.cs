using UnityEngine;

public abstract class Spawner : MonoBehaviour
{
	public enum ExecutionType
	{
		Awake = 0,
		Start = 1,
		Enable = 2,
		Disable = 3,
		Destroy = 4,
		Manual = 5
	}

	[InspectorName("Spawn on")] public ExecutionType executeOn;

	protected virtual void Awake() => TrySpawnOn(ExecutionType.Awake);
	protected virtual void Start() => TrySpawnOn(ExecutionType.Start);
	private void OnEnable() => TrySpawnOn(ExecutionType.Enable);
	private void OnDisable() => TrySpawnOn(ExecutionType.Disable);
	private void OnDestroy() => TrySpawnOn(ExecutionType.Destroy);

	private void TrySpawnOn(ExecutionType executionType)
	{
		if (executionType == executeOn)
			Spawn();
	}

	public abstract void Spawn();
}