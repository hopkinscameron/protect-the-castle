using System.Collections;
using UnityEngine;

namespace ProtectTheCastle.Towers.Balls
{
    public class Impact : MonoBehaviour, IImpact
    {
        public float duration { get; private set; } = 2f;

        private void Awake()
        {
            StartCoroutine("Die");
        }

        private IEnumerator Die()
        {
            yield return new WaitForSeconds(duration);
            Destroy(gameObject);
        }
    }
}
