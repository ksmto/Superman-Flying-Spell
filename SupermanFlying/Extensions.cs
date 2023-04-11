using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using ThunderRoad;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

namespace Extensions
{
    internal static class Methods
    {
        public static Creature GetClosestCreature()
            => Creature.allActive.Where(creature => creature != null && !creature.isKilled && !creature.isPlayer)
                       .OrderBy(creature => Vector3.Distance(Player.currentCreature.transform.position, creature.transform.position))
                       .FirstOrDefault();

        public static Item GetClosestItem()
            => Item.allActive.Where(item => item != null)
                   .OrderBy(item => Vector3.Distance(Player.currentCreature.transform.position, item.transform.position))
                   .FirstOrDefault();

        public static void SliceAllParts(this Creature creature)
        {
            if (creature != null)
            {
                creature.Kill();
                creature.GetAllSliceableParts().TrySlice();
            }
        }

        public static RagdollPart GetRagdollPart(this Creature creature, RagdollPart.Type ragdollPartType) => creature?.ragdoll?.GetPart(ragdollPartType);

        public static RagdollPart Head(this Creature creature) => creature?.GetRagdollPart(RagdollPart.Type.Head);

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

        public static float DistanceBetweenCreatureAndPlayer(this Creature creature) => Vector3.Distance(Player.currentCreature.transform.position, creature.transform.position);

        public static float DistanceBetweenItems(this Item first, Item other) => Vector3.Distance(first.transform.position, other.transform.position);

        public static float DistanceBetweenHands() => Vector3.Distance(Player.currentCreature.handLeft.transform.position, Player.currentCreature.handRight.transform.position);

        public static bool EmptyHanded(this RagdollHand hand) => hand?.grabbedHandle is null && hand?.caster?.telekinesis?.catchedHandle is null && !hand.caster.isFiring && !hand.caster.isMerging;

        public static bool GripPressed(this RagdollHand hand) => PlayerControl.GetHand(hand.side).gripPressed;

        public static bool TriggerPressed(this RagdollHand hand) => PlayerControl.GetHand(hand.side).usePressed;

        public static bool AlternateUsePressed(this RagdollHand hand) => PlayerControl.GetHand(hand.side).alternateUsePressed;

        public static Vector3 Velocity(this RagdollHand hand) => Player.currentCreature.transform.rotation * hand.playerHand.controlHand.GetHandVelocity();

        public static float VelocityDirection(this RagdollHand hand, Vector3 direction) => Vector3.Dot(hand.Velocity(), direction);

        public static Vector3 BackHandPosition(this RagdollHand hand, float distance = 1.5f) => hand.palmCollider.transform.position + (-hand.palmCollider.transform.forward * Mathf.Abs(distance));

        public static Vector3 FrontHandPosition(this RagdollHand hand, float distance = 1.5f) => hand.palmCollider.transform.position + (hand.palmCollider.transform.forward * Mathf.Abs(distance));

        public static Transform ThumbFingerTip(this RagdollHand hand) => hand.fingerThumb.tip;

        public static Vector3 AboveThumbTip(this RagdollHand hand, float distance = 1.5f) => hand.IndexFingerTip().position + (hand.ThumbFingerTip().transform.forward * Mathf.Abs(distance));

        public static Transform IndexFingerTip(this RagdollHand hand) => hand.fingerIndex.tip;

        public static Vector3 AboveIndexTip(this RagdollHand hand, float distance = 1.5f) => hand.IndexFingerTip().position + (hand.IndexFingerTip().transform.forward * Mathf.Abs(distance));

        public static Transform MiddleFingerTip(this RagdollHand hand) => hand.fingerMiddle.tip;

        public static Vector3 AboveMiddleTip(this RagdollHand hand, float distance = 1.5f) => hand.MiddleFingerTip().transform.position + (hand.MiddleFingerTip().transform.forward * Mathf.Abs(distance));

        public static Transform RingFingerTip(this RagdollHand hand) => hand.fingerRing.tip;

        public static Vector3 AboveRingTip(this RagdollHand hand, float distance = 1.5f) => hand.RingFingerTip().transform.position + (hand.RingFingerTip().transform.forward * Mathf.Abs(distance));

        public static Transform PinkyFingerTip(this RagdollHand hand) => hand.fingerLittle.tip;

        public static Vector3 AbovePinkyTip(this RagdollHand hand, float distance = 1.5f) => hand.PinkyFingerTip().transform.position + (hand.PinkyFingerTip().transform.forward * Mathf.Abs(distance));

        public static void LoadBrain(this Creature creature, string brainID = null) => creature?.brain?.Load(brainID ?? creature.brain?.instance?.id);

        public static void StopBrain(this Creature creature) => creature?.brain?.Stop();

        public static void EndCast(this SpellCaster spellCaster)
        {
            spellCaster.isFiring = false;
            spellCaster.intensity = 0.0f;
            spellCaster.Fire(false);
        }

        public static void Imbue(this Item item, SpellCastCharge spellType)
        {
            foreach (var colliderGroups in item.colliderGroups.Where(colliderGroups => colliderGroups != null && colliderGroups.imbue != null)) colliderGroups.imbue.Transfer(spellType, colliderGroups.imbue.maxEnergy);
        }

        public static void CreateExplosion(Vector3 position,
                                           float explosionRadius = 10.0f,
                                           float explosionForce = 5.0f,
                                           float itemExplosionForce = 7.5f,
                                           float explosionDamage = 20.0f,
                                           EffectData explosionEffectData = null)
        {
            if (explosionEffectData != null)
            {
                var explosionEffectInstance = explosionEffectData.Spawn(position, Quaternion.identity, 1.0f, 1.0f);

                if (explosionEffectInstance != null)
                {
                    explosionEffectInstance.Play();
                }
            }

            foreach (var collider in Physics.OverlapSphere(position, explosionRadius))
            {
                if (collider.GetComponentInParent<Creature>() is Creature creature)
                {
                    if (creature.isPlayer)
                    {
                        creature.player.locomotion.GetPhysicBody().rigidBody.AddExplosionForce(explosionForce / 2.0f, position, explosionRadius, 1.0f, ForceMode.VelocityChange);
                    }
                    else
                    {
                        creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                        creature.GetAllParts().physicBody.rigidBody.AddExplosionForce(explosionForce, position, explosionRadius, 1.0f, ForceMode.VelocityChange);
                        creature.Damage(new CollisionInstance(new DamageStruct(DamageType.Energy, explosionDamage)));
                        creature.handLeft?.UnGrab(false);
                        creature.handRight?.UnGrab(false);
                    }
                }

                if (collider.GetComponentInParent<Item>() is Item item && item != null)
                {
                    item.physicBody.rigidBody.AddExplosionForce(itemExplosionForce, position, explosionRadius, 1.0f, ForceMode.VelocityChange);

                    if (item.GetComponent<Breakable>() is Breakable breakable && breakable != null)
                    {
                        breakable.Break();

                        foreach (var brokenItems in breakable.subBrokenItems.Where(brokenItems => brokenItems != null))
                        {
                            brokenItems.physicBody.rigidBody.AddExplosionForce(itemExplosionForce, position, explosionRadius, 1.0f, ForceMode.VelocityChange);
                        }

                        foreach (var brokenBodies in breakable.subBrokenBodies.Where(brokenBodies => brokenBodies != null))
                        {
                            brokenBodies.rigidBody.AddExplosionForce(itemExplosionForce, position, explosionRadius, 1.0f, ForceMode.VelocityChange);
                        }
                    }
                }
            }
        }

        public static void CreateElectrocutionRadius(Vector3 position,
                                                     float radius = 10.0f,
                                                     float electrocutionPower = 1.0f,
                                                     float electrocutionDuration = 7.5f,
                                                     EffectData electrocutionEffectData = null)
        {
            foreach (var collider in Physics.OverlapSphere(position, radius))
            {
                if (collider.GetComponentInParent<Creature>() is Creature creature && !creature.isPlayer)
                {
                    creature.TryElectrocute(electrocutionPower, electrocutionDuration, true, false, electrocutionEffectData);
                }

                if (collider.GetComponentInParent<Item>() is Item item && item != null)
                {
                    item.Imbue(Catalog.GetData<SpellCastCharge>("Lightning"));
                }
            }
        }

        public static bool PrimarilyX(this Vector3 vector3) => vector3.x > vector3.y && vector3.x > vector3.z;
        public static bool PrimarilyY(this Vector3 vector3) => vector3.y > vector3.x && vector3.y > vector3.z;
        public static bool PrimarilyZ(this Vector3 vector3) => vector3.z > vector3.x && vector3.z > vector3.y;

        public static T GetOrAddComponent<T>(this GameObject gameObject) where T : Component => gameObject?.GetComponent<T>() ? gameObject?.AddComponent<T>() : gameObject?.GetComponent<T>();

        public static T GetAndFindComponent<T>(this GameObject gameObject, string find) where T : Component => gameObject?.transform.Find(find)?.GetComponent<T>();

        public static T GetAndFindComponent<T>(this Transform transform, string find) where T : Component => transform?.gameObject.transform.Find(find)?.GetComponent<T>();

        public static T GetAndFindComponent<T>(this Rigidbody rigidbody, string find) where T : Component => rigidbody?.gameObject.transform.Find(find)?.GetComponent<T>();

        public static T GetAndFindComponent<T>(this Item item, string find) where T : Component => item?.gameObject.transform.Find(find)?.GetComponent<T>();

        public static void InertKill(this Creature creature)
        {
            creature?.Kill();
            creature?.ragdoll?.SetState(Ragdoll.State.Inert);
        }

        public static Vector3 Resize(this Item item, float newScaleSize) => item.transform.localScale = new Vector3(newScaleSize, newScaleSize, newScaleSize);

        public static void Unpenetrate(this Item item)
        {
            foreach (var damagers in item.collisionHandlers.SelectMany(collisionHandlers => collisionHandlers.damagers)) damagers.UnPenetrateAll();
        }

        public static void AddForce(this Creature creature, Vector3 force, ForceMode forceMode = ForceMode.Impulse)
        {
            if (creature != null)
            {
                creature.ragdoll.SetState(Ragdoll.State.Destabilized);
                creature.GetAllParts()?.physicBody.AddForce(force, forceMode);
            }
        }

        public static void Destroy(this GameObject gameObject) => Destroy(gameObject);

        public static Color Darken(this Color color, float darkenSpeed) => Color.Lerp(color, Color.clear, darkenSpeed);

        public static void AddForceTowards(this Item item, Vector3 target, ForceMode forceMode = ForceMode.VelocityChange) => item.physicBody?.AddForce((item.transform.position - target).normalized, forceMode);

        public static void AddForceTowards(this PhysicBody physicBody, Vector3 target, ForceMode forceMode = ForceMode.VelocityChange) => physicBody?.AddForce((physicBody.transform.position - target).normalized, forceMode);

        public static void AddForceTowards(this Rigidbody rigidbody, Vector3 target, ForceMode forceMode = ForceMode.VelocityChange) => rigidbody?.AddForce((rigidbody.position - target).normalized, forceMode);

        public static void AddForceTowards(this GameObject gameObject, Vector3 target, ForceMode forceMode = ForceMode.VelocityChange) => gameObject.GetPhysicBody()?.AddForce((gameObject.transform.position - target).normalized, forceMode);

        public static void AddForceTowards(this Transform transform, Vector3 target, ForceMode forceMode = ForceMode.VelocityChange) => transform.GetPhysicBody()?.AddForce((transform.position - target).normalized, forceMode);

        public static float Sqrt(this float number) => Mathf.Sqrt(number);

        public static float Abs(this float number) => Mathf.Abs(number);

        public static float Clamp(this float number, float minimum, float maximum) => Mathf.Clamp(number, minimum, maximum);

        public static float Lerp(this float minimum, float maximum, float speed) => Mathf.Lerp(minimum, maximum, speed);

        public static float GetRandomNumber(float minimum = Mathf.NegativeInfinity, float maximum = Mathf.Infinity) => Random.Range(minimum, maximum);

        public static void Clone(this Creature creature, Vector3 position, Quaternion rotation, string brainID = null)
        {
            creature.data.SpawnAsync(position,
                                     rotation.y,
                                     null,
                                     false,
                                     null,
                                     newCreature =>
                                     {
                                         newCreature.LoadBrain(creature.brain.instance.id);
                                     });
        }

        public static void Clone(this Item item, Vector3 position, Quaternion rotation, Vector3 force, ForceMode forceMode = ForceMode.Impulse)
        {
            item.data.SpawnAsync(newItem =>
            {
                newItem.transform.position = position;
                newItem.transform.rotation = rotation;
                newItem.physicBody.AddForce(force, forceMode);
            });
        }

        public delegate void LineCallback(LineRenderer lineRenderer);

        public static LineRenderer CreateLine(this GameObject gameObject, LineCallback callback = null)
        {
            if (gameObject != null)
            {
                var lineRenderer = gameObject.AddComponent<LineRenderer>();

                if (callback != null)
                {
                    callback.Invoke(lineRenderer);
                }

                return lineRenderer;
            }
            return null;
        }

        public static bool Facing(this Vector3 source, Vector3 target, float angle = 45.0f) => Vector3.Dot(source.normalized, target.normalized) >= Mathf.Cos(Mathf.Deg2Rad * angle);

        public static Direction GetFacedDirection(this Vector3 vector3, float angle = 45.0f)
        {
            if (vector3.Facing(Vector3.up, angle)) return Direction.Upward;
            if (vector3.Facing(Vector3.forward, angle)) return Direction.Forward;
            if (vector3.Facing(Vector3.left, angle)) return Direction.Leftward;
            if (vector3.Facing(Vector3.right, angle)) return Direction.Rightward;
            if (vector3.Facing(Vector3.back, angle)) return Direction.Backward;
            return vector3.Facing(Vector3.down, angle) ? Direction.Downward : Direction.Any;
        }

        public static RagdollHand Offhand(this Item item) => item.mainHandler.otherHand;

        public static RagdollHand Offhand(this SpellCaster spellCaster) => spellCaster.ragdollHand.otherHand;

        public static IEnumerator Decrease(this float value, float decrement, float delay = 1.0f)
        {
            while (value > 0)
            {
                value -= decrement;
                yield return new WaitForSeconds(delay);
            }
        }

        public static float VelocityDirection(this Item item, Vector3 direction) => Vector3.Dot(item.physicBody.velocity, direction);

        public static Vector3 AboveHead(this Creature creature, float distance = 1.50f) => creature.Head().transform.position + (creature.Head().upDirection * distance);

        public static Vector3 AboveFace(this Creature creature, float distance = 1.50f) => creature.Head().transform.position + (creature.Head().forwardDirection * distance);

        public static Axis GetAxis(this Vector3 vector3) => vector3.PrimarilyY()
                                                                ? vector3.PrimarilyX() ? Axis.X : Axis.Y
                                                                : vector3.PrimarilyX()
                                                                    ? Axis.X
                                                                    : Axis.Z;

        public static void ForBothHands(Action<RagdollHand> action, Creature creature)
        {
            action(creature.handLeft);
            action(creature.handRight);
        }

        public static void ForBothFeet(Action<RagdollFoot> action, Creature creature)
        {
            action(creature.footLeft);
            action(creature.footRight);
        }

        public static bool InWater(this Item item) => item.waterHandler.inWater;

        public static bool InWater(this Creature creature) => creature.waterHandler.inWater;

        public static float Round(this float number) => Mathf.RoundToInt(number);

        public static bool HasComponent<T>(this Item item) where T : Component => item.gameObject.GetComponent<T>() != null;

        public static bool HasComponent<T>(this GameObject gameObject) where T : Component => gameObject.GetComponent<T>() != null;

        public static bool HasComponent<T>(this Transform transform) where T : Component => transform.gameObject.GetComponent<T>() != null;

        public static bool HasComponent<T>(this PhysicBody physicBody) where T : Component => physicBody.gameObject.GetComponent<T>() != null;

        public static bool HasComponent<T>(this Rigidbody rigidbody) where T : Component => rigidbody.gameObject.GetComponent<T>() != null;

        public static RaycastHit[] ConeCastAll(Vector3 origin, float maxRadius, Vector3 direction, float maxDistance, float coneAngle)
        {
            var sphereCastHits = Physics.SphereCastAll(origin - new Vector3(0, 0, maxRadius), maxRadius, direction, maxDistance);
            var coneCastHitList = new List<RaycastHit>();
            if (sphereCastHits.Length > 0)
            {
                foreach (var hit in sphereCastHits)
                {
                    hit.collider.gameObject.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
                    var hitPoint = hit.point;
                    var directionToHit = hitPoint - origin;
                    var angleToHit = Vector3.Angle(direction, directionToHit);
                    if (angleToHit < coneAngle) coneCastHitList.Add(hit);
                }
            }

            var coneCastHits = new RaycastHit[coneCastHitList.Count];
            coneCastHits = coneCastHitList.ToArray();
            return coneCastHits;
        }

        public static Transform GetFreeSlot(this Creature creature) => creature.holders.Select(holders => holders.HasSlotFree() ? holders.slots.FirstOrDefault() : null).FirstOrDefault();

        public static void Holster(this Creature creature)
        {
            if (creature != null)
            {
                if (creature.GetFreeSlot() != null)
                {
                    if (creature.handLeft.HoldingItem())
                    {
                        BackpackHolder.instance?.StoreItem(creature.handLeft.grabbedHandle.item);
                    }
                    if (creature.handRight.HoldingItem())
                    {
                        BackpackHolder.instance?.StoreItem(creature.handRight.grabbedHandle.item);
                    }
                }
                else
                {
                    creature.holders?.FirstOrDefault()?.UnSnap(creature?.holders?.FirstOrDefault()?.items?.FirstOrDefault());

                    if (creature.handLeft.HoldingItem())
                    {
                        BackpackHolder.instance?.StoreItem(creature.handLeft.grabbedHandle.item);
                    }
                    if (creature.handRight.HoldingItem())
                    {
                        BackpackHolder.instance?.StoreItem(creature.handRight.grabbedHandle.item);
                    }
                }
            }
        }

        public static bool HoldingItem(this RagdollHand hand) => hand?.grabbedHandle?.item != null;

        public static void Set<T>(this object source, string fieldName, T value) => source?.GetType()?.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Instance)?.SetValue(source, value);

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component
        {
            if (gameObject.GetComponent<T>() != null) Object.Destroy(gameObject.GetComponent<T>());
        }

        public static void Unimbue(this Item item)
        {
            foreach (var imbues in item.imbues) imbues.energy = 0.0f;
        }

        public static RagdollPart GetAllParts(this Creature creature) => creature.ragdoll.parts.FirstOrDefault();

        public static RagdollPart GetAllSliceableParts(this Creature creature) => creature.ragdoll.parts.FirstOrDefault(part => part.sliceAllowed);

        public static Item GetItemInHolder(this Creature creature)
        {
            foreach (var holder in creature.holders)
            {
                if (holder.items.Count <= 0) continue;
                var itemInHolder = holder.items.FirstOrDefault();
                holder.UnSnap(itemInHolder);
                return itemInHolder;
            }
            return null;
        }

        public static bool Empty<T>(this List<T> list) => list.Count == 0;

        public static T RemoveAndGetElement<T>(this List<T> list, int index = 0)
        {
            if (list.Empty())
            {
                var indexOutOfRangeException = new IndexOutOfRangeException();
                throw indexOutOfRangeException;
            }

            var element = list[index];
            list.RemoveAt(index);
            return element;
        }

        public static Item HeldItem(this RagdollHand hand) => hand.grabbedHandle.item;

        public static T RandomElement<T>(this List<T> list) => list[Random.Range(0, list.Count)];

        public static T NextElement<T>(this List<T> list, int index) => index > list.Count - 1 ? list[index + 1] : list.FirstOrDefault();

        public static RagdollPart GetRandomPart(this Creature creature)
        {
            var partsList = creature.ragdoll.parts.Where(parts => parts.sliceAllowed).ToList();
            return partsList.RandomElement();
        }

        public static string RandomElement(this string[] stringArray) => stringArray[Random.Range(0, stringArray.Length)];

        public static void AddMultiple<T>(this List<T> list, params (T, T)[] tuples)
        {
            foreach (var tuple in tuples)
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
            }
        }

        public static void AddMultiple<T>(this List<T> list, params (T, T, T)[] tuples)
        {
            foreach (var tuple in tuples)
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
                list.Add(tuple.Item3);
            }
        }

        public static void AddMultiple<T>(this List<T> list, params (T, T, T, T)[] tuples)
        {
            foreach (var tuple in tuples)
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
                list.Add(tuple.Item3);
                list.Add(tuple.Item4);
            }
        }

        public static void AddMultiple<T>(this List<T> list, params (T, T, T, T, T)[] tuples)
        {
            foreach (var tuple in tuples)
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
                list.Add(tuple.Item3);
                list.Add(tuple.Item4);
                list.Add(tuple.Item5);
            }
        }

        public static void AddMultiple<T>(this List<T> list, params (T, T, T, T, T, T)[] tuples)
        {
            foreach (var tuple in tuples)
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
                list.Add(tuple.Item3);
                list.Add(tuple.Item4);
                list.Add(tuple.Item5);
                list.Add(tuple.Item6);
            }
        }

        public static void AddMultiple<T>(this List<T> list, params (T, T, T, T, T, T, T)[] tuples)
        {
            foreach (var tuple in tuples)
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
                list.Add(tuple.Item3);
                list.Add(tuple.Item4);
                list.Add(tuple.Item5);
                list.Add(tuple.Item6);
                list.Add(tuple.Item7);
            }
        }

        public static void AddMultiple<T>(this List<T> list, params (T, T, T, T, T, T, T, T)[] tuples)
        {
            foreach (var tuple in tuples)
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
                list.Add(tuple.Item3);
                list.Add(tuple.Item4);
                list.Add(tuple.Item5);
                list.Add(tuple.Item6);
                list.Add(tuple.Item7);
                list.Add(tuple.Item8);
            }
        }

        public static void AddMultiple<T>(this List<T> list, params (T, T, T, T, T, T, T, T, T)[] tuples)
        {
            foreach (var tuple in tuples)
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
                list.Add(tuple.Item3);
                list.Add(tuple.Item4);
                list.Add(tuple.Item5);
                list.Add(tuple.Item6);
                list.Add(tuple.Item7);
                list.Add(tuple.Item8);
                list.Add(tuple.Item9);
            }
        }

        public static void AddMultiple<T>(this List<T> list, params (T, T, T, T, T, T, T, T, T, T)[] tuples)
        {
            foreach (var tuple in tuples)
            {
                list.Add(tuple.Item1);
                list.Add(tuple.Item2);
                list.Add(tuple.Item3);
                list.Add(tuple.Item4);
                list.Add(tuple.Item5);
                list.Add(tuple.Item6);
                list.Add(tuple.Item7);
                list.Add(tuple.Item8);
                list.Add(tuple.Item9);
                list.Add(tuple.Item10);
            }
        }

        public static T HighestIndexValue<T>(this List<T> list) => list[list.Count - 1];

        public static T LowestIndexValue<T>(this List<T> list) => list[0];

        public static void ShowMessage(string text,
                                       int priority = 1,
                                       float showDelay = 0.0f,
                                       bool tutorialMessage = false,
                                       bool skippable = true,
                                       bool warnPlayer = true,
                                       MessageAnchorType anchorType = MessageAnchorType.Head)
            => DisplayMessage.instance.ShowMessage(new DisplayMessage.MessageData(text,
                                                                                  "",
                                                                                  "",
                                                                                  "",
                                                                                  priority,
                                                                                  showDelay,
                                                                                  null,
                                                                                  null,
                                                                                  tutorialMessage,
                                                                                  skippable,
                                                                                  warnPlayer: warnPlayer,
                                                                                  anchorType: anchorType));

        public static List<T> RemoveFirst<T>(this List<T> list)
        {
            if (list?.Count > 0)
            {
                list.RemoveAt(0);
            }
            else
            {
                var indexOutOfRangeException = new IndexOutOfRangeException();
                throw indexOutOfRangeException;
            }
            return list;
        }

        public static List<T> RemoveLast<T>(this List<T> list)
        {
            if (list?.Count > 0)
            {
                list.RemoveAt(list.Count - 1);
            }
            else
            {
                var indexOutOfRangeException = new IndexOutOfRangeException();
                throw indexOutOfRangeException;
            }
            return list;
        }

        public static Item LastHeldItem(this RagdollHand hand) => Item.allActive.Select(item => item != null && hand == item.lastHandler ? item : null).FirstOrDefault();

        public static string Where(this string source, Func<char, bool> predicate) => source != null ? new string(source.Where(predicate).ToArray()) : throw new ArgumentNullException(nameof(source));

        public static void SpawnMeteor(Vector3 position,
                                       Quaternion rotation,
                                       Vector3 velocity,
                                       float meteorVelocity = 3.0f,
                                       float meteorExplosionRadius = 5.0f,
                                       float meteorExplosionForce = 3.0f,
                                       float meteorItemExplosionForce = 5.0f,
                                       float meteorExplosionDamage = 20.0f,
                                       ItemData meteorItemData = null,
                                       EffectData meteorEffectData = null,
                                       EffectData meteorExplosionEffectData = null,
                                       GuidanceMode guidance = GuidanceMode.NonGuided)
        {
            var meteorIntensityCurve = AnimationCurve.EaseInOut(0f, 0f, 0.5f, 1f);

            meteorItemData?.SpawnAsync(item =>
            {
                item.disallowDespawn = true;
                var component = item.GetComponent<ItemMagicAreaProjectile>();

                if (component == null) return;
                component.OnHit += collisionInstance =>
                {
                    CreateExplosion(collisionInstance.contactPoint, meteorExplosionRadius, meteorExplosionForce, meteorItemExplosionForce, meteorExplosionDamage, meteorExplosionEffectData);
                };
                component.guidance = guidance;
                component.guidanceAmount = 5.0f;
                component.speed = meteorVelocity;
                component.guidanceFunc = () => Vector3
                                               .Slerp(Vector3.Slerp(-Player.currentCreature.handLeft.transform.right,
                                                                    -Player.currentCreature.handRight.transform.right,
                                                                    0.5f).normalized,
                                                      (Vector3.Slerp(Player.currentCreature.handLeft.transform.position,
                                                                     Player.currentCreature.handRight.transform.position,
                                                                     0.5f)
                                                       - Vector3.Lerp(Player.currentCreature.ragdoll.headPart.transform.position,
                                                                      Player.currentCreature.transform.position,
                                                                      0.5f)).normalized,
                                                      0.5f).normalized;
                component.effectIntensityCurve = meteorIntensityCurve;
                item.physicBody.AddForce(velocity * meteorVelocity, ForceMode.Impulse);
                component.Fire(velocity, meteorEffectData, null, Player.currentCreature.ragdoll, (HapticDevice)3);
            }, position, rotation);
        }

        public static void Haptic(this RagdollHand hand, float hapticIntensity = 0.5f) => hand.playerHand.controlHand?.HapticShort(hapticIntensity);

        public static bool EndOnGrip(this SpellCaster spellCaster, bool enabled)
        {
            SpellCastCharge spellCastCharge = spellCaster.spellInstance as SpellCastCharge;
            return spellCastCharge != null && (enabled ? spellCastCharge.endOnGrip = true : spellCastCharge.endOnGrip = false);
        }

        public static bool AnyPartDismembered(this Creature creature)
            => creature.Head().isSliced
               || creature.Neck().isSliced
               || creature.Torso().isSliced
               || creature.LeftHand().isSliced
               || creature.RightHand().isSliced
               || creature.LeftArm().isSliced
               || creature.RightArm().isSliced
               || creature.LeftLeg().isSliced
               || creature.RightLeg().isSliced
               || creature.LeftFoot().isSliced
               || creature.RightFoot().isSliced;

        public static void SlicePart(this Creature creature, RagdollPart.Type ragdollPartType) => creature.GetRagdollPart(ragdollPartType)?.TrySlice();

        public static void SetPositionAndRotation(this Creature creature, Vector3 position, Quaternion rotation)
        {
            if (creature != null)
            {
                creature.locomotion.GetPhysicBody().transform.position = position;
                creature.locomotion.GetPhysicBody().transform.rotation = rotation;
            }
        }

        public static void AddForce(this Player player, Vector3 force, ForceMode forceMode = ForceMode.VelocityChange) => player?.locomotion?.GetPhysicBody()?.AddForce(force, forceMode);

        public static Direction GetVelocityDirection(this RagdollHand hand, float velocity)
        {
            if (hand.VelocityDirection(Vector3.up) >= velocity) return Direction.Upward;
            if (hand.VelocityDirection(Vector3.forward) >= velocity) return Direction.Forward;
            if (hand.VelocityDirection(Vector3.left) >= velocity) return Direction.Leftward;
            if (hand.VelocityDirection(Vector3.right) >= velocity) return Direction.Rightward;
            if (hand.VelocityDirection(Vector3.back) >= velocity) return Direction.Backward;
            return hand.VelocityDirection(Vector3.down) >= velocity ? Direction.Downward : Direction.Any;
        }
    }
}

public class Continuum
{
    private Continuum continuum;
    private Func<bool> condition;
    private Action action;
    private Type type = Type.Start;

    private enum Type
    {
        Start,
        WaitFor,
        Do,
        End
    }

    public static Continuum Start() => new Continuum();

    public Continuum WaitFor(Func<bool> condition)
    {
        continuum = new Continuum { condition = condition, type = Type.WaitFor };
        return continuum;
    }

    public Continuum Do(Action action)
    {
        continuum = new Continuum { action = action, type = Type.Do };
        return continuum;
    }

    public void Update()
    {
        switch (type)
        {
            case Type.Start:
                if (continuum is null)
                {
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
                if (condition.Invoke())
                {
                    if (continuum is null)
                    {
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
                if (continuum is null)
                {
                    type = Type.Start;
                    return;
                }
                type = continuum.type;
                action = continuum.action;
                condition = continuum.condition;
                continuum = continuum.continuum;
                break;

            case Type.End:
                return;
        }
    }
}

public enum Direction
{
    Any,
    Upward,
    Forward,
    Leftward,
    Rightward,
    Backward,
    Downward
}

public enum Axis
{
    X, Y, Z
}