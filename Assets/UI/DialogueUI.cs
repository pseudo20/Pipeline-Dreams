﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Yarn.Unity;


public class DialogueUI : MonoBehaviour
{
    
    [SerializeField] GameObject DialoguePanel;
    [SerializeField] int FOVDialogue;
    [SerializeField] int FOVNormal;
    [SerializeField] Text TitleText;
    [SerializeField] Text DescriptionText;
    [Range(0,1)][SerializeField] float LerpSpeed;
    PlayerController PC;
    EntityManager EM;
    Camera FrontCam;
    MapManager mManager;
    bool visible = false;
    bool isMoving = true;
    // Start is called before the first frame update
    private void Awake() {
        PC = GameObject.FindGameObjectWithTag("SceneManager").GetComponent<PlayerController>();
        EM = PC.GetComponent<EntityManager>();
        mManager = PC.GetComponent<MapManager>();
        FrontCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving) {
            FrontCam.fieldOfView = Mathf.Lerp(FrontCam.fieldOfView, visible ? FOVDialogue : FOVNormal, LerpSpeed);
            if (Mathf.Abs(FrontCam.fieldOfView - (visible ? FOVDialogue : FOVNormal)) < 0.1) {
                FrontCam.fieldOfView = visible ? FOVDialogue : FOVNormal;
                isMoving = false;
            }
        } 

        

    }

    private void ShowTileDialogue(TileData data) {
        TitleText.text = data.Name;
        if (data.HasDialogue)
            FindObjectOfType<DialogueRunner>().StartDialogue(data.Name);
        else {
            DescriptionText.text = "Nothing special with this " + data.Name + ".";
        }

    }

    private void ShowEntityDialogue(EntityData data) {
        TitleText.text = data.Name;
        if (data.HasDialogue)
            FindObjectOfType<DialogueRunner>().StartDialogue(data.Name);
        else {
            DescriptionText.text = "This " + data.Name + " does not seem to want to talk with me....";
        }
        
    }
    public void HideDialogue() {
        isMoving = true;
        visible = false;
        PC.EnableInput(PlayerInputFlag.UIPANEL);
        FindObjectOfType<DialogueRunner>()?.Stop();
        DialoguePanel.SetActive(false);
    }
    public void ShowDialogue() {
        isMoving = true;
        visible = true;
        PC.DisableInput(PlayerInputFlag.UIPANEL);
        

        
        
        DialoguePanel.SetActive(visible);
        
            var e = EM.FindEntityInLine(Util.LHQToFace(EM.Player.IdealRotation), EM.Player);
            if (e != null)
                ShowEntityDialogue(e.Data);
            else {
                var t = mManager.GetTileRelative(Vector3Int.zero, Util.LHQToFace(EM.Player.IdealRotation), EM.Player);
                ShowTileDialogue(mManager.Dataset.Dataset.Find((x) => x.Type == t));
            }
        
    }  
}
