using UnityEngine;
using UnityEngine.EventSystems;

namespace CoreFramework.Input
{
    /// <summary>
    /// Tracking low level input
    /// </summary>
    public abstract class CoreInputController : BaseController
    {
        protected InputModel m_InputModel;
        protected CameraModel m_CameraModel;

        public CoreInputController(Dispatcher dispatcher) : base(dispatcher)
        { }

        public override void InitializeComplete()
        {
            m_InputModel = Dispatcher.GetModel<InputModel>();
            m_CameraModel = Dispatcher.GetModel<CameraModel>();

            UpdateModel updateModel = Dispatcher.GetModel<UpdateModel>();
            updateModel.OnUpdate += Update;
        }

        public void Update(float deltaTime)
        {
            if (InputDetected() && !IsPointerOverUI() && m_CameraModel.MainCamera != null)
            {
                m_InputModel.Touch(UnityEngine.Input.mousePosition);

                Ray ray = m_CameraModel.MainCamera.ScreenPointToRay(UnityEngine.Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    m_InputModel.LastTouchHitPoint = hit.point;
                }
            }

            HandleSpecialInput();
        }

        protected abstract void HandleSpecialInput();

        private bool InputDetected()
        {
#if !UNITY_EDITOR
            return UnityEngine.Input.touchCount > 0 && UnityEngine.Input.touches[0].phase == TouchPhase.Began;
#endif

            return UnityEngine.Input.GetMouseButtonDown(0);
        }

        private bool IsPointerOverUI()
        {
#if !UNITY_EDITOR
            return EventSystem.current.IsPointerOverGameObject(UnityEngine.Input.touches[0].fingerId); 
#endif
            return EventSystem.current.IsPointerOverGameObject();
        }
    }
}
