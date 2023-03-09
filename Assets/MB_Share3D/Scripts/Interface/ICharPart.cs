using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public interface ICharPart
    {
        Transform transform { get; }

        void PreAttach(CharRuntime character, CharPartSide side);
        void StartAttach(CharRuntime character, CharPartSide side);
        void StopAttach(CharRuntime character, CharPartSide side);
    }
    public static class CharPartExtension
    {
        public static void SetParent(this ICharPart part, Transform target) => SceneObjectUtils.SetParent(part.transform, target);

        public static void FollowPosition(this ICharPart part, Transform target) => SceneObjectUtils.FollowPosition(part.transform, target);

        public static void FollowRotation(this ICharPart part, Transform target) => SceneObjectUtils.FollowRotation(part.transform, target);
        public static void FollowRotation(this ICharPart part, Transform target, Vector3 offset) => SceneObjectUtils.FollowRotation(part.transform, target, offset);

        public static void ApplyMaterial(this ICharPart part, ref Material targetValue, Material setValue, Material defaultValue, bool withActive, params Renderer[] renderers)
        {
            targetValue = setValue ? setValue : defaultValue;
            SceneObjectUtils.SetActiveMaterial(targetValue, renderers);
        }
    }
}