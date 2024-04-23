using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    [Tooltip("�г��� �Է� �ʵ�")]
    [SerializeField] TMP_InputField nameInputField;
    [Tooltip("�� ���� ��ư")]
    [SerializeField] Button connectButton;
    [Tooltip("��Ʈ��ũ ���� �ؽ�Ʈ")]
    [SerializeField] TMP_Text statusText;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();//���� ������ ����(�ܺ� �⺻���� ���)

        connectButton.onClick.AddListener(connectToRoom);//�� ���� ��ư�� �̺�Ʈ �߰�
    }

    /// <summary>
    /// �г��� Ȯ�� �� �� ����
    /// </summary>
    public void connectToRoom()
    {
        if (string.IsNullOrEmpty(nameInputField.text))//�г����� ����ִ� ���
        {
            Debug.Log("�г��� �������.");
            return;
        }

        PhotonNetwork.LocalPlayer.NickName = nameInputField.text;
        PhotonNetwork.JoinRandomRoom();//���� �� ���� �õ�
    }

    /// <summary>
    /// ���� �� ���� ���н� ȣ��Ǵ� �Լ�
    /// </summary>
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join room. Creating a room...");

        PhotonNetwork.CreateRoom("Room", new Photon.Realtime.RoomOptions { MaxPlayers = 10 });//�� ����(���� �����ϸ鼭 ���ε� ����)
    }

    /// <summary>
    /// �� ���� ������ ȣ��Ǵ� �Լ�
    /// </summary>
    public override void OnJoinedRoom()
    {
        Debug.Log("Joined Room.");

        PhotonNetwork.IsMessageQueueRunning = true;//�޽��� ť ���� Ȱ��ȭ
        PhotonNetwork.LoadLevel("Chat");//ä�� ������ �̵�(����ȭ�� ���� LoadLevel ���)
    }

    void Update()
    {
        statusText.text = PhotonNetwork.NetworkClientState.ToString();//��Ʈ��ũ ���� ǥ��

        if (!PhotonNetwork.IsConnected)
            return;


        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            connectToRoom();
    }
}
