using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ThunderRoad;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Extensions {
    internal static class Methods {
        public static Vector3 Position(this Creature creature) => creature.transform.position;

        public static Creature GetClosestCreature() =>
            Creature.allActive.Where(creature => creature.Alive() && !creature.isPlayer)
                    .OrderBy(creature => Vector3.Distance(Player.currentCreature.Position(), creature.Position()))
                    .FirstOrDefault();

        public static Item GetClosestItem() =>
            Item.allActive.Where(item => item is not null)
                .OrderBy(item => Vector3.Distance(Player.currentCreature.Position(), item.transform.position))
                .FirstOrDefault();

        public static void SliceAllParts(this Creature creature) {
            creature?.Kill();
            creature?.Head()?.TrySlice();
            creature?.Neck()?.TrySlice();
            creature?.LeftArm()?.TrySlice();
            creature?.RightArm()?.TrySlice();
            creature?.LeftLeg()?.TrySlice();
            creature?.RightLeg()?.TrySlice();
        }

        public static RagdollPart GetRagdollPart(this Creature creature, RagdollPart.Type ragdollPartType) => creature?.ragdoll?.GetPart(ragdollPartType);
        public static RagdollPart Head(this Creature creature) => creature?.ragdoll?.headPart;
        public static RagdollPart Neck(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.Neck);
        public static RagdollPart Torso(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.Torso);
        public static RagdollPart LeftHand(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.LeftHand);
        public static RagdollPart RightHand(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.RightHand);
        public static RagdollPart LeftArm(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.LeftArm);
        public static RagdollPart RightArm(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.RightArm);
        public static RagdollPart LeftLeg(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.LeftLeg);
        public static RagdollPart RightLeg(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.RightLeg);
        public static RagdollPart LeftFoot(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.LeftFoot);
        public static RagdollPart RightFoot(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.RightFoot);

        public static float DistanceBetweenCreatureAndPlayer(this Creature creature) =>
            Vector3.Distance(Player.currentCreature.Position(), creature.transform.position);

        public static float DistanceBetweenItems(this Item first, Item other) => (first.transform.position - other.transform.position).sqrMagnitude;

        public static float DistanceBetweenHands() =>
            Vector3.Distance(Player.currentCreature.handLeft.transform.position, Player.currentCreature.handRight.transform.position);

        public static bool EmptyHanded(this RagdollHand hand) =>
            hand?.grabbedHandle is null && hand?.caster?.telekinesis?.catchedHandle is null && !hand.caster.isFiring && !hand.caster.isMerging;

        public static bool GripPressed(this RagdollHand hand) => PlayerControl.GetHand(hand.side).gripPressed;
        public static bool TriggerPressed(this RagdollHand hand) => PlayerControl.GetHand(hand.side).usePressed;
        public static bool AlternateUsePressed(this RagdollHand hand) => PlayerControl.GetHand(hand.side).alternateUsePressed;
        public static bool GripPressed(this SpellCaster spellCaster) => PlayerControl.GetHand(spellCaster.ragdollHand.side).gripPressed;
        public static bool AlternateUsePressed(this SpellCaster spellCaster) => PlayerControl.GetHand(spellCaster.ragdollHand.side).alternateUsePressed;

        public static Vector3 Velocity(this RagdollHand hand) => Player.currentCreature.transform.rotation * hand.playerHand.controlHand.GetHandVelocity();

        public static float VelocityDirection(this RagdollHand hand, Vector3 direction) => Vector3.Dot(hand.Velocity(), direction);

        public static Vector3 BackHandPosition(this RagdollHand hand, float distance = 1.5f) =>
            hand.palmCollider.transform.position + -hand.palmCollider.transform.forward * -distance;

        public static Vector3 FrontHandPosition(this RagdollHand hand, float distance = 1.5f) =>
            hand.palmCollider.transform.position + -hand.palmCollider.transform.forward * distance;

        public static Transform ThumbFingerTip(this RagdollHand hand) => hand.fingerThumb.tip;

        public static Vector3 AboveThumbTip(this RagdollHand hand, float distance = 1.5f) =>
            hand.IndexFingerTip().position + -hand.ThumbFingerTip().transform.forward * -distance;

        public static Transform IndexFingerTip(this RagdollHand hand) => hand.fingerIndex.tip;

        public static Vector3 AboveIndexTip(this RagdollHand hand, float distance = 1.5f) =>
            hand.IndexFingerTip().position + -hand.IndexFingerTip().transform.forward * -distance;

        public static Transform MiddleFingerTip(this RagdollHand hand) => hand.fingerMiddle.tip;

        public static Vector3 AboveMiddleTip(this RagdollHand hand, float distance = 1.5f) =>
            hand.MiddleFingerTip().transform.position + -hand.MiddleFingerTip().transform.forward * -distance;

        public static Transform RingFingerTip(this RagdollHand hand) => hand.fingerRing.tip;

        public static Vector3 AboveRingTip(this RagdollHand hand, float distance = 1.5f) =>
            hand.RingFingerTip().transform.position + -hand.RingFingerTip().transform.forward * -distance;

        public static Transform PinkyFingerTip(this RagdollHand hand) => hand.fingerLittle.tip;

        public static Vector3 AbovePinkyTip(this RagdollHand hand, float distance = 1.5f) =>
            hand.PinkyFingerTip().transform.position + -hand.PinkyFingerTip().transform.forward * -distance;

        public static void LoadBrain(this Creature creature, [CanBeNull] string brainID = null) =>
            creature?.brain?.Load(brainID ?? creature.brain?.instance?.id);

        public static void StopBrain(this Creature creature) => creature?.brain?.Stop();

        public static void EndCast(this SpellCaster spellCaster) {
            spellCaster?.EndCast();
            spellCaster.intensity = 0.0f;
            spellCaster?.Fire(false);
            spellCaster?.spellInstance?.Fire(false);
            spellCaster.isSpraying = false;
            spellCaster.isMerging = false;
            spellCaster?.StopFingersEffect();
        }

        public static void Imbue(this Item item, string spellType, float imbuePower) {
            foreach (var colliderGroup in item.colliderGroups)
                colliderGroup?.imbue?.Transfer(Catalog.GetData<SpellCastCharge>(spellType), imbuePower);
        }

        public static bool Alive(this Creature creature) => !creature?.isKilled ?? false;

        public static void CreateExplosion(Vector3 position,
                                           float explosionRadius = 10.0f,
                                           float explosionForce = 25.0f,
                                           [CanBeNull] EffectData effectData = null,
                                           [CanBeNull] string effectID = "MeteorExplosion") {
            if (effectData is not null) {
                effectData = Catalog.GetData<EffectData>(effectID);
                effectData?.Spawn(position, Quaternion.identity, 1.0f, 1.0f)?.Play();
            }
            var rigidbodyHashSet = new HashSet<Rigidbody>();
            var creatureHashSet = new HashSet<Creature>();
            foreach (var collider in Physics.OverlapSphere(position, explosionRadius)) {
                if (!collider.attachedRigidbody || rigidbodyHashSet.Contains(collider.attachedRigidbody)) return;
                var creature = collider.attachedRigidbody?.GetComponentInParent<Creature>();
                if (creature.Alive() && !creature.isPlayer && !creatureHashSet.Contains(creature)) {
                    creature.ragdoll?.SetState(Ragdoll.State.Destabilized);
                    creatureHashSet.Add(creature);
                }
                if (collider.attachedRigidbody?.GetComponentInParent<Player>() is not null) explosionForce /= 2.0f;
                rigidbodyHashSet.Add(collider.attachedRigidbody);
                collider.attachedRigidbody?.AddExplosionForce(explosionForce, position, explosionRadius, 1.0f, ForceMode.VelocityChange);
            }
        }

        public static bool PrimarilyX(this Vector3 vector3) => vector3.x > vector3.y && vector3.x > vector3.z;
        public static bool PrimarilyY(this Vector3 vector3) => vector3.y > vector3.x && vector3.y > vector3.z;
        public static bool PrimarilyZ(this Vector3 vector3) => vector3.z > vector3.x && vector3.z > vector3.y;

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component =>
            gameObject?.GetComponent<T>() ? gameObject?.AddComponent<T>() : gameObject?.GetComponent<T>();

        public static T GetComponetAndFind<T>(this GameObject gameObject, string find) where T : Component =>
            gameObject?.transform.Find(find)?.GetComponent<T>();

        public static T GetComponetAndFind<T>(this Transform transform, string find) where T : Component =>
            transform?.gameObject.transform.Find(find)?.GetComponent<T>();

        public static T GetComponetAndFind<T>(this Rigidbody rigidbody, string find) where T : Component =>
            rigidbody?.gameObject.transform.Find(find)?.GetComponent<T>();

        public static T GetComponetAndFind<T>(this Item item, string find) where T : Component =>
            item?.gameObject.transform.Find(find)?.GetComponent<T>();

        public static void InertKill(this Creature creature) {
            creature?.Kill();
            creature?.ragdoll?.SetState(Ragdoll.State.Inert);
        }

        public static Vector3 ChangeScale(this Item item, float newScaleSize) =>
            item.transform.localScale = new Vector3(newScaleSize, newScaleSize, newScaleSize);

        public static void Unpenetrate(this Item item) {
            foreach (var damagers in item.collisionHandlers
                                         .SelectMany(collisionHandlers => collisionHandlers.damagers)) damagers.UnPenetrateAll();
        }

        public static void AddForce(this Creature creature,
                                    Vector3 force,
                                    float? forceAdded = null,
                                    ForceMode forceMode = ForceMode.Impulse) {
            foreach (var parts in creature?.ragdoll?.parts) parts?.physicBody?.AddForce(force * forceAdded ?? Vector3.zero, forceMode);
        }

        public static void Destroy(this GameObject gameObject) => Destroy(gameObject);
        public static Color Darken(this Color color, float darkenSpeed) => Color.Lerp(color, Color.clear, darkenSpeed);

        public static void AddForceTowards(this Rigidbody rigidbody,
                                           Vector3 target,
                                           float? forceAdded = null,
                                           ForceMode forceMode = ForceMode.VelocityChange) {
            rigidbody?.AddForce((rigidbody.position - target).normalized * forceAdded ?? Vector3.zero, forceMode);
        }

        public static float Sqrt(this float number) => Mathf.Sqrt(number);
        public static float Abs(this float number) => Mathf.Abs(number);
        public static float Clamp(this float number, float minimum, float maximum) => Mathf.Clamp(number, minimum, maximum);
        public static float Lerp(this float minimum, float maximum, float speed) => Mathf.Lerp(minimum, maximum, speed);
        public static float RandomNumber(float minimum = Mathf.NegativeInfinity, float maximum = Mathf.Infinity) => Random.Range(minimum, maximum);

        public static void Clone(this Creature creature, Vector3 position, Quaternion rotation, [CanBeNull] string brainID = null) {
            creature.data.SpawnAsync(position,
                                     rotation.y,
                                     null,
                                     false, 
                                     null, 
                                     newCreature => { newCreature.LoadBrain(brainID); });
        }

        public static void Clone(this Item item,
                                 Vector3 position,
                                 Quaternion rotation,
                                 Vector3? forceDirection = null,
                                 float? forceAdded = null,
                                 ForceMode forceMode = ForceMode.Impulse) {
            item.data.SpawnAsync(newItem => {
                newItem.transform.position = position;
                newItem.transform.rotation = rotation;
                newItem.physicBody.AddForce(forceDirection ?? Vector3.up * forceAdded ?? Vector3.zero, forceMode);
            });
        }

        public delegate void LineCallback(LineRenderer lineRenderer);

        public static LineRenderer CreateLine(this GameObject gameObj, LineCallback callback = null) {
            var lineRenderer = gameObj?.AddComponent<LineRenderer>();
            callback?.Invoke(lineRenderer);
            return lineRenderer;
        }

        public static bool Facing(this Vector3 source, Vector3 target, float angle = 45.0f) {
            var dot = Vector3.Dot(source.normalized, target.normalized);
            var cosAngle = Mathf.Cos(Mathf.Deg2Rad * angle);
            return dot >= cosAngle;
        }

        public static Direction GetDirection(this Vector3 vector3, float angle = 45.0f) {
            if (vector3.Facing(Vector3.up, angle)) return Direction.Upward;
            if (vector3.Facing(Vector3.forward, angle)) return Direction.Forward;
            if (vector3.Facing(Vector3.left, angle)) return Direction.Leftward;
            if (vector3.Facing(Vector3.right, angle)) return Direction.Rightward;
            if (vector3.Facing(Vector3.back, angle)) return Direction.Backward;
            return vector3.Facing(Vector3.down, angle)
                       ? Direction.Downward
                       : Direction.Any;
        }

        public static RagdollHand Offhand(this Item item) => item.mainHandler.otherHand;
        public static RagdollHand Offhand(this SpellCaster spellCaster) => spellCaster.ragdollHand.otherHand;

        public static IEnumerator Decrease(this float value, float decrement, float delay = 1.0f) {
            while (value > 0) {
                value -= decrement;
                yield return new WaitForSeconds(delay);
            }
        }

        public static float VelocityDirection(this Item item, Vector3 direction) => Vector3.Dot(item.physicBody.velocity, direction);

        public static Vector3 AboveFace(this Creature creature, float distance = 1.50f) =>
            creature.Head().transform.position + creature.Head().forwardDirection * distance;

        public static void ElectrocutionRadius(Vector3 position,
                                               float electrocutionPower = 25.0f,
                                               float electrocutionDuration = 10.0f,
                                               float electrocutionRadius = 10.0f) {
            var rigidbodyHashSet = new HashSet<Rigidbody>();
            var creatureHashSet = new HashSet<Creature>();
            foreach (var collider in Physics.OverlapSphere(position, electrocutionRadius)) {
                if (!collider.attachedRigidbody || rigidbodyHashSet.Contains(collider.attachedRigidbody)) return;
                var creature = collider.attachedRigidbody?.GetComponentInParent<Creature>();
                if (creature.Alive() && !creature.isPlayer && !creatureHashSet.Contains(creature)) {
                    creature.ragdoll?.SetState(Ragdoll.State.Destabilized);
                    creatureHashSet.Add(creature);
                }
                rigidbodyHashSet.Add(collider.attachedRigidbody);
                if (!creature.isPlayer)
                    collider.attachedRigidbody?.GetComponentInParent<Creature>()
                            ?.TryElectrocute(electrocutionPower,
                                             electrocutionDuration,
                                             true,
                                             false,
                                             Catalog.GetData<EffectData>("ImbueLightningRagdoll"));
            }
        }

        public static Axis GetAxis(this Vector3 vector3) {
            if (vector3.PrimarilyX()) return Axis.X;
            return vector3.PrimarilyY() ? Axis.Y : Axis.Z;
        }

        public static void ForBothHands(Action<RagdollHand> action, Creature creature) {
            action(creature.handLeft);
            action(creature.handRight);
        }

        public static bool InWater(this Item item) => item.waterHandler.inWater;

        public static bool InWater(this Creature creature) => creature.waterHandler.inWater;
        
        public static Vector3 AboveHead(this Creature creature, float distance = 1.50f) =>
            creature.Head().transform.position + creature.Head().upDirection * distance;
    }
}

public class Continuum {
    private Continuum continuum;
    private Func<bool> condition;
    private Action action;
    private Type type = Type.Start;

    private enum Type {
        Start,
        WaitFor,
        Do,
        End
    }

    public static Continuum Start() => new Continuum();

    public Continuum WaitFor(Func<bool> condition) {
        continuum = new Continuum { condition = condition, type = Type.WaitFor };
        return continuum;
    }

    public Continuum Do(Action action) {
        continuum = new Continuum { action = action, type = Type.Do };
        return continuum;
    }

    public void Update() {
        switch (type) {
            case Type.Start:
                if (continuum is null) {
                    type = Type.End;
                    return;
                }
                type = continuum.type;
                action = continuum.action;
                condition = continuum.condition;
                continuum = continuum.continuum;
                Update();
                break;
            case Type.WaitFor:
                if (condition.Invoke()) {
                    if (continuum is null) {
                        type = Type.End;
                        return;
                    }
                    type = continuum.type;
                    action = continuum.action;
                    condition = continuum.condition;
                    continuum = continuum.continuum;
                    Update();
                }
                break;
            case Type.Do:
                action.Invoke();
                if (continuum is null) {
                    type = Type.Start;
                    return;
                }
                type = continuum.type;
                action = continuum.action;
                condition = continuum.condition;
                continuum = continuum.continuum;
                break;
            case Type.End: return;
        }
    }
}

public enum Direction {
    Any,
    Upward,
    Forward,
    Leftward,
    Rightward,
    Backward,
    Downward
}

public enum Axis {
    X, Y, Z
}