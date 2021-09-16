using UnityEngine;

public class StinkyLaserActivator : MonoBehaviour {
    public LaserRedirectionCube stinkyCube;
    private Doors _doors; 

    void Start() {
        _doors = GetComponent<Doors>();
    }

    void Update() {
        _doors.ChangeState(stinkyCube.active);
    }
}
