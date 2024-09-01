using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using TMPro;
using UnityEngine;

public class AirplaneController : MonoBehaviour
{
    public int serverPort = 1234;
    public TextMeshProUGUI textM;
    private TcpListener server;
    private Thread serverThread;

    private Vector3 airplanePosition = Vector3.zero;
    private Quaternion airplaneRotation = Quaternion.identity;
    private string textMessageString;

    private Vector3 velocity = Vector3.zero;
    private Vector3 angularVelocity = Vector3.zero;
    public Vector3 impact = Vector3.one;

    private Vector3 acceleration = Vector3.zero;
    public bool useAccelerationForRotation = false;
    public bool steer = true;

    public Vector3 referenceGravity = new Vector3(10, 0.3f, 0.3f);
    public Vector3 offset = new Vector3(10, 0.3f, 0.3f);
    public Vector3 accelCoeffs = new Vector3(0, -1, -1);


    void Start()
    {
        StartServer();
    }

    void Update()
    {
        if (useAccelerationForRotation)
        {

            Vector3 normalizedRefGravity = referenceGravity.normalized;
            Vector3 normalizedAcceleration = acceleration.normalized;

            Quaternion targetRotation = Quaternion.FromToRotation(normalizedAcceleration, normalizedRefGravity);
            Quaternion clockwise90Rotation = Quaternion.Euler(offset);
            targetRotation *= clockwise90Rotation;

            airplaneRotation = Quaternion.Slerp(airplaneRotation, targetRotation, Time.deltaTime * 2f);
            
            Vector3 steerVector = Vector3.Scale(angularVelocity, impact);
            if (steer) {
            steerVector = new Vector3(0,-steerVector.y,0);
            Quaternion deltaRotation = Quaternion.Euler(steerVector * Time.deltaTime);
            airplaneRotation *= deltaRotation;
            }
        }
        else
        {
            Quaternion deltaRotation = Quaternion.Euler(Vector3.Scale(angularVelocity, impact) * Time.deltaTime);
            airplaneRotation *= deltaRotation;
        }
        transform.rotation = airplaneRotation;
        textM.text = textMessageString;
    }

    void StartServer()
    {
        serverThread = new Thread(() =>
        {
            try
            {
                server = new TcpListener(IPAddress.Any, serverPort);
                server.Start();
                Debug.Log($"Server started on port {serverPort}");

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    Debug.Log("Client connected");
                    NetworkStream stream = client.GetStream();

                    while (client.Connected)
                    {
                        byte[] data = new byte[1024];
                        int bytesRead = stream.Read(data, 0, data.Length);
                        if (bytesRead > 0)
                        {
                            string receivedData = Encoding.UTF8.GetString(data, 0, bytesRead).Trim();
                            string[] dataForProccessArray = receivedData.Split('\n'); 
                            foreach(string dataForProcess in dataForProccessArray) {
                                try{
                                    ProcessData(dataForProcess);
                                }
                                catch (Exception e)
                                {
                                    Debug.LogWarning("Failed to process data: " + e.Message + " dataForProcess " + dataForProcess);
                                }
                            }
                        }
                        else
                        {
                            Debug.Log("No data received. Closing connection.");
                            break;
                        }
                    }

                    client.Close();
                    Debug.Log("Client disconnected");
                }
            }
            catch (Exception e)
            {
                Debug.LogError("Server error: " + e.Message);
            }
        });
        serverThread.IsBackground = true;
        serverThread.Start();
    }

    void ProcessData(string data)
    {
        string[] parts = data.Split('|');

        string accelerationData = parts[0].Trim();
        string gyroData = parts[1].Trim();

        string[] accelValues = accelerationData.Split(',');
        float accelX = float.Parse(accelValues[0].Split(':')[1]);
        float accelY = float.Parse(accelValues[1].Split(':')[1]);
        float accelZ = float.Parse(accelValues[2].Split(':')[1]);

        acceleration = new Vector3(accelCoeffs.x * accelY,accelCoeffs.y *  accelX,accelCoeffs.z *  accelZ);
        Debug.Log(acceleration);
        velocity = new Vector3(accelY, accelZ, accelX);

        string[] gyroValues = gyroData.Split(',');

        float gyroX = Mathf.Round(float.Parse(gyroValues[0].Split(':')[1]) * 10f) / 10f;
        float gyroY = Mathf.Round(float.Parse(gyroValues[1].Split(':')[1]) * 10f) / 10f;
        float gyroZ = Mathf.Round(float.Parse(gyroValues[2].Split(':')[1]) * 10f) / 10f;

        angularVelocity = new Vector3(gyroY, gyroX, gyroZ);

        StringBuilder logMessage = new StringBuilder("Effective Values : Gyro : ");
        logMessage.Append(gyroX).Append(" , ").Append(gyroY).Append(" , ").Append(gyroZ).Append(" Accel: ").Append(accelX).Append(" , ").Append(accelY).Append(" , ").Append(accelZ);
        Debug.Log(logMessage.ToString());

        StringBuilder textMessage = new StringBuilder("Gyro : ");
        textMessage.Append(gyroX).Append(" , ").Append(gyroY).Append(" , ").Append(gyroZ);
        textMessageString = textMessage.ToString();
    }

    void OnApplicationQuit()
    {
        StopServer();
    }

    void StopServer()
    {
        if (server != null)
        {
            server.Stop();
        }
        if (serverThread != null)
        {
            serverThread.Abort();
        }
    }
}
