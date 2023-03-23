using Extensions;
using ThunderRoad;
using UnityEngine;

namespace SupermanFlying;

public class Spell : SpellCastCharge {
    [ModOption("Flying Force",
                  "The amount of force that is applied to your hand when casting.",
                  valueSourceName = "Flying Force Value",
                  defaultValueIndex = 0)]
    public static float flyingForce = 9.0f;
    [ModOption("Flying Boost Force Multiplier",
                  "The amount of multiplier force that is applied to the flying force when you press the grip.",
                  valueSourceName = "Flying Boost Force Multiplier Value",
                  defaultValueIndex = 0)]
    public static float boostForceMultiplier = 2.0f;

    public override void UpdateCaster() {
        base.UpdateCaster();
        if (!spellCaster.isFiring) return;
        Player.fallDamage = false;
        spellCaster.ragdollHand.physicBody?.AddForce(-spellCaster.ragdollHand.transform.right *
                                                     (spellCaster.ragdollHand.GripPressed()
                                                          ? flyingForce * boostForceMultiplier
                                                          : flyingForce / boostForceMultiplier), 
                                                     ForceMode.VelocityChange);
    }
}