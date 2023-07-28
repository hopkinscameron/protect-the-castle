using System.Collections;
using UnityEngine;

namespace ProtectTheCastle.Towers
{
    public class Explosion : MonoBehaviour, IExplosion
    {
        private float duration = 2f;

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
