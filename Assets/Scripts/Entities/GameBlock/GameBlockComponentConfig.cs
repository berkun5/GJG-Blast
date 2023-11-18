using Gruffdev.BCS;

public abstract class GameBlockComponentConfig : ConfigScriptableObject
{
	public abstract void ConstructSystemComponent(GameBlock gameBlock);
}
