using ThunderRoad;

namespace SupermanFlying
{
    public class ModOptions
    {
        [ModOptionArrows]
        [ModOption("Force", "The amount of force that is applied to your hand when casting.", valueSourceName = "Values", defaultValueIndex = 7, order = 0, saveValue = true)]
        public static float FlyingForce { get; set; }

        [ModOptionArrows]
        [ModOption("Force Multiplier", "The amount of multiplier force that is multiplied to the flying force when you press the grip.", valueSourceName = "Values", defaultValueIndex = 2, order = 1, saveValue = true)]
        public static float BoostForceMultiplier { get; set; }

        public static ModOptionFloat[] Values =
        {
            new ModOptionFloat("1.025", 1.025f),
            new ModOptionFloat("1.05", 1.05f),
            new ModOptionFloat("1.075", 1.075f),
            new ModOptionFloat("1.1", 1.1f),
            new ModOptionFloat("1.125", 1.125f),
            new ModOptionFloat("1.15", 1.15f),
            new ModOptionFloat("1.175", 1.175f),
            new ModOptionFloat("1.2", 1.2f)
        };
    }
}