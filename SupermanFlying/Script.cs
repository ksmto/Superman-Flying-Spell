using System.Linq;
using ThunderRoad;

namespace SupermanFlying
{
    public class Script : ThunderScript
    {
        public override void ScriptEnable()
        {
            base.ScriptEnable();
            EventManager.onPossess += EventManager_onPossess;
        }

        private void EventManager_onPossess(Creature creature, EventTime eventTime)
        {
            if (eventTime == EventTime.OnEnd && Player.currentCreature.container.contents.All(contents => contents.itemData.id != "SpellSupermanFlying") && creature.isPlayer && creature != null)
            {
                creature.container?.AddContent("SpellSupermanFlying");
            }
        }

        public override void ScriptDisable()
        {
            base.ScriptDisable();
            EventManager.onPossess -= EventManager_onPossess;
        }
    }
}