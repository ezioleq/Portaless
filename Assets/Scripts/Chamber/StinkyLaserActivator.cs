using UnityEngine;

public class StinkyLaserActivator : MonoBehaviour {
    [SerializeField] private LaserRedirectionCube stinkyCube;
    private Doors doors; 

    void Start() {
        doors = GetComponent<Doors>();
    }

    void Update() {
        doors.ChangeState(stinkyCube.Active);
    }
}
