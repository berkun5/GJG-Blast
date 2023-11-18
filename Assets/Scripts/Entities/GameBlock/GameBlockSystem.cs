using UnityEngine;
using Gruffdev.BCS;

public class GameBlockSystem<T> : MonoBehaviour, IEntitySystem
	where T : GameBlockComponentConfig
{
	public GameBlock gameBlock { get; private set; }

	[SerializeField] protected T config;

	public T Config { get => config; }

	public virtual void Init(GameBlock gameBlock, T config)
	{
		this.config = config;
		this.gameBlock = gameBlock;
	}

	public virtual void Init() { }
	public virtual void LateSetup() { }
	public virtual void Remove() { }
	public virtual void ReusedSetup() { }
}
