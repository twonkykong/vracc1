using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using UnityEngine.UI;
using System;
using UnityEngine.Video;
//using YoutubePlayer;

public class OnlineController : MonoBehaviourPunCallbacks
{
    public SpawnerOnline spawner;
    public WeldingOnline welding;

    public Text chatText;

    //HOLDER
    [PunRPC]
    public void Hold(int playerViewID, int propViewID, int holdPointViewID, int linerViewID)
    {
        GameObject holdPoint = PhotonView.Find(holdPointViewID).gameObject;
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        GameObject liner = PhotonView.Find(linerViewID).gameObject;
        PhotonView player = PhotonView.Find(playerViewID);
        prop.GetPhotonView().TransferOwnership(player.Owner);
        prop.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        prop.GetComponent<Rigidbody>().isKinematic = false;

        if (prop.GetComponent<Outline>() != null)
        {
            prop.GetComponent<Outline>().enabled = true;
            Debug.Log(prop.GetComponent<Outline>().name);
            Debug.Log("yes");
        }
        else
        {
            GameObject g = prop.transform.parent.gameObject;
            while (g.transform.parent != null) g = g.transform.parent.gameObject;

            g.GetComponentInChildren<Outline>().enabled = true;
            Debug.Log(g.GetComponentInChildren<Outline>().name);
            Debug.Log("no");
        }

        holdPoint.GetComponent<FixedJoint>().connectedBody = prop.GetComponent<Rigidbody>();

        prop.GetComponent<Prop>().holded = true;
        liner.GetComponent<DrawingLine>().enabled = true;
    }

    [PunRPC]
    public void Drop(int propViewID, int holdPointViewID, int linerViewID)
    {
        GameObject holdPoint = PhotonView.Find(holdPointViewID).gameObject;
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        GameObject liner = PhotonView.Find(linerViewID).gameObject;
        holdPoint.GetComponent<FixedJoint>().connectedBody = null;

        prop.GetComponent<Prop>().holded = false;

        if (prop.GetComponent<Outline>() != null) prop.GetComponent<Outline>().enabled = false;
        else
        {
            GameObject g = prop.transform.parent.gameObject;
            while (g.transform.parent != null) g = g.transform.parent.gameObject;

            g.GetComponentInChildren<Outline>().enabled = false;
        }

        if (prop.GetComponent<Prop>().isKinematic == true) prop.GetComponent<Rigidbody>().isKinematic = true;
        liner.GetComponent<DrawingLine>().enabled = false;
    }

    [PunRPC]
    public void Fix(int propViewID)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        prop.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;

        if (prop.GetComponent<Outline>() != null) prop.GetComponent<Outline>().enabled = false;
        else
        {
            GameObject g = prop.transform.parent.gameObject;
            while (g.transform.parent != null) g = g.transform.parent.gameObject;

            g.GetComponentInChildren<Outline>().enabled = false;
        }
    }

    [PunRPC]
    public void ChangeAnchor(int camViewID, int propViewID, int holdPointViewID, int multiplier)
    {
        GameObject cam = PhotonView.Find(camViewID).gameObject;
        GameObject holdPoint = PhotonView.Find(holdPointViewID).gameObject;
        GameObject prop = PhotonView.Find(propViewID).gameObject;

        prop.transform.position += (cam.transform.forward * 0.1f) * multiplier;
        holdPoint.GetComponent<FixedJoint>().connectedAnchor = prop.transform.position;
    }

    //WIRING
    [PunRPC]
    public void Wire(int prop1ViewID, int prop2ViewID)
    {
        GameObject prop1 = PhotonView.Find(prop1ViewID).gameObject;
        Debug.Log(prop2ViewID);
        GameObject prop2 = PhotonView.Find(prop2ViewID).gameObject;

        if (!(prop1.GetComponent<PowerGenerator>() && prop2.GetComponent<PowerGenerator>()) && !(prop1.GetComponent<Technology>() && prop2.GetComponent<Technology>()))
        {
            if (prop1.GetComponent<PowerGenerator>() != null)
            {
                prop1.GetComponent<PowerGenerator>().EnableConnection(prop2);
            }

            else
            {
                prop2.GetComponent<PowerGenerator>().EnableConnection(prop1);
            }
        }
    }

    [PunRPC]
    public void RemoveWire(int propViewID)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;

        if (prop.GetComponent<Technology>() != null && prop.GetComponent<Technology>().generator != null)
        {
            prop.GetComponent<Technology>().generator.GetComponent<PowerGenerator>().connectedTechs.Remove(prop);
            prop.GetComponent<Technology>().generator = null;
            prop.GetComponent<Technology>().Activate(false);
        }

        else if (prop.GetComponent<PowerGenerator>() != null && prop.GetComponent<PowerGenerator>().connectedTechs.Count != 0)
        {
            prop.GetComponent<PowerGenerator>().connectedTechs[prop.GetComponent<PowerGenerator>().connectedTechs.Count - 1].GetComponent<Technology>().generator = null;
            prop.GetComponent<PowerGenerator>().connectedTechs[prop.GetComponent<PowerGenerator>().connectedTechs.Count - 1].GetComponent<Technology>().Activate(false);

            prop.GetComponent<PowerGenerator>().connectedTechs.RemoveAt(prop.GetComponent<PowerGenerator>().connectedTechs.Count - 1);
        }
    }

    //WELDING
    [PunRPC]
    public void Weld(int prop1ViewID, string jointType, int prop2ViewID = 0, int gViewID = 0, int g1ViewID = 0)
    {
        GameObject prop1 = PhotonView.Find(prop1ViewID).gameObject;

        if (jointType == "remove")
        {
            if (prop1.GetComponent<Joint>() != null)
            {
                foreach (Joint conJoint in prop1.GetComponents<Joint>())
                {
                    if (prop1.GetComponent<Prop>().joint == null || conJoint != prop1.GetComponent<Prop>().joint)
                    {
                        Destroy(prop1.GetComponent<Joint>());
                        Destroy(prop1.GetComponentInChildren<LineRenderer>().gameObject);

                        break;
                    }
                }
            }
            else
            {
                if (prop1.GetComponent<Prop>().connectedJoints.Count > 0)
                {
                    prop1.GetComponent<Prop>().connectedJoints.RemoveAt(0);
                    Destroy(prop1.GetComponent<Prop>().connectedJoints[0]);
                }
            }
            return;
        }

        GameObject prop2 = PhotonView.Find(prop2ViewID).gameObject;
        GameObject g = PhotonView.Find(gViewID).gameObject;
        GameObject g1 = PhotonView.Find(g1ViewID).gameObject;

        Joint joint = null;
        if (jointType == "fixed")
        {
            joint = prop1.AddComponent<FixedJoint>();
        }
        else if (jointType == "spring")
        {
            joint = prop1.AddComponent<SpringJoint>();
        }
        else if (jointType == "hinge")
        {
            joint = prop1.AddComponent<HingeJoint>();
        }

        if (joint != null)
        {
            joint.connectedBody = prop2.GetComponent<Rigidbody>();
            prop2.GetComponent<Rigidbody>().mass += prop1.GetComponent<Rigidbody>().mass;

            prop2.GetComponent<Prop>().connectedJoints.Add(joint);
        }

        g.name = "new line";
        g.transform.parent = prop1.transform;
        g.AddComponent<LineRenderer>().startWidth = 0.2f;
        g.GetComponent<Renderer>().material = welding.weldMat;

        int count = prop1.GetComponents<Joint>().Length;
        g1.name = "weldingLine " + prop1.name + " " + (count - 1);

        g1.transform.parent = prop2.transform;
    }

    //USEFUL STUFF
    [PunRPC]
    public void Sit(int chairViewID, bool value)
    {
        GameObject chair = PhotonView.Find(chairViewID).gameObject;
        chair.GetComponent<VehicleChair>().sitting = value;
    }

    //TECH INTERACTION
    [PunRPC]
    public void Interact(int propViewID)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;

        if (prop.GetComponent<Technology>() != null && prop.GetComponent<Technology>().needGenerator == false)
        {
            prop.GetComponent<Technology>().Activate(!prop.GetComponent<Technology>().isWorking);
        }

        else if (prop.GetComponent<GeneratorAdditionals>() != null) prop.GetComponent<GeneratorAdditionals>().Action();
    }

    //CHANGE COLOR
    [PunRPC]
    public void ChangeColor(int propViewID, float r, float g, float b, float a)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;

        if (prop.tag == "text")
        {
            prop.GetComponent<TextMeshPro>().color = new Color(r / 255, g / 255, b / 255, a / 255);
        }
        else
        {
            while (prop.GetComponent<Renderer>() == null && prop.transform.parent != null) prop = prop.transform.parent.gameObject;
            Material mat = new Material(prop.GetComponent<Renderer>().material);
            mat.color = new Color(r / 255, g / 255, b / 255, a / 255);
            prop.GetComponent<Renderer>().material = mat;
        }
    }

    //CHANGE WEIGHT
    [PunRPC]
    public void ChangeWeight(int propViewID, float value, bool increase)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        if (increase) prop.GetComponent<Rigidbody>().mass *= value;
        else prop.GetComponent<Rigidbody>().mass /= value;
    }

    [PunRPC]
    public void SwitchGravity(int propViewID, bool value)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        prop.GetComponent<Rigidbody>().useGravity = value;
    }

    //CHANGE SIZE
    [PunRPC]
    public void ChangeSize(int propViewID, float x, float y, float z, bool increase, bool wholeObject)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        if (wholeObject)
        {
            while (prop.transform.parent != null && prop.transform.GetComponent<Rigidbody>() != null) prop = prop.transform.parent.gameObject;
        }

        if (increase) prop.transform.localScale = new Vector3(prop.transform.localScale.x * x, prop.transform.localScale.y * y, prop.transform.localScale.z * z);
        else prop.transform.localScale = new Vector3(prop.transform.localScale.x / x, prop.transform.localScale.y / y, prop.transform.localScale.z / z);
    }

    //CHANGE TEXT
    [PunRPC]
    public void ChangeText(int propViewID, string text, string fontName)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        TMP_FontAsset font = (TMP_FontAsset)Resources.Load(fontName);

        prop.GetComponent<TextMeshPro>().text = text;
        prop.GetComponent<TextMeshPro>().font = font;
    }

    //YOUTUBE TV
    [PunRPC]
    public async void ChangeVideo(int propViewID, string videoUrl)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        var youtubePlayer = prop.GetComponentInChildren<YoutubeSimplified>();
        youtubePlayer.GetComponentInChildren<VideoPlayer>().Stop();
        youtubePlayer.url = videoUrl;
        youtubePlayer.Play();

        prop.GetComponentInChildren<Animation>().Stop();
        prop.GetComponentInChildren<Animation>().Play("play");
    }

    [PunRPC]
    public void ChangeVolume(int propViewID, float volume)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        var youtubePlayer = prop.GetComponentInChildren<YoutubeSimplified>();
        youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).mute = false;
        youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).volume += volume;

        prop.GetComponentInChildren<Animation>().Stop();
        prop.GetComponentInChildren<Animation>().Play("changeVolume");

        prop.GetComponentInChildren<TextMeshPro>().text = youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).volume * 100 + "%";

    }

    [PunRPC]
    public void PauseVideo(int propViewID, bool value)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        var youtubePlayer = prop.GetComponentInChildren<YoutubeSimplified>();
        if (value) youtubePlayer.GetComponentInChildren<VideoPlayer>().Pause();
        else youtubePlayer.GetComponentInChildren<VideoPlayer>().Play();

        prop.GetComponentInChildren<Animation>().Stop();
        if (value) prop.GetComponentInChildren<Animation>().Play("pause");
        else prop.GetComponentInChildren<Animation>().Play("play");
    }

    [PunRPC]
    public void RewindVideo(int propViewID, float sec)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        var youtubePlayer = prop.GetComponentInChildren<YoutubeSimplified>();
        youtubePlayer.GetComponentInChildren<VideoPlayer>().time += sec;

        prop.GetComponentInChildren<Animation>().Stop();
        if (sec > 0) prop.GetComponentInChildren<Animation>().Play("rewind+");
        else prop.GetComponentInChildren<Animation>().Play("rewind-");
    }

    [PunRPC]
    public void OffVolume(int propViewID)
    {
        GameObject prop = PhotonView.Find(propViewID).gameObject;
        var youtubePlayer = prop.GetComponentInChildren<YoutubeSimplified>();
        youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).mute = !youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).mute;

        prop.GetComponentInChildren<Animation>().Stop();
        if (youtubePlayer.GetComponentInChildren<VideoPlayer>().GetTargetAudioSource(0).mute == true) prop.GetComponentInChildren<Animation>().Play("off volume");
        else prop.GetComponentInChildren<Animation>().Play("on volume");
    }
}
