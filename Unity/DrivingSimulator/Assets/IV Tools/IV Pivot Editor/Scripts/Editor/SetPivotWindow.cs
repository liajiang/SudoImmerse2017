using System;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class SetPivotWindow : EditorWindow
{
    private Vector3 _pivot;

    private GameObject _obj; //Selected object in the Hierarchy
    private MeshFilter _meshFilter; //Mesh Filter of the selected object
    private Mesh _mesh; //Mesh of the selected object

    private bool _setOnScene = true;

    private bool _snapToVertex = false;

    private static bool _drawBoundingBox = true;

    private Transform _transform;

    private Transform _trPivot;

    private static SetPivotWindow _window;

    private float _x = .5f;
    private float _y = .5f;
    private float _z = .5f;

    private Collider _collider;

    private Vector3 _initialColliderCenter;

    private PropertyInfo _colliderCenterProperty;

    private Vector3 Min
    {
        get { return _transform.TransformPoint(_mesh.bounds.min); }
    }

    private Vector3 Center
    {
        get { return _transform.TransformPoint(_mesh.bounds.center); }
    }

    private Vector3 Max
    {
        get { return _transform.TransformPoint(_mesh.bounds.max); }
    }

    [MenuItem("GameObject/Set Pivot")] //Place the Set Pivot menu item in the GameObject menu
    static void Init()
    {
        _window = GetWindow<SetPivotWindow>();

#if !UNITY_5
        _window.title = "Set Pivot";
#endif
#if UNITY_5
        _window.titleContent = new GUIContent("Set Pivot");
#endif
        _window.Show();
        _window.SelectionChanged();
    }

    void OnEnable()
    {
        SceneView.onSceneGUIDelegate += OnSceneGUI;
        _window = this;
        _window.minSize = new Vector2(300, 240);
        _window.maxSize = new Vector2(300, 240);
    }

    void OnDisable()
    {
        SceneView.onSceneGUIDelegate -= OnSceneGUI;
    }

    void SelectionChanged()
    {
        if (_obj == null)
        {
            _meshFilter = null;
            _mesh = null;
            _collider = null;
            Repaint();
            return;
        }

        _meshFilter = _obj.GetComponent<MeshFilter>();

        _transform = _obj.transform;

        _collider = _obj.GetComponent<Collider>();

        if (_collider)
        {
            Type type = _collider.GetType();

            _colliderCenterProperty = type.GetProperty("center");

            if (_colliderCenterProperty != null)
            {
                _initialColliderCenter = (Vector3)_colliderCenterProperty.GetValue(_collider, null);
                _initialPos = _transform.position;
            }
        }

        if (_meshFilter != null && _setOnScene)
            SceneView.currentDrawingSceneView.FrameSelected();

        SelectMesh();

        Repaint();
    }

    bool CloneAndSaveMesh()
    {
        string path = EditorUtility.SaveFilePanelInProject("Save Cloned Mesh", _meshFilter.sharedMesh.name, "asset", "");

        if (string.IsNullOrEmpty(path))
            return false;

        // clone mesh
        Mesh cloneMesh = Instantiate(_meshFilter.sharedMesh);

        // save mesh asset
        AssetDatabase.CreateAsset(cloneMesh, AssetDatabase.GenerateUniqueAssetPath(path));

        _meshFilter.sharedMesh = cloneMesh;

        MeshCollider meshColl = _transform.GetComponent<MeshCollider>();

        if (meshColl)
        {
            meshColl.sharedMesh = cloneMesh;
        }

        SelectMesh();

        return true;
    }

    void OnGUI()
    {
        if (_meshFilter)
        {
            if (GUILayout.Button(new GUIContent("Clone mesh", "Clones the mesh so you won't lose the original one.")))
            {
                CloneAndSaveMesh();
            }
        }
        if (_mesh)
        {
            bool setOnScene = EditorGUILayout.Toggle(new GUIContent("Use Handles", "Use a gizmo to set the pivot on scene view."), _setOnScene);

            if (!_setOnScene && setOnScene)
            {
                _pivot = _transform.position;
                _lastGizmoPivot = _pivot;
            }

            _setOnScene = setOnScene;

            _snapToVertex = EditorGUILayout.Toggle(new GUIContent("Snap to vertex", "Snap pivot position handle to mesh vertices."), _snapToVertex);

            bool draw = EditorGUILayout.Toggle(new GUIContent("Draw bounding box", "Choose whether to draw bounding box gizmo or not."), _drawBoundingBox);

            if (draw != _drawBoundingBox)
            {
                ((SceneView)SceneView.sceneViews[0]).Repaint();
            }

            _drawBoundingBox = draw;

            EditorGUILayout.BeginHorizontal();

            _trPivot = (Transform)EditorGUILayout.ObjectField(_trPivot, typeof(Transform), true);

            EditorGUI.BeginDisabledGroup(!_trPivot);

            if (GUILayout.Button("Set to transform"))
            {
                _pivot = _trPivot.position;
                UpdatePivot();
            }

            EditorGUI.EndDisabledGroup();

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("X", GUILayout.MaxWidth(20));

            float x = EditorGUILayout.Slider(_x, 0, 1);

            if (!Mathf.Approximately(x, _x))
            {
                _pivot.x = Mathf.Lerp(Min.x, Max.x, x);

                UpdatePivot();
            }

            _x = x;


            if (GUILayout.Button(new GUIContent("Center", "Center pivot on X axis."), GUILayout.MinWidth(75)))
            {
                _x = .5f;
                _pivot.x = Mathf.Lerp(Min.x, Max.x, _x);
                UpdatePivot();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Y", GUILayout.MaxWidth(20));

            float y = EditorGUILayout.Slider(_y, 0, 1);

            if (!Mathf.Approximately(y, _y))
            {
                _pivot.y = Mathf.Lerp(Min.y, Max.y, y);

                UpdatePivot();
            }

            _y = y;

            if (GUILayout.Button(new GUIContent("Center", "Center pivot on Y axis."), GUILayout.MinWidth(75)))
            {
                _y = .5f;
                _pivot.y = Mathf.Lerp(Min.y, Max.y, _y);
                UpdatePivot();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();

            EditorGUILayout.LabelField("Z", GUILayout.MaxWidth(20));

            float z = EditorGUILayout.Slider(_z, 0, 1);

            if (!Mathf.Approximately(z, _z))
            {
                _pivot.z = Mathf.Lerp(Min.z, Max.z, z);

                UpdatePivot();
            }

            _z = z;

            if (GUILayout.Button(new GUIContent("Center", "Center pivot on Z axis."), GUILayout.MinWidth(75)))
            {
                _z = .5f;
                _pivot.z = Mathf.Lerp(Min.z, Max.z, _z);
                UpdatePivot();
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Separator();

            if (GUILayout.Button("Center Pivot"))
            {
                _x = _y = _z = .5f;
                _pivot = _transform.TransformPoint(_mesh.bounds.center);

                UpdatePivot();
            }

            if (GUILayout.Button(new GUIContent("Freeze Rotation", "Freezing the rotation will allow you to change the pivot orientation.")))
            {
                FreezeRotation();
            }

            if (GUILayout.Button(new GUIContent("Freeze scale",
                    "Sets transform scale to one without changing object's look")))
            {
                FreezeScale();
            }
        }
        else
        {
            EditorGUILayout.HelpBox("No Mesh Selected!", MessageType.Error);
        }
    }


    void FreezeRotation()
    {
        if (_mesh)
        {
            Transform[] children = new Transform[_transform.childCount];
            //
            // remove children so they don't move along
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = _transform.GetChild(i);

                children[i].parent = null;
            }

            string path = AssetDatabase.GetAssetPath(_mesh);

            if (!path.EndsWith(".asset") && !string.IsNullOrEmpty(path))
            {
                CloneAndSaveMesh();
            }

            Vector3[] worldVertices = _mesh.vertices.Select(v => _transform.TransformPoint(v)).ToArray();

            _transform.rotation = Quaternion.identity;

            Vector3[] vertices = worldVertices.Select(v => _transform.InverseTransformPoint(v)).ToArray();

            _mesh.vertices = vertices;

            _mesh.RecalculateBounds();

            _lastGizmoPivot = _pivot;

            _mesh.RecalculateNormals();

            // re add children
            for (int i = 0; i < children.Length; i++)
            {
                children[i].parent = _transform;
            }

            EditorSceneManager.MarkAllScenesDirty();
        }
    }

    void FreezeScale()
    {
        if (_mesh)
        {
            Transform[] children = new Transform[_transform.childCount];

            // remove children so they don't move along
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = _transform.GetChild(i);

                children[i].parent = null;
            }

            string path = AssetDatabase.GetAssetPath(_mesh);

            if (!path.EndsWith(".asset") && !string.IsNullOrEmpty(path))
            {
                CloneAndSaveMesh();
            }

            Vector3[] worldVertices = _mesh.vertices.Select(v => _transform.TransformPoint(v)).ToArray();

            _transform.localScale = Vector3.one;

            Vector3[] vertices = worldVertices.Select(v => _transform.InverseTransformPoint(v)).ToArray();

            _mesh.vertices = vertices;

            _mesh.RecalculateBounds();

            _lastGizmoPivot = _pivot;

            _mesh.RecalculateNormals();

            // re add children
            for (int i = 0; i < children.Length; i++)
            {
                children[i].parent = _transform;
            }

            EditorSceneManager.MarkAllScenesDirty();
        }
    }

    private Vector3 _initialPos;

    bool UpdatePivot()
    {
        if (_mesh)
        {
            Vector3[] worldVertices = _mesh.vertices.Select(v => _transform.TransformPoint(v)).ToArray();

            Transform[] children = new Transform[_transform.childCount];

            // remove children so they don't move along
            for (int i = 0; i < children.Length; i++)
            {
                children[i] = _transform.GetChild(i);

                children[i].parent = null;
            }

            string path = AssetDatabase.GetAssetPath(_mesh);

            if (!path.EndsWith(".asset") && !string.IsNullOrEmpty(path))
            {
                bool cloned = CloneAndSaveMesh();

                if (!cloned)
                    return false;
            }

            _transform.position = _pivot;

            Vector3[] vertices = worldVertices.Select(v => _transform.InverseTransformPoint(v)).ToArray();

            _mesh.vertices = vertices;

            _mesh.RecalculateBounds();

            _lastGizmoPivot = _pivot;

            if (_collider)
            {
                if (_collider is MeshCollider)
                {
                    MeshCollider m = _collider as MeshCollider;
                    m.sharedMesh = _mesh;
                    m.convex = !m.convex;
                    m.convex = !m.convex;
                }

                if (_colliderCenterProperty != null)
                {
                    _colliderCenterProperty.SetValue(_collider,
                        _initialColliderCenter + (_initialPos - _transform.position), null);
                }
            }

            // re add children
            for (int i = 0; i < children.Length; i++)
            {
                children[i].parent = _transform;
            }

            EditorSceneManager.MarkAllScenesDirty();

            return true;
        }
        else
        {
            return false;
        }
    }

    void SelectMesh()
    {
        if (_meshFilter)
        {
            _mesh = _meshFilter.sharedMesh;

            _pivot = _transform.position;

            _lastGizmoPivot = _pivot;
        }
        else
        {
            _mesh = null;
        }
    }


    [DrawGizmo(GizmoType.Selected)]
    private static void DrawGizmos(MeshFilter filter, GizmoType gizmoType)
    {
        if (_window == null)
            return;

        if (_window._mesh == null)
            return;

        /* Handles.color = Color.red;
         Handles.FreeMoveHandle(_window._transform.position, _window._transform.rotation, .1f, Vector3.zero,
             Handles.SphereCap);*/

        if (!_drawBoundingBox)
            return;

        if (filter.sharedMesh == _window._mesh)
        {
            Gizmos.color = Color.green;

            Gizmos.matrix = Matrix4x4.TRS(filter.transform.position, filter.transform.rotation, filter.transform.localScale);

            Vector3 center = _window._mesh.bounds.center;

            Vector3 size = _window._mesh.bounds.size;

            Gizmos.DrawWireCube(center, size);
        }
    }

    private Vector3 _lastGizmoPivot;

    void OnSceneGUI(SceneView sceneView)
    {
        GameObject obj = Selection.activeGameObject;

        if (obj != _obj || _transform == null)
        {
            _obj = obj;

            SelectionChanged();

            return;
        }

        if (_mesh == null || _transform == null)
            return;

        if (!_setOnScene)
        {
            if (Tools.current == UnityEditor.Tool.None)
                Tools.current = UnityEditor.Tool.Move;

            return;
        }

        Handles.color = Color.red;

        Handles.FreeMoveHandle(_transform.position, _transform.rotation, .1f, Vector3.zero,
            Handles.SphereCap);

        Tools.current = UnityEditor.Tool.None;

        Vector3 pivot = Handles.DoPositionHandle(_lastGizmoPivot, Quaternion.identity);

        bool movedPivot = pivot != _lastGizmoPivot;

        _lastGizmoPivot = pivot;

        // only change pivot when the current gizmo pivot is different from player position and the user stoped moving the handle
        if (pivot != _transform.position && !movedPivot)
        {
            _lastGizmoPivot = _pivot;

            Vector3[] worldVertices = _mesh.vertices.Select(v => _transform.TransformPoint(v)).ToArray();

            Vector3 snapPivot = ClosestVertex(worldVertices, pivot);

            if (_snapToVertex)
            {
                Vector3 oldPivot = _transform.position;

                if (snapPivot != oldPivot)
                {
                    if (!Mathf.Approximately(_pivot.x, pivot.x))
                    {
                        _pivot.x = snapPivot.x;
                    }
                    if (!Mathf.Approximately(_pivot.y, pivot.y))
                    {
                        _pivot.y = snapPivot.y;
                    }
                    if (!Mathf.Approximately(_pivot.z, pivot.z))
                    {
                        _pivot.z = snapPivot.z;
                    }

                    bool update = UpdatePivot();

                    if (!update)
                        _lastGizmoPivot = _transform.position;
                }
            }
            else
            {
                _pivot = pivot;

                bool update = UpdatePivot();

                if (!update)
                {
                    _lastGizmoPivot = _transform.position;
                }
            }
        }
    }

    private Vector3 ClosestVertex(Vector3[] vertices, Vector3 pivot)
    {
        int closest = 0;

        for (int i = 1; i < vertices.Length; i++)
        {
            if (Vector3.Distance(vertices[i], pivot) < Vector3.Distance(vertices[closest], pivot))
            {
                closest = i;
            }
        }

        return vertices[closest];
    }
}