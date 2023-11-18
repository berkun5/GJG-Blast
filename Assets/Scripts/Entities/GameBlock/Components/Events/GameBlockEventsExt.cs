using UnityEngine;
using Gruffdev.BCS;

public partial class GameBlock : MonoBehaviour, IEntity
{
	public bool hasEvents { private set; get; }
	public GameBlockEventsSystem events { private set; get; }
	public GameBlockEventsConfig eventsConfig { private set; get; }
	
	public GameBlockEventsSystem AddEvents(GameBlockEventsConfig config)
	{
		if (hasEvents)
			Destroy(events);
		
		events = gameObject.AddComponent<GameBlockEventsSystem>();
		eventsConfig = config;
		events.Init(this, config);
		hasEvents = true;
		return events;
	}
	
	public void RemoveEvents()
	{
		if (!hasEvents)
			return;
	
		events.Remove();
		Destroy(events);
	
		hasEvents = false;
		events = null;
		eventsConfig = null;
	}
}
