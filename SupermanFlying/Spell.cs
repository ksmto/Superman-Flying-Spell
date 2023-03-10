using Extensions;
using ThunderRoad;
using UnityEngine;

namespace SupermanFlying;
public class Spell : SpellCastCharge {
	public float flyingForce = 9.0f;
	public float boostForceMultiplier = 2.0f;
	public override void UpdateCaster() {
		base.UpdateCaster();
		if (!spellCaster.isFiring) return;
		Player.fallDamage = false;
		spellCaster?.ragdollHand?.rb?.AddForce(-spellCaster.ragdollHand.transform.right *
		                                    (spellCaster.ragdollHand.GripPressed()
			                                     ? flyingForce * boostForceMultiplier
			                                     : flyingForce / boostForceMultiplier),
		                                    ForceMode.VelocityChange);
	}
}