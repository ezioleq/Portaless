using System.Collections;
using UnityEngine;

namespace Chamber.Items
{
    public class AerialFaithPlate : MonoBehaviour
    {
        [SerializeField] private Transform plate;

        [Header("Visuals")]
        [SerializeField] private float maxPlateRotation = 60f;
        [SerializeField] private float animationSpeed = 20f;

        [Header("Settings")]
        [SerializeField] private float force = 20f;
        [SerializeField] private float cooldown = 2f;

        private new Transform transform;
        private bool deployed;

        private void Start()
        {
            transform = gameObject.transform;
        }

        private void Update()
        {
            // TODO: Avoid animation in update this way.
            plate.localRotation = Quaternion.Slerp(
                plate.localRotation,
                Quaternion.Euler(
                    (deployed) ? new Vector3(maxPlateRotation, 0, 0) : Vector3.zero
                ),
                animationSpeed * Time.deltaTime
            );
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

            StartCoroutine(RetreatTimeout());
        }

        private IEnumerator RetreatTimeout()
        {
            yield return new WaitForSeconds(cooldown);
            deployed = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            LaunchColliderObject(other);
        }
    }
}
