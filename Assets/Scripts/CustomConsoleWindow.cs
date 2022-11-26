using UnityEngine;

namespace UNO.General
{
    public class CustomConsoleWindow : MonoBehaviour
    {
        [SerializeField] private Transform logParent;
        [SerializeField] private Transform logObject;
    
        void Awake()
        {
            DontDestroyOnLoad(gameObject);
            Application.logMessageReceived += HijackLogMessage;
        }

        private void HijackLogMessage(string condition, string stacktrace, LogType type)
        {
            LogObject log = Instantiate(logObject, logParent).GetComponent<LogObject>();

            switch (type)
            {
                case LogType.Log:
                    log.SetColour(Color.green);
                    break;
                case LogType.Warning:
                    log.SetColour(Color.yellow);
                    break;
                case LogType.Exception:
                    log.SetColour(Color.red);
                    break;
                case LogType.Error:
                    log.SetColour(Color.red);
                    break;
                case LogType.Assert:
                    log.SetColour(Color.red);
                    break;
            }
            
            log.SetText(condition);
        }

        private void OnApplicationQuit()
        {
            Application.logMessageReceived -= HijackLogMessage;
        }
    }   
}