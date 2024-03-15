using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.Net.Sockets;
using System.Threading;
using UnityEngine;

public class SynchronizeEditorTransforms : MonoBehaviour
{
	const int FRAME_BUFFER_SIZE = 32;
	const int HEAD_DISTANCE_ACCEPTABLE_DRIFT = FRAME_BUFFER_SIZE / 8;
	const int HEAD_DISTANCE_SPEEDUP = FRAME_BUFFER_SIZE / 2 + HEAD_DISTANCE_ACCEPTABLE_DRIFT;
	const int HEAD_DISTANCE_SLOWDOWN = FRAME_BUFFER_SIZE / 2 - HEAD_DISTANCE_ACCEPTABLE_DRIFT;

	const float SPEEDUP_MULTIPLIER = 1.05f;
	const float SLOWDOWN_MULTIPLIER = 0.95f;

	public static SynchronizeEditorTransforms activeInstance;

    const int HEADER_LENGTH = 8;
    const string ADDRESS = "localhost";

    public int port = 10300;
    public byte sendRateHz = 30;

    TcpListener listener;
	TcpClient client;
    NetworkStream networkStream;
    BinaryFormatter formatter;

    List<Transform> transforms;

	bool isConnected = false;
	string status = "";

	FrameData[] frameBuffer = new FrameData[FRAME_BUFFER_SIZE];
	int writeHead = 0;

	int framesReceived;
	bool playbackStarted = false;
	double playbackTime;

	int headDistance;
	float playbackSpeed = 1f;

	Thread pollThread;

	private void Awake() {

		if (activeInstance != null) {
			GameObject.Destroy(this);
			Debug.LogError("More than one SynchronizeEditorTransforms are active at the same time, this is not allowed.");
		}

		activeInstance = this;

        this.transforms = new List<Transform>();

		RegisterTransforms();

		int dataSize = this.transforms.Count * Transformation.SIZE_BYTES + HEADER_LENGTH;
		var dataRateKBsPerSecond = ((float)(sendRateHz * dataSize)) / 1000f;
		Debug.Log($"nTransforms={this.transforms.Count} @ {this.sendRateHz} Hz, Send rate: {dataRateKBsPerSecond:0.00} KB/s");

        this.formatter = new BinaryFormatter();
		Application.runInBackground = true;

		int bufferLength = this.transforms.Count * Transformation.SIZE_BYTES + HEADER_LENGTH;

		for (int i = 0; i < frameBuffer.Length; i++) {
			frameBuffer[i] = new FrameData(this.transforms.Count);
		}
	}

	void RegisterTransforms() {
		var rbs = GetComponentsInChildren<Rigidbody>(true);

		foreach(var rb in rbs) {
			rb.isKinematic = true;
		}

		var renderers = GetComponentsInChildren<Renderer>(true);

		HashSet<Transform> uniqueTransforms = new HashSet<Transform>();
		uniqueTransforms.Add(this.transform);

		foreach(var renderer in renderers) {
			Transform t = renderer.transform;

			while(uniqueTransforms.Add(t)) {
				t = t.parent;
			}
		}

		this.transforms = new List<Transform>(uniqueTransforms);
	}

	private void Start() {
		StartCoroutine(RunListener());
	}

	private void Update() {

		UpdateStatus();

		if(!this.playbackStarted) {
			if(this.framesReceived > FRAME_BUFFER_SIZE / 2) {
				StartPlayback();
			}
			else {
				return;
			}
		}

		RunPlayback();
	}

	void UpdateStatus() {
		if(!this.isConnected) {
			this.status = "Awaiting connection";
		}
		else if(!this.playbackStarted) {
			this.status = $"Connected, awaiting data";
		}
		else {
			this.status = $"Connected, Synchronizing!, headDistance={this.headDistance}";
		}
	}

	void StartPlayback() {
		this.playbackStarted = true;
		SeekPlaybackTime();
	}

	void RunPlayback() {

		this.playbackTime += this.playbackSpeed * Time.deltaTime;
		
		if(!SeekReadHead(out int readHead, out int nextReadHead)) {
			// Read head is outside of window
			Debug.Log("SynchronizeEditorTransform: Read head is outside frame buffer window, playback may stutter.");
			SeekPlaybackTime();
			return;
		}

		this.headDistance = GetHeadDistance(readHead);

		if(this.headDistance > HEAD_DISTANCE_SPEEDUP) {
			this.playbackSpeed = SPEEDUP_MULTIPLIER;
		}
		else if(this.headDistance < HEAD_DISTANCE_SLOWDOWN) {
			this.playbackSpeed = SLOWDOWN_MULTIPLIER;
		}
		else {
			this.playbackSpeed = 1f;
		}

		var prevFrame = this.frameBuffer[readHead];
		var nextFrame = this.frameBuffer[nextReadHead];

		float t = (float) ((this.playbackTime - prevFrame.timestamp) / (nextFrame.timestamp - prevFrame.timestamp));

		for (int i = 0; i < this.transforms.Count; i++) {
			this.transforms[i].localPosition = Vector3.Lerp(prevFrame.transformations[i].GetPosition(), nextFrame.transformations[i].GetPosition(), t);
			this.transforms[i].localRotation = Quaternion.Slerp(prevFrame.transformations[i].GetRotation(), nextFrame.transformations[i].GetRotation(), t);
		}
	}

	void SeekPlaybackTime() {
		double meanTime = 0;
		for (int i = 0; i < FRAME_BUFFER_SIZE; i++) {
			meanTime += this.frameBuffer[i].timestamp;
		}

		meanTime /= FRAME_BUFFER_SIZE;

		this.playbackTime = meanTime;
	}

	bool SeekReadHead(out int readHead, out int nextReadHead) {
		readHead = 0;
		nextReadHead = 1;

		for (readHead = 0; readHead < FRAME_BUFFER_SIZE; readHead++) {

			nextReadHead = (readHead + 1) % FRAME_BUFFER_SIZE;

			if(readHead == this.writeHead ||nextReadHead == this.writeHead) {
				// Don't read from write head as it may be being updated.
			}
			else if (this.playbackTime >= this.frameBuffer[readHead].timestamp && this.playbackTime < this.frameBuffer[nextReadHead].timestamp) {
				return true;
			}
		}

		return false;
	}

	int GetHeadDistance(int readHead) {
		return (this.writeHead - readHead + FRAME_BUFFER_SIZE) % FRAME_BUFFER_SIZE;
	}

	IEnumerator RunListener() {
		this.listener = new TcpListener(this.port);
		this.listener.Start();
		this.framesReceived = 0;
		this.writeHead = 0;
		this.playbackStarted = false;
		this.playbackSpeed = 1f;

		var task = this.listener.AcceptTcpClientAsync();
		while(!task.IsCompleted) {
			yield return 0;
		}

		if (task.Status != System.Threading.Tasks.TaskStatus.RanToCompletion) {
			OnConnectionFailed();
		}
		else {
			this.client = task.Result;

			if(!this.client.Connected) {
				OnConnectionFailed();
			}
			else {
				this.isConnected = true;
				this.networkStream = this.client.GetStream();

				this.networkStream.WriteByte(this.sendRateHz);

				StartPollThread();

				while (this.client.Connected) {
					yield return 0;
				}

				CloseConnection();
			}
		}
	}

	void CloseConnection() {
		this.pollThread.Abort();
		this.networkStream.Close();
		this.listener.Stop();
		this.networkStream.Dispose();
		this.isConnected = false;
	}

	private void OnApplicationQuit() {
		activeInstance = null;
		CloseConnection();
	}

	void StartPollThread() {
		this.pollThread = new Thread(PollClient);
		this.pollThread.Start();
	}

	void PollClient() {

		while(this.isConnected) {
			if(this.networkStream.DataAvailable) {

				this.framesReceived++;

				this.frameBuffer[writeHead].timestamp = (double)this.formatter.Deserialize(this.networkStream);

				for (int i = 0; i < this.transforms.Count; i++) {
					Transformation t = (Transformation)this.formatter.Deserialize(this.networkStream);
					this.frameBuffer[writeHead].transformations[i] = t;
				}

				writeHead = (writeHead + 1) % FRAME_BUFFER_SIZE;
			}
		}
	}

	void OnConnectionFailed() {
		Debug.LogError("SynchronizeEditorTransform: Could not connect to game");
	}

	private void OnGUI() {
		GUI.Label(new Rect(5f, 5f, 500f, 40f), $"Synchronize Editor Transforms: {this.status}");
	}

	[System.Serializable]
	struct Transformation
	{
		public const int SIZE_BYTES = 4 * 7;

		public float posX, posY, posZ;
		public float rotX, rotY, rotZ, rotW;

		public Vector3 GetPosition() {
			return new Vector3(posX, posY, posZ);
		}

		public Quaternion GetRotation() {
			return new Quaternion(rotX, rotY, rotZ, rotW);
		}
	}

	struct FrameData
	{
		public double timestamp;
		public Transformation[] transformations;

		public FrameData(int nTransformations) {
			this.timestamp = default;
			this.transformations = new Transformation[nTransformations];
		}
	}
}
