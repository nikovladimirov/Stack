using DefaultNamespace.Enums;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DefaultNamespace
{
    public class TestButtonBehaviour : MonoBehaviour
    {
        private Button _button;

        [SerializeField] private TestEnum _method;

        private void Awake()
        {
            _button = GetComponent<Button>();
            _button.onClick.AddListener(TestButtonExecute);
        }

        private void TestButtonExecute()
        {
            switch (_method)
            {
                case TestEnum.SpawnCube:
                    GameManager.Instance.SpawnNextCube();
                    break;
            }
        }
    }
}