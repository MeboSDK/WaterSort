using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Core.Enums;
using DG.Tweening.Plugins.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ThomassPuzzle.Helpers
{
    public class AnimationHelper
    {
        public static TweenerCore<Quaternion,Vector3, QuaternionOptions> Rotate(GameObject @object, float radius,float delay)
        {
            return @object.transform.DOLocalRotate(new Vector3(0, 0, radius), delay).SetEase(Ease.Linear);
        }

        public static TweenerCore<Vector3, Vector3, VectorOptions> Moving(Flask from, Flask to,float delay)
        {
            var targetPosition = new Vector3(to.GetDotRectForLiquidLine().transform.position.x, to.transform.position.y + 1f, to.transform.position.z);

            return from.transform.DOMove(targetPosition, delay);
        }

    }
}
