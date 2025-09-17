using UnityEngine;
using UnityEngine.InputSystem;
using VContainer;
using MessagePipe;
using Application;

namespace Infrastructure.Input
{
    /// <summary>
    /// Обработчик ввода для управления камерой и действиями на сетке.
    /// </summary>
    public class InputController : MonoBehaviour
    {
        //---------------------------------------------------
        [Inject] [SerializeField] private InputSystem_Actions _inputActions;
        [Inject] private IPublisher<CellHoveredEvent> _cellHoveredPublisher;
        [Inject] private IPublisher<CellClickedEvent> _cellClickedPublisher;


        //---------------------------------------------------
        [Inject] private LayerMask _cellLayerMask;
        //---------------------------------------------------
        private Collider _lastHoveredCell = null;
        //---------------------------------------------------

        private Camera _camera;
        private Vector3 _moveDirection;
        private float _zoomDelta;
        //!---------------------------------------------------

        private void Awake()
        {
            _camera = Camera.main;

            _inputActions.Enable();
        }

        void OnEnable()
        {
            // Подписка на событие левой кнопки мыши (например, Player.SelectCell)
            _inputActions.Player.Attack.performed += OnSelectCell;
        }

        void OnDisable()
        {
            _inputActions.Player.Attack.performed -= OnSelectCell;
        }

        private void Update()
        {
            MoveCamera();
            
            ZoomCamera();

            RaycastOnCell();
        }

        // Зум камеры
        private void ZoomCamera()
        {
            if (_zoomDelta != 0f)
            {
                _camera.orthographicSize = Mathf.Clamp(_camera.orthographicSize - _zoomDelta, 5f, 50f);
                _zoomDelta = 0f;
            }
        }

        // Перемещение камеры
        private void MoveCamera()
        {
            Vector2 input = _inputActions.Player.Move.ReadValue<Vector2>();

            _moveDirection = new Vector3(input.x, 0, input.y);

            if (_moveDirection != Vector3.zero)
            {
                _camera.transform.position += _moveDirection * Time.deltaTime * 10f;
            }
        }
        
        // Наведение на клетку
        private void RaycastOnCell()
        {
            Ray ray = _camera.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit, _cellLayerMask))
            {
                if (hit.collider != _lastHoveredCell)
                {
                    _cellHoveredPublisher.Publish(new CellHoveredEvent(hit.collider));

                    _lastHoveredCell = hit.collider;
                }
            }
            else
            {
                // Если луч не попал ни в какую клетку, сбросить подсветку
                if (_lastHoveredCell != null)
                {
                    _cellHoveredPublisher.Publish(new CellHoveredEvent(null));

                    _lastHoveredCell = null;
                }
            }
        }

        // Input System: WASD
        public void OnMove(InputAction.CallbackContext context)
        {
            Debug.Log("OnMove triggered" + context.phase);
            Vector2 input = context.ReadValue<Vector2>();
            _moveDirection = new Vector3(input.x, 0, input.y);
        }

        // Input System: колесо мыши
        public void OnZoom(InputAction.CallbackContext context)
        {
            float scroll = context.ReadValue<float>();
            _zoomDelta = scroll;
        }

        // Input System: выбор клетки (клик)
        public void OnSelectCell(InputAction.CallbackContext context)
        {
            _cellClickedPublisher.Publish(new CellClickedEvent(_lastHoveredCell));
        }

        // Input System: горячие клавиши (1/2/3/R/Del)
        public void OnHotkey(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                var key = context.control.displayName;
                Debug.Log($"Нажата горячая клавиша: {key}");
                // TODO: обработать выбор здания, вращение, удаление
            }
        }
    }
}
