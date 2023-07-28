using Cinemachine;
using ProtectTheCastle.Shared;
using UnityEngine;
using UnityEngine.UI;

namespace ProtectTheCastle.UI
{
    public class HealthBar : MonoBehaviour, IHealthBar
    {
        [SerializeField]
        private Slider slider;
        private CinemachineVirtualCamera _camera;

        private void Awake()
        {
            _camera = GameObject.FindGameObjectWithTag(Constants.VIRTUAL_CAMERA_TAG).GetComponent<CinemachineVirtualCamera>();
        }

        private void Update()
        {
            transform.rotation = _camera.transform.rotation;
        }

        public void SetHealth(float maxHealth, float currentHealth)
        {
            slider.value = currentHealth / maxHealth;
        }
    }
}
