using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviourPunCallbacks
{
    public GameObject loading, menu;
    public InputField nicknameField, createRoomField, joinRoomField;

    public Slider fov, sens, createMaxPlayers;
    public Slider[] bodyColor, tshirtColor, pantsColor;
    public Text fovText, sensText;
    public Image bodyColorPrev;
    public Toggle useTShirt, usePants;
    public Dropdown onlineMap, singlePlayerMap;

    public TextMeshPro onlineAddInfo, nicknameText;
    public SkinnedMeshRenderer playerMesh, tshirtMesh, pantsMesh;
    public Animation onlineAddInfoAnim;
    public float horizontalFoV = 90.0f;
    public Camera camera;

    private void Start()
    {
        string color = PlayerPrefs.GetString("bodyColor");
        Debug.Log(color);
        bodyColor[0].value = System.Convert.ToInt32(color.Split('/')[0]);
        bodyColor[1].value = System.Convert.ToInt32(color.Split('/')[1]);
        bodyColor[2].value = System.Convert.ToInt32(color.Split('/')[2]);

        Material mat = new Material(playerMesh.material);
        mat.color = new Color(bodyColor[0].value / 255, bodyColor[1].value / 255, bodyColor[2].value / 255);
        playerMesh.material = mat;

        if (PlayerPrefs.GetInt("sensitivity") < 1) sens.value = 10;
        else sens.value = PlayerPrefs.GetInt("sensitivity");
        if (PlayerPrefs.GetInt("fov") < 1) fov.value = 60;
        else fov.value = PlayerPrefs.GetInt("fov");

        nicknameText.gameObject.transform.LookAt(transform.position);
    }

    private void Update()
    {
        fovText.text = "" + fov.value;
        sensText.text = "" + sens.value;

        if (PhotonNetwork.IsConnected)
        {
            onlineAddInfo.text = "region: " + PhotonNetwork.CloudRegion + "\nplayers online: " + PhotonNetwork.CountOfPlayers + "\nrooms: " + PhotonNetwork.CountOfRooms + "\n\n\nfune pictur:";
        }

        float halfWidth = Mathf.Tan(0.5f * horizontalFoV * Mathf.Deg2Rad);

        float halfHeight = halfWidth * Screen.height / Screen.width;

        float verticalFoV = 2.0f * Mathf.Atan(halfHeight) * Mathf.Rad2Deg;

        camera.fieldOfView = verticalFoV;
    }

    public void Connect()
    {
        if (!PhotonNetwork.IsConnected)
        {
            PhotonNetwork.GameVersion = "vracc1";
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NickName = "Player " + Random.Range(1, 101);
            PhotonNetwork.ConnectUsingSettings();
        }
    }

    public override void OnConnectedToMaster()
    {
        loading.SetActive(false);
        menu.SetActive(true);

        onlineAddInfoAnim.Stop();
        onlineAddInfoAnim.Play("onlineaddinfo");
        nicknameText.gameObject.SetActive(true);
        nicknameText.text = PhotonNetwork.NickName;
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        loading.SetActive(true);
        menu.SetActive(false);

        onlineAddInfoAnim.Stop();
        onlineAddInfoAnim.Play("onlineaddinfo=pff");
    }

    public void CreateRoom()
    {
        string roomName = "Room " + PhotonNetwork.CountOfRooms;
        if (createRoomField.text != "") roomName = createRoomField.text;

        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = System.Convert.ToByte(createMaxPlayers.value)});
        Application.LoadLevel(onlineMap.options[onlineMap.value].text);
    }

    public void JoinRoom()
    {
        if (joinRoomField.text == "") PhotonNetwork.JoinRandomRoom();
        else
        {
            PhotonNetwork.JoinRoom(joinRoomField.text);
        }
    }

    public void SinglePlayer()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.OfflineMode = true;
        string roomName = "Room " + PhotonNetwork.CountOfRooms;
        if (createRoomField.text != "") roomName = createRoomField.text;

        PhotonNetwork.CreateRoom(roomName, new RoomOptions() { MaxPlayers = System.Convert.ToByte(createMaxPlayers.value) });
        Application.LoadLevel(onlineMap.options[onlineMap.value].text);
    }

    public void ChangeNickname()
    {
        nicknameText.text = PhotonNetwork.NickName = nicknameField.text;
        nicknameField.text = "";
    }

    public void ChangeSensitivity()
    {
        PlayerPrefs.SetInt("sensitivity", System.Convert.ToInt32(sens.value));
    }
    
    public void ChangeFov()
    {
        PlayerPrefs.SetInt("fov", System.Convert.ToInt32(fov.value));
    }

    public void ToggleClick()
    {
        tshirtMesh.gameObject.SetActive(!useTShirt.isOn);
        useTShirt.isOn = !useTShirt.isOn;

        if (useTShirt.isOn) PlayerPrefs.SetString("tshirtColor", tshirtColor[0].value + "/" + tshirtColor[1].value + "/" + tshirtColor[2].value);
        else PlayerPrefs.SetString("tshirtColor", "none");

        pantsMesh.gameObject.SetActive(!usePants.isOn);
        usePants.isOn = !usePants.isOn;

        if (usePants.isOn) PlayerPrefs.SetString("pantsColor", pantsColor[0].value + "/" + pantsColor[1].value + "/" + pantsColor[2].value);
        else PlayerPrefs.SetString("pantsColor", "none");
    }

    public void ChangeBodyColor()
    {
        bodyColorPrev.color = new Color(bodyColor[0].value / 255, bodyColor[1].value / 255, bodyColor[2].value / 255);
        Material mat = new Material(playerMesh.material);
        mat.color = new Color(bodyColor[0].value / 255, bodyColor[1].value / 255, bodyColor[2].value / 255);
        playerMesh.material = mat;

        PlayerPrefs.SetString("bodyColor", bodyColor[0].value + "/" + bodyColor[1].value + "/" + bodyColor[2].value);
    }

    public void ChangeTShirtColor()
    {
        Material mat = new Material(tshirtMesh.material);
        mat.color = new Color(tshirtColor[0].value / 255, tshirtColor[1].value / 255, tshirtColor[2].value / 255);
        tshirtMesh.material = mat;

        PlayerPrefs.SetString("tshirtColor", tshirtColor[0].value + "/" + tshirtColor[1].value + "/" + tshirtColor[2].value);
    }

    public void ChangePantsColor()
    {
        Material mat = new Material(pantsMesh.material);
        mat.color = new Color(pantsColor[0].value / 255, pantsColor[1].value / 255, pantsColor[2].value / 255);
        pantsMesh.material = mat;

        PlayerPrefs.SetString("pantsColor", pantsColor[0].value + "/" + pantsColor[1].value + "/" + pantsColor[2].value);
    }
}
