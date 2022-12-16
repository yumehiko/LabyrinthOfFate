using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;

namespace yumehiko.LOF.View
{
	public class EntityViews : MonoBehaviour
	{
		[SerializeField] private Transform viewParent;
		private List<IActorView> views;

		public IActorView SpawnEntityView(EntitySpawnPoint spawnPoint, ActorView view)
        {
			Debug.Log("InstantiateEntity");
			var instance = Instantiate(view, (Vector2)spawnPoint.Position, Quaternion.identity, viewParent);
			return instance;
        }

		public void Remove(IActorView view)
		{
			views.Remove(view);
			view.DestroySelf();
		}
	}
}