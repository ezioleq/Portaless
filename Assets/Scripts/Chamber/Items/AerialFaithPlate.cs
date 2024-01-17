using System.Collections;
using UnityEngine;

namespace Chamber.Items
{
    public class AerialFaithPlate : MonoBehaviour
    {
        [SerializeField] private Transform plate;

        [Header("Visuals")]
        [SerializeField] private float maxPlateRotation = 60f;
        [SerializeField] private float openAnimationDuration = 0.1f;
        [SerializeField] private float closeAnimationDuration = 0.25f;

        [Header("Settings")]
        [SerializeField] private float force = 20f;
        [SerializeField] private float cooldown = 2f;

        private new Transform transform;
        private bool deployed;

        private void Start()
        {
            transform = gameObject.transform;
        }

        private void LaunchColliderObject(Collider other)
        {
            if (deployed)
                return;

            var direction = (transform.forward + transform.up) * force;

            if (other.CompareTag("Player"))
            {
                other.GetComponent<CharacterMotor>().AddMomentum(direction);
            }
            else if (other.TryGetComponent<Rigidbody>(out var rb))
            {
                rb.AddForce(direction, ForceMode.VelocityChange);
            }
            else
            {
                return;
            }

            deployed = true;

            StartCoroutine(StartPlateRotationAnimation(maxPlateRotation, openAnimationDuration));
            StartCoroutine(RetreatTimeout());
        }

        private IEnumerator RetreatTimeout()
        {
            yield return new WaitForSeconds(cooldown);
            StartCoroutine(StartPlateRotationAnimation(0, closeAnimationDuration));
            deployed = false;
        }

        private IEnumerator StartPlateRotationAnimation(float targetRotation, float duration)
        {
            var startRotation = plate.localRotation;
            var endRotation  = Quaternion.Euler(targetRotation, 0, 0);
            float time = 0;

            while (time < duration)
            {
                plate.localRotation = Quaternion.Slerp(
                    startRotation,
                    endRotation,
                    time / duration
                );

                time += Time.deltaTime;
                yield return null;
            }

            plate.localRotation = endRotation;
        }

        private void OnTriggerEnter(Collider other)
        {
            LaunchColliderObject(other);
        }
    }
}
