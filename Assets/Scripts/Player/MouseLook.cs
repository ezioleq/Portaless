using Portaless.Input;
using UnityEngine;

namespace Portaless.Player
{
    public class MouseLook : MonoBehaviour
    {
        [Header("Config")]
        [Range(0.1f, 2)] public float mouseSensitivity = 0.4f;
        [Range(0, 90)] [SerializeField] private float maxYRotation = 90;
        [Range(-90, 0)] [SerializeField] private float minYRotation = -90;

        [Header("Zoom settings")]
        [SerializeField] private float zoomingFOV = 30f;
        [SerializeField] private float zoomSpeed = 10f;

        [Header("Locks")]
        public bool lockMouse;
        public bool showMouse;

        private new Camera camera;
        private Health health;
        private float defaultFOV;
        private float mouseYRotation;

        private void Start()
        {
            camera = Camera.main;
            defaultFOV = camera?.fieldOfView ?? 60;
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (!lockMouse && health.IsAlive())
                Mouse();

            camera.fieldOfView = Mathf.Lerp(
                camera.fieldOfView, InputManager.Instance.Actions.Gameplay.Zoom.IsPressed() ? zoomingFOV : defaultFOV,
                zoomSpeed * Time.deltaTime
            );

            ToggleCursorVisibility(showMouse);
        }

        private void Mouse()
        {
            var input = InputManager.Instance.Actions.Gameplay.Look.ReadValue<Vector2>();
            float mouseX = input.x * mouseSensitivity;

            mouseYRotation += input.y * mouseSensitivity;
            mouseYRotation = Mathf.Clamp(mouseYRotation, minYRotation, maxYRotation);

            camera.transform.localEulerAngles = new Vector3(-mouseYRotation, 0, 0);
            transform.Rotate(0, mouseX, 0);
        }

        private void ToggleCursorVisibility(bool visible)
        {
            Cursor.visible = visible;
            Cursor.lockState = visible ? CursorLockMode.None : CursorLockMode.Locked;
            lockMouse = visible;
        }
    }
}
