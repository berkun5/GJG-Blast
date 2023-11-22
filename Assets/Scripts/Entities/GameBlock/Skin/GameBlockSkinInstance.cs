using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameBlockSkinInstance : MonoBehaviour
{
	public GameBlockGraphicsSystem graphics { get; private set; }
	public void Init(GameBlockGraphicsSystem graphicsSystem)
	{
		graphics = graphicsSystem;
	}
}
