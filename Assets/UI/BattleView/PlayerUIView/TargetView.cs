using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Assets.Other;
using UnityEngine;
using UnityEngine.UI;
using Assets.Models.Pool;

namespace Assets.UI.BattleView.PlayerUIView
{
    public class TargetView
    {
        SpecialObjectPool<GameObject<Image>> pointerPool;
        SpecialObjectPool<GameObject<TargetPointerBehaviour>> outPointerPool;
        Canvas canvas;

        public TargetView(Canvas canvas)
        {
            this.canvas = canvas;
            var image = new GameObject().AddComponent<Image>();
            image.sprite = Resources.Load<Sprite>(@"Views\TargetLock");
            pointerPool = new SpecialObjectPool<GameObject<Image>>(new GameObject<Image>(image, canvas.transform));

            outPointerPool = new SpecialObjectPool<GameObject<TargetPointerBehaviour>>(new GameObject<TargetPointerBehaviour>(Resources.Load<TargetPointerBehaviour>(@"Views\TargetPointer"), canvas.transform));
        }

        public void SetTargets(IEnumerable<Vector3> targets)
        {
            pointerPool.ReleaseAll();
            outPointerPool.ReleaseAll();

            float halfWidth = Screen.width * .5f - 100;
            float halfHeight = Screen.height * .5f - 100;

            SquareBounds screenBounds = new SquareBounds(-halfWidth, halfWidth, -halfHeight, halfHeight);

            Camera camera = Camera.main;
            Transform cameraTransform = camera.transform;
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraPosition = cameraTransform.position;
            Vector3 halfScreenSize = new Vector3(Screen.width * .5f, Screen.height * .5f);

            foreach (var t in targets)
            {
                Vector3 position = camera.WorldToScreenPoint(t) - halfScreenSize;
                position.z = 0;

                if (Vector3.Dot(cameraForward, cameraPosition - t) > 0)
                {
                    position.Normalize();
                    position *= -Mathf.Max(halfScreenSize.x, halfScreenSize.y);
                    screenBounds.ToBounds(ref position);

                    var v = outPointerPool.Get().obj;
                    v.rectTransform.localPosition = position;
                    v.color = Color.red;
                    v.rectTransform.rotation = Quaternion.AngleAxis(Mathf.Acos(position.y < 0 ? -position.normalized.x : position.normalized.x) * Mathf.Rad2Deg + (position.y < 0 ? 0 : 180), Vector3.forward);
                    v.UpdateGear();
                }
                else if (screenBounds.ToBounds(ref position))
                {
                    var v = outPointerPool.Get().obj;
                    v.rectTransform.localPosition = position;
                    v.color = Color.red;
                    v.rectTransform.rotation = Quaternion.AngleAxis(Mathf.Acos(position.y < 0 ? -position.normalized.x : position.normalized.x) * Mathf.Rad2Deg + (position.y < 0 ? 0 : 180), Vector3.forward);
                    v.UpdateGear();
                }
                else
                {
                    var v = pointerPool.Get().obj;
                    v.rectTransform.localPosition = position;
                    v.color = Color.red;
                    v.rectTransform.eulerAngles += new Vector3(0, 0, 180) * Time.deltaTime;
                }
            }
        }
    }
}
