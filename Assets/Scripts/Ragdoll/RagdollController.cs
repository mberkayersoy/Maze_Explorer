using System;
using System.Collections.Generic;
using System.Linq;
using Extensions.Other;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace _Game.Extensions.Ragdoll
{
    [RequireComponent(typeof(Animator))]
    public class RagdollController : MonoBehaviour
    {
        //-------Public Variables-------//
        [ReadOnly] public bool IsRagdolled;

        //------Serialized Fields-------//
        [TabGroup("Config"), SerializeField] private List<BonePart> Parts;
        [TabGroup("Config"), SerializeField] private bool AssignLayerToParts;
        [TabGroup("Config"), SerializeField, ShowIf(nameof(AssignLayerToParts))] private LayerMask LayerPart;
        [TabGroup("Config"), SerializeField] private bool HaveMainColliders;
        [TabGroup("Config"), SerializeField, ShowIf(nameof(HaveMainColliders))] private Collider[] MainColliders;

        [TabGroup("Event"), SerializeField] private UnityEvent OnRagdolled;
        [TabGroup("Events"), SerializeField] private UnityEvent OnRagdollCancelled;
        //------Private Variables-------//
        private Animator _animator;

        #region UNITY_METHODS

        #endregion


        #region PUBLIC_METHODS

        [Button(ButtonSizes.Gigantic), GUIColor(0, 1, 0)]
        public void Initialize()
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            InitializeBoneParts();
        }

        [Button, TabGroup("Functions")]
        public void SetRagdoll(bool flag)
        {
            CheckAnimator(flag);
            CheckMainCollider(flag);
            SetRigidbodyKinematics(flag);
            IsRagdolled = flag;
            RaiseRagdollEvents(flag);
        }

        [Button, TabGroup("Functions")]
        public void AddForce(BoneName bone, Vector3 force, ForceMode forceMode = ForceMode.Impulse)
        {
            var part = Parts.FirstOrDefault((val) => val.BoneName == bone);
            part?.Rigidbody.AddForce(force, forceMode);
        }

        [Button, TabGroup("Functions")]
        public void AddForceToAllParts(Vector3 force, ForceMode forceMode = ForceMode.Impulse)
        {
            foreach (var part in Parts)
            {
                part.Rigidbody.AddForce(force, forceMode);
            }
        }

        [Button, TabGroup("Functions")]
        public float GetTotalMass()
        {
            return Parts.Sum(part => part.Rigidbody.mass);
        }

        [Button, TabGroup("Functions")]
        public void SetTotalMass(float totalMass)
        {
            foreach (var part in Parts)
            {
                part.Rigidbody.mass = GetBoneMass(part.BoneName, totalMass);
            }
        }

        [Button, TabGroup("Functions")]
        public void SetGravityActive(bool flag)
        {
            foreach (var part in Parts)
            {
                part.Rigidbody.useGravity = flag;
            }
        }

        [Button, TabGroup("Functions")]
        public void SetColliderActive(bool flag)
        {
            if (HaveMainColliders)
            {
                foreach (var col in MainColliders)
                {
                    col.enabled = !flag;
                }
            }
            foreach (var part in Parts)
            {
                part.Collider.enabled = flag;
            }
        }

        [Button, TabGroup("Functions")]
        public void SetColliderCollisionDetection(CollisionDetectionMode mode)
        {
            foreach (var part in Parts)
            {
                part.Rigidbody.collisionDetectionMode = mode;
            }
        }

        [Button(ButtonSizes.Large), GUIColor(1f, 0f, 0f)]
        public void RemoveRagdoll()
        {
            foreach (var part in Parts)
            {
                DestroyImmediate(part.Joint);
                DestroyImmediate(part.Rigidbody);
                DestroyImmediate(part.Collider);
            }
            Parts = new List<BonePart>();
        }
        #endregion


        #region PRIVATE_METHODS

        private static float GetBoneMass(BoneName bone, float totalMass)
        {
            return bone switch
            {
                BoneName.Chest => totalMass * 0.2f,
                BoneName.Head => totalMass * 0.05f,
                BoneName.Hips => totalMass * 0.2f,
                BoneName.LeftFoot => totalMass * 0.05f / 2f,
                BoneName.RightFoot => totalMass * 0.05f / 2f,
                BoneName.LeftHand => totalMass * 0.02f / 2f,
                BoneName.RightHand => totalMass * 0.02f / 2f,
                BoneName.LeftLowerArm => totalMass * 0.05f / 2f,
                BoneName.RightLowerArm => totalMass * 0.05f / 2f,
                BoneName.LeftUpperArm => totalMass * 0.08f / 2f,
                BoneName.RightUpperArm => totalMass * 0.08f / 2f,
                BoneName.LeftUpperLeg => totalMass * 0.2f / 2f,
                BoneName.RightUpperLeg => totalMass * 0.2f / 2f,
                BoneName.RightLowerLeg => totalMass * 0.15f / 2f,
                BoneName.LeftLowerLeg => totalMass * 0.15f / 2f,
                _ => throw new ArgumentOutOfRangeException(nameof(bone), bone, null)
            };
        }

        private void InitializeBoneParts()
        {
            Parts = new List<BonePart>();
            var parts = new List<BonePart>()
            {
                CreateNewPart(HumanBodyBones.Hips),
                CreateNewPart(HumanBodyBones.LeftUpperLeg),
                CreateNewPart(HumanBodyBones.LeftLowerLeg),
                CreateNewPart(HumanBodyBones.RightUpperLeg),
                CreateNewPart(HumanBodyBones.RightLowerLeg),
                CreateNewPart(HumanBodyBones.LeftUpperArm),
                CreateNewPart(HumanBodyBones.LeftLowerArm),
                CreateNewPart(HumanBodyBones.RightUpperArm),
                CreateNewPart(HumanBodyBones.RightLowerArm),
                CreateNewPart(HumanBodyBones.Chest),
                CreateNewPart(HumanBodyBones.Head),
                CreateNewPart(HumanBodyBones.LeftFoot),
                CreateNewPart(HumanBodyBones.RightFoot),
                CreateNewPart(HumanBodyBones.LeftHand),
                CreateNewPart(HumanBodyBones.RightHand)
            };
            Parts = parts;
            if (AssignLayerToParts)
                ApplyLayerMask();
        }

        private void ApplyLayerMask()
        {
            foreach (var part in Parts)
            {
                part.GameObject.layer = LayerPart.GetLayerFromLayerMask();
            }
        }

        private BonePart CreateNewPart(HumanBodyBones bone)
        {
            var boneIndex = 0;
            var enumNames = System.Enum.GetNames(typeof(BoneName));
            for (int ind = 0; ind < enumNames.Length; ind++)
            {
                if (bone.ToString() == enumNames[ind])
                {
                    boneIndex = ind;
                    break;
                }
            }
            var boneTransform = _animator.GetBoneTransform(bone);
            var newBonePart = new BonePart
            {
                BoneName = (BoneName)boneIndex,
                Collider = boneTransform.GetComponent<Collider>(),
                Joint = boneTransform.GetComponent<Joint>(),
                Rigidbody = boneTransform.GetComponent<Rigidbody>(),
                Transform = boneTransform,
                GameObject = boneTransform.gameObject
            };
            return newBonePart;
        }

        private void CheckMainCollider(bool flag)
        {
            if (!HaveMainColliders)
                return;
            foreach (var col in MainColliders)
            {
                col.enabled = !flag;
                col.TryGetComponent(out Rigidbody rb);
                if (!rb)
                    return;
                rb.isKinematic = flag;
            }
        }

        private void SetRigidbodyKinematics(bool flag)
        {
            foreach (var part in Parts)
            {
                part.Rigidbody.isKinematic = !flag;
                part.Collider.enabled = flag;
            }
        }

        private void CheckAnimator(bool flag)
        {
            if (_animator == null)
                _animator = GetComponent<Animator>();
            _animator.enabled = !flag;
        }

        private void RaiseRagdollEvents(bool flag)
        {
            if (flag)
                OnRagdolled?.Invoke();
            else
                OnRagdollCancelled?.Invoke();
        }
        #endregion

    }
}