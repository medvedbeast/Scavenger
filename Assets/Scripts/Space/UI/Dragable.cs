using UnityEngine;
using UnityEngine.EventSystems;
using Common;

namespace Space
{
    public class Dragable : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public RectTransform boundingObject;
        public GameObject temporalParent;

        [HideInInspector]
        public Vector3 mouseOffset;
        [HideInInspector]
        public GameObject startParent;
        [HideInInspector]
        public Vector3 startPosition;
        [HideInInspector]
        public RectTransform rectTransform;
        [HideInInspector]
        public event Action OnDragEnded;

        /// <summary>
        /// OnDragEnded action delegate.
        /// </summary>
        /// <param name="position">If position == null, Dragable is out of bounds, otherwise position is point, where drag ended.</param>
        [HideInInspector]
        public delegate void Action(GameObject subject, object position);

        public void Start()
        {
            rectTransform = GetComponent<RectTransform>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            startPosition = rectTransform.anchoredPosition3D;
            startParent = rectTransform.parent.gameObject;
            transform.SetParent(temporalParent.transform);

            Vector3 mouse = new Vector3(Input.mousePosition.x - Screen.width / 2, Input.mousePosition.y - Screen.height / 2, 0);
            mouseOffset = new Vector3(rectTransform.anchoredPosition3D.x - mouse.x, rectTransform.anchoredPosition3D.y - mouse.y, 0);
        }

        public void OnDrag(PointerEventData eventData)
        {
            Vector3 mouse = new Vector3(Input.mousePosition.x - Screen.width / 2 + mouseOffset.x, Input.mousePosition.y - Screen.height / 2 + mouseOffset.y, 0);
            rectTransform.anchoredPosition3D = mouse;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            float minX = -boundingObject.rect.width / 2;
            float maxX = boundingObject.rect.width / 2;
            float minY = -boundingObject.rect.height / 2;
            float maxY = boundingObject.rect.height / 2;
            float x = rectTransform.anchoredPosition3D.x;
            float y = rectTransform.anchoredPosition3D.y;
            if (x < minX || x > maxX || y < minY || y > maxY)
            {
                if (OnDragEnded != null)
                {
                    OnDragEnded(transform.gameObject, null);
                }
            }
            {
                if (OnDragEnded != null)
                {
                    OnDragEnded(transform.gameObject, new Vector3(x, y, 0));
                }
            }
        }
    }
}