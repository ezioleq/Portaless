using UnityEngine;

namespace Portaless.Chamber
{
	public class Dropper : MonoBehaviour {
		[SerializeField] private GameObject objectToDrop;
		[SerializeField] private GameObject droppedObject;
		[SerializeField] private Transform dropPoint;
		private bool alreadyDropped;

		public void DropCube() {
			if (droppedObject != null && alreadyDropped)
				Destroy(droppedObject);

			droppedObject = Instantiate(objectToDrop, dropPoint.position, dropPoint.rotation);
			alreadyDropped = true;
		}
	}
}
