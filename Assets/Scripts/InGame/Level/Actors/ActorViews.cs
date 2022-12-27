using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using yumehiko.LOF.Model;

namespace yumehiko.LOF.View
{
	public class ActorViews : MonoBehaviour
	{
		[SerializeField] private Transform viewParent;
		private readonly List<IActorView> views = new List<IActorView>();

		public IActorView SpawnActorView(Vector2Int position, ActorView view)
        {
			var instance = Instantiate(view, (Vector2)position, Quaternion.identity, viewParent);
			views.Add(instance);
			return instance;
        }

		public void AddView(IActorView view)
        {
			views.Add(view);
		}

		public void Remove(IActorView view)
		{
			views.Remove(view);
			view.DestroySelf();
		}
	}
}