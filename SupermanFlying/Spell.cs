using Extensions;
using ThunderRoad;
using UnityEngine;

namespace SupermanFlying
{
    public class Spell : SpellCastCharge
    {
        public override void UpdateCaster()
        {
            base.UpdateCaster();
            if (spellCaster != null)
            {
                spellCaster.EndOnGrip(false);

                if (Player.currentCreature != null)
                {
                    Player.fallDamage = false;

                    if (spellCaster.isFiring)
                    {
                        Player.local?.AddForce(-spellCaster.ragdollHand.transform.right * (spellCaster.ragdollHand.GripPressed() ? ModOptions.FlyingForce * ModOptions.BoostForceMultiplier : ModOptions.FlyingForce / ModOptions.BoostForceMultiplier), ForceMode.VelocityChange);
                    }
                }
            }
        }
    }
}