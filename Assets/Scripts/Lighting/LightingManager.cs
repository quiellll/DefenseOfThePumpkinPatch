using Unity.VisualScripting;
using UnityEngine;

//https://gist.github.com/Glynn-Taylor/08da28896147faa6ba8f9654057d38e6
public class LightingManager : MonoBehaviour
{
    //Scene References
    [SerializeField] private Light DirectionalLight;
    [SerializeField] private LightingPreset Preset;
    //Variables
    //[SerializeField][Range(0f,1f)]private float TimeOfDay;

    [SerializeField][Range(0f, 1f)] private float _initialTimePercent;
    [SerializeField] float _cycleSpeed;

    private bool _advanceDay;

    private float _timePercent = 0.25f;

    private float _targetTime;

    private Vector3 _initialEulers;

    private void Awake()
    {
        _timePercent = _initialTimePercent;
        _initialEulers = transform.localRotation.eulerAngles;

        UpdateLighting(_timePercent);
    }

    private void Start() //se ejecuta de ultimo para no recibir el primer evento de buildmode
    {
        GameManager.Instance.StartBuildMode.AddListener(AdvanceDay);
    }

    private void OnDestroy()
    {
        GameManager.Instance?.StartBuildMode.RemoveListener(AdvanceDay);
    }

    private void Update()
    {
        if (!_advanceDay) return;

        _timePercent += _cycleSpeed * Time.deltaTime;

        if (_timePercent < _targetTime) UpdateLighting(_timePercent % 1);
        else
        {
            _timePercent = _targetTime;
            _advanceDay = false;
        }
    }


    private void UpdateLighting(float t)
    {
        //Set ambient
        RenderSettings.ambientLight = Preset.AmbientColor.Evaluate(t);

        //If the directional light is set then rotate and set it's color,
        //I actually rarely use the rotation because it casts tall shadows unless you clamp the value
        if (DirectionalLight != null)
        {
            DirectionalLight.color = Preset.DirectionalColor.Evaluate(t);

            DirectionalLight.transform.localRotation = Quaternion.Euler(new Vector3((t * 360f) - 90, -65, 0));

        }

    }

    public void AdvanceDay()
    {
        if (_advanceDay) return;

        _advanceDay = true;
        _targetTime = _timePercent + 0.5f;
    }

}