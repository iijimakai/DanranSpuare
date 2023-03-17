#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using System.IO;

namespace CreateCode
{
    public class PlayerControllerCreate : EditorWindow
    {
        private string scriptName = "PlayerController";
        private string createPath = "Assets/Scripts/";

        [MenuItem("CreateCode/PlayerController")]
        private static void ShowWindow()
        {
            PlayerControllerCreate window = GetWindow<PlayerControllerCreate>();
            window.titleContent = new GUIContent("CreateMenu");
        }

        private void OnGUI()
        {
            GUILayout.Label("スクリプト名を入力してください");
            scriptName = EditorGUILayout.TextField(scriptName, GUILayout.Height(20));

            GUILayout.Label("以下の場所に生成されます");
            GUILayout.Label(createPath + scriptName + ".cs");

            if (GUILayout.Button("スクリプト生成"))
            {
                CreateCode();
            }
        }

        private void CreateCode()
        {
            string filePath = createPath + scriptName + ".cs";
            filePath = AssetDatabase.GenerateUniqueAssetPath(filePath);
            File.WriteAllText(filePath, SetCode());
            AssetDatabase.Refresh();
            Debug.Log("生成終了");
        }

        private string SetCode()
        {
            string code;
            code = @"using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [Header(""スピード調整"")]
    [SerializeField] private float spead = 5;

    [Header(""きびきび動かす場合増やす"")]
    [SerializeField] private float moveForceMultiplier = 100;

    [Header(""回転速度"")]
    [SerializeField] private float angleSpead = 100;

    private Vector3 move;
    private Vector3 subtractmove;
    private Vector3 rotatemove;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        PlayerMove();
    }

    private void PlayerMove()
    {
        float x = 0, z = 0;
        x = Input.GetAxis(""Horizontal"") * angleSpead;
        z = Input.GetAxis(""Vertical"");

        if (z <= 0)
        {
            z = 0;
        }

        move = new Vector3(0, 0, z);
        rotatemove = new Vector3(0, x, 0) * Time.deltaTime;
        move = move * spead;
        move = transform.TransformDirection(move);
        subtractmove = new Vector3(move.x - rb.velocity.x, 0f, move.z - rb.velocity.z) * Time.deltaTime;
        transform.Rotate(rotatemove);
        rb.AddForce(moveForceMultiplier * (subtractmove));
    }
}
";
            return code;
        }
    }
}
#endif