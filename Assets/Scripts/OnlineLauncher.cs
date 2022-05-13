using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class OnlineLauncher : MonoBehaviourPunCallbacks
{
    [SerializeField] private TMP_InputField userName;
    [SerializeField] private TMP_InputField joinRoomName;
    [SerializeField] private TMP_InputField createRoomName;

    [SerializeField] private GameObject loadingMenu;
    [SerializeField] private GameObject joinCreateRoomMenu;
    [SerializeField] private TMP_Text loadingMessage;

    private bool levelLoaded = false;

    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Debug.Log("Connecting to Master");
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PhotonNetwork.IsConnectedAndReady && !PhotonNetwork.InRoom)
        {
            loadingMenu.SetActive(false);
            joinCreateRoomMenu.SetActive(true);
            userName.text = PlayerPrefs.GetString("userName");
        }

        if (PhotonNetwork.InRoom && PhotonNetwork.CurrentRoom.PlayerCount == 2 && !levelLoaded)
        {
            levelLoaded = true;
            PhotonNetwork.LoadLevel("Online Players");
        }
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("Connected to Master");
        PhotonNetwork.JoinLobby();
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        loadingMenu.SetActive(false);
        joinCreateRoomMenu.SetActive(true);
    }

    public void SetUserName()
    {
        PlayerPrefs playerPrefs = new PlayerPrefs();
        PlayerPrefs.SetString("userName", userName.text);
        PhotonNetwork.NickName = PlayerPrefs.GetString("userName");
    }

    public void CreateRoom()
    {
        if (string.IsNullOrEmpty(createRoomName.text))
        {
            Debug.Log("Enter room name...");
            return;
        }
        if(string.IsNullOrEmpty(userName.text))
        {
            Debug.Log("Enter user name...");
            return;
        }

        PhotonNetwork.CreateRoom(createRoomName.text, new RoomOptions() {MaxPlayers=2});

        loadingMenu.SetActive(true);
        joinCreateRoomMenu.SetActive(false);
        loadingMessage.text = "Please wait, joining room...";
    }

    public void JoinRoom()
    {
        if(string.IsNullOrEmpty(joinRoomName.text))
        {
            Debug.Log("Enter room name...");
            return;
        }
        if (string.IsNullOrEmpty(userName.text))
        {
            Debug.Log("Enter user name...");
            return;
        }

        PhotonNetwork.JoinRoom(joinRoomName.text);

        loadingMenu.SetActive(true);
        joinCreateRoomMenu.SetActive(false);
        loadingMessage.text = "Please wait, joining room...";
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room");

        if (PhotonNetwork.CountOfPlayers == 1)
            loadingMessage.text = "Waiting for opponent to join...";
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Create Room Failed: " + message);
    }

    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Join Room Failed: " + message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        if(roomList.Count == 2)
        {
            PhotonNetwork.LoadLevel("Online Players");
        }
    }
}
