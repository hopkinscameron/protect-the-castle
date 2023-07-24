using ProtectTheCastle.Enums.Castles;
using ProtectTheCastle.Game;
using UnityEngine;

namespace ProtectTheCastle.Castles
{
	public class Castle : MonoBehaviour, ICastle
	{
		public float health { get; private set; }
		public float numCastles { get; private set; }
		public float numPlayers { get; private set; }
        
		[SerializeField]
        private EnumCastleType _type;

		private void Start()
        {
            CastlePrefabTypeSettings settings = GetSettings();
            health = settings.health;
            numCastles = settings.numCastles;
            numPlayers = settings.numPlayers;
        }

		public bool Attacked(float amount)
		{
			health = health - amount;
			return health <= 0;
		}

		public void HandleDeath()
		{
			Destroy(gameObject);
		}

		private CastlePrefabTypeSettings GetSettings()
        {
            switch (_type)
            {
                case EnumCastleType.HeavyArmored:
                    return GameSettingsManager.Instance.castlePrefabSettings.heavyArmored;
                case EnumCastleType.Medic:
                    return GameSettingsManager.Instance.castlePrefabSettings.medic;
                case EnumCastleType.SpeedDemon:
                    return GameSettingsManager.Instance.castlePrefabSettings.speedDemon;
                default:
                    return GameSettingsManager.Instance.castlePrefabSettings.normal;
            }
        }
	}
}
