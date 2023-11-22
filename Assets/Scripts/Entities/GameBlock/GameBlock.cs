using UnityEngine;
using Gruffdev.BCS;
using System;

[AddComponentMenu("GameBlock/GameBlock")]
public partial class GameBlock : MonoBehaviour
	, IEntity
	, IEntityUpdate
	, IEntityFixedUpdate
	, IEntityLateUpdate
{
	public GameBlockConfig config;
	public GameBlockType blockType;
	public RectTransform gameBlockRect;
	public IEntitySystem[] allSystems;
	public IUpdate[] updateSystems;
	public ILateUpdate[] lateUpdateSystems;
	public IFixedUpdate[] fixedUpdateSystems;

	public void Init(GameBlockConfig config)
	{
		this.config = config;
	}

	public void FindSystems()
	{
		allSystems = gameObject.GetComponents<IEntitySystem>();
		updateSystems = gameObject.GetComponents<IUpdate>();
		lateUpdateSystems = gameObject.GetComponents<ILateUpdate>();
		fixedUpdateSystems = gameObject.GetComponents<IFixedUpdate>();
	}

	protected virtual void Awake() => GameBlockEntityManager.I.AddEntity(this);
	protected virtual void OnEnable() => GameBlockEntityManager.I.EnableEntity(this);
	protected virtual void OnDisable() => GameBlockEntityManager.I.DisableEntity(this);
	protected virtual void OnDestroy() => GameBlockEntityManager.I.RemoveEntity(this);

	public void OnUpdate()
	{
		for (int i = 0; i < updateSystems.Length; i++)
			updateSystems[i].OnUpdate();
	}

	public void OnLateUpdate()
	{
		for (int i = 0; i < lateUpdateSystems.Length; i++)
			lateUpdateSystems[i].OnLateUpdate();
	}

	public void OnFixedUpdate()
	{
		for (int i = 0; i < fixedUpdateSystems.Length; i++)
			fixedUpdateSystems[i].OnFixedUpdate();
	}

	internal void DoScale()
	{
		throw new NotImplementedException();
	}
}
