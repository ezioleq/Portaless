using UnityEngine;

public enum CubeTypes {
    Normal,
    Reflective
}

public class CubeType : MonoBehaviour {
    private Rigidbody _rb;
    public CubeTypes type;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
        _rb.interpolation = RigidbodyInterpolation.Interpolate;
    }
}
