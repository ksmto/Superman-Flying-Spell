using Extensions;
using ThunderRoad;
using UnityEngine;

namespace SupermanFlying;

public class Spell : SpellCastCharge
{
    [ModOption("Flying Force", "The amount of force that is applied to your hand when casting.", valueSourceName = "ForceValues", defaultValueIndex = 7, saveValue = true)]
    public static float flyingForce;

    [ModOption("Flying Boost Force Multiplier", "The amount of multiplier force that is applied to the flying force when you press the grip.", valueSourceName = "ForceValues", defaultValueIndex = 2, saveValue = true)]
    public static float boostForceMultiplier;

    public override void UpdateCaster()
    {
        base.UpdateCaster();
        if (spellCaster != null)
        {
            if (Player.currentCreature != null)
            {
                Player.fallDamage = false;

                if (spellCaster.isFiring)
                {
                    spellCaster?.ragdollHand?.physicBody?.AddForce(-spellCaster.ragdollHand.transform.right * (spellCaster.ragdollHand.GripPressed() ? flyingForce * boostForceMultiplier : flyingForce / boostForceMultiplier), ForceMode.VelocityChange);
                }
            }
        }
    }

    public static ModOptionFloat[] ForceValues =
    {
        new ModOptionFloat("2.0", 2.0f),
        new ModOptionFloat("3.0", 3.0f),
        new ModOptionFloat("4.0", 4.0f),
        new ModOptionFloat("5.0", 5.0f),
        new ModOptionFloat("6.0", 6.0f),
        new ModOptionFloat("7.0", 7.0f),
        new ModOptionFloat("8.0", 8.0f),
        new ModOptionFloat("9.0", 9.0f),
        new ModOptionFloat("10.0", 10.0f)
    };
}