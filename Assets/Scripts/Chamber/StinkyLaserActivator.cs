using Portaless.Lasers;
using UnityEngine;

namespace Portaless.Chamber
{
    public class StinkyLaserActivator : MonoBehaviour {
        [SerializeField] private LaserOrigin stinkyCube;
        private Doors doors;

        void Start() {
            doors = GetComponent<Doors>();
        }

        void Update() {
            doors.ChangeState(stinkyCube.Active);
        }
    }
}
