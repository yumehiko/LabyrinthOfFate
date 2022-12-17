using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;

namespace yumehiko.LOF.View
{
	public class EntityViews : MonoBehaviour
	{
		[SerializeField] private Transform viewParent;
		private List<IActorView> views = new List<IActorView>();

		public IActorView SpawnEntityView(ActorSpawnPoint spawnPoint, ActorView view)
        {
			var instance = Instantiate(view, (Vector2)spawnPoint.Position, Quaternion.identity, viewParent);
			views.Add(instance);
			return instance;
        }

		public void Remove(IActorView view)
		{
			views.Remove(view);
			view.DestroySelf();
		}
	}
}