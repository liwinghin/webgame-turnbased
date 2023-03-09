using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace MB
{
    public interface ICamPart
    {
        Transform transform { get; }

        void StartAttach(CamRuntime cam);
        void StopAttach(CamRuntime cam);
    }
    public static class CamPartExtension
    {
        public static void Attach(this ICamPart part, Transform target) => SceneObjectUtils.SetParent(part.transform, target);

    }
}