using UnityEngine;
using UnityEngine.UI;
namespace Es.InkPainter.Sample
{
    public class MousePainter : MonoBehaviour
    {
        public static MousePainter Instance;
        public int UserID = 0;
        
        public Transform Canvas;
        public Camera UICamera;
        /// <summary>
        /// Types of methods used to paint.
        /// </summary>
        [System.Serializable]
        private enum UseMethodType
        {
            RaycastHitInfo,
            WorldPoint,
            NearestSurfacePoint,
            DirectUV,
        }

        [SerializeField]
        public Brush brush;

        [SerializeField]
        private UseMethodType useMethodType = UseMethodType.RaycastHitInfo;

        void Awake()
        {
            Instance = this;
        }

        void Drawing()
        {

            var ray = Camera.main.ScreenPointToRay(UICamera.WorldToScreenPoint(GameManager.Instance.Hand[UserID].localPosition));
            bool success = true;
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                //print(hitInfo.point + "       " + ray + "  " + hitInfo.transform.name);

                var paintObject = hitInfo.transform.GetComponent<InkCanvas>();
                //var paintObject = Canvas.transform.GetComponent<InkCanvas>();
                if (paintObject != null)
                {

                    switch (useMethodType)
                    {
                        case UseMethodType.RaycastHitInfo:
                            success = paintObject.Paint(brush, hitInfo);
                            break;

                        case UseMethodType.WorldPoint:
                            success = paintObject.Paint(brush, hitInfo.point);
                            break;

                        case UseMethodType.NearestSurfacePoint:
                            success = paintObject.PaintNearestTriangleSurface(brush, hitInfo.point);
                            break;

                        case UseMethodType.DirectUV:
                            if (!(hitInfo.collider is MeshCollider))
                                Debug.LogWarning("Raycast may be unexpected if you do not use MeshCollider.");
                            success = paintObject.PaintUVDirect(brush, hitInfo.textureCoord);
                            break;
                    }
                }
               
                if (!success)
                    Debug.LogError("Failed to paint.");
            }
            //else
            //{
               
            //}
        }
        private void Update()
        {
            if (UserID == 0 &&  GameManager.Instance.Open)
            {
                Drawing();
            }


            if (UserID == 1 && GameManager.Instance.Open_1 /*|| Input.GetKey(KeyCode.Space)*/)
            {
                Drawing();
            }
             
             if (UserID == 2 && GameManager.Instance.Open_2 /*|| Input.GetKey(KeyCode.Space)*/)
            {
                Drawing();
            }
             
        }

        //public void OnGUI()
        //{
        //    if (GUILayout.Button("Reset"))
        //    {
        //        foreach (var canvas in FindObjectsOfType<InkCanvas>())
        //            canvas.ResetPaint();
        //    }
        //}
        public void ResetManager()
        {
            foreach (var canvas in FindObjectsOfType<InkCanvas>())
                canvas.ResetPaint();
        }
    }
}