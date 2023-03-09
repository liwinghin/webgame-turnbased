using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public interface IShipPart
    {
        Transform transform { get; }

        void PreAttach(ShipRuntime ship);
        void StartAttach(ShipRuntime ship);
        void StopAttach(ShipRuntime ship);
    }
    public static class ShipPartExtension
    {
        public static void SetParent(this IShipPart part, Transform target) => SceneObjectUtils.SetParent(part.transform, target);

        public static void FollowPosition(this IShipPart part, Transform target) => SceneObjectUtils.FollowPosition(part.transform, target);

        public static void FollowRotation(this IShipPart part, Transform target) => SceneObjectUtils.FollowRotation(part.transform, target);

        public static void ApplyMaterial(this IShipPart part, ref Material targetValue, Material setValue, Material defaultValue, bool withActive, params Renderer[] renderers)
        {
            targetValue = setValue ? setValue : defaultValue;
            SceneObjectUtils.SetActiveMaterial(targetValue, renderers);
        }
    }
}