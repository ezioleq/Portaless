using UnityEngine;

public class Dropper : MonoBehaviour {
	public GameObject objectToDrop;
	public GameObject droppedObject;
	public Transform dropPoint;
	private bool _alreadyDropped;
	
	void Start() {
		
	}

	void Update() {

	}

	public void DropCube() {
		if (droppedObject != null && _alreadyDropped)
			Destroy(droppedObject);

		droppedObject = Instantiate(objectToDrop, dropPoint.position, dropPoint.rotation);
		_alreadyDropped = true;
	}
}
