using ThunderRoad;
using UnityEngine;

namespace SupermanFlying;
public class Spell : SpellCastCharge {
	public float flyingForce = 7.5f;
	public override void UpdateCaster() {
		base.UpdateCaster();
		if (!spellCaster.isFiring) return;
		Player.fallDamage = false;
		switch (spellCaster.side) {
			case Side.Left:
				Player.currentCreature.handLeft.rb.AddForce(-Player.currentCreature.handLeft.transform.right *
				                                            flyingForce,
				                                            ForceMode.VelocityChange);
				break;
			case Side.Right:
				Player.currentCreature.handRight.rb.AddForce(-Player.currentCreature.handRight.transform.right *
				                                             flyingForce,
				                                             ForceMode.VelocityChange);
				break;
		}
	}
}