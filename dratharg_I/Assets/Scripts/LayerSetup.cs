using UnityEngine;

public class LayerSetup {

	public static void SetLayer (GameObject gameObject, int newLayer)
	{
		if (gameObject == null)
			return;

		gameObject.layer = newLayer;

		foreach (Transform child in gameObject.transform)
		{
			if (child == null)
				continue;

			SetLayer(child.gameObject, newLayer);
		}
	}

}