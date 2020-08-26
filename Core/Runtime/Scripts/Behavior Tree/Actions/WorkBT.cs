﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Lord.Core {

    public class WorkBT : MoveBT {
        public GameObject workObject;
        public Vector3 workPosition;
        public WorkBT(Character character, GameObject workObject, Vector3 workPosition):
            base(character, workPosition) {
                this.workObject = workObject;
                this.workPosition = workPosition;
            }

        private NodeStates WorkResourceSource() {
            if (workObject != null) {
                ResourceSource _resourceSource = workObject.GetComponent<ResourceSource>();
                _resourceSource.AssignCharacter(character);
                return NodeStates.RUNNING;
            } else {
                this.character.animator.Rebind();
                return NodeStates.SUCCESS;
            }
        }
        public TaskNode WorkResourceSourceNode() {
            return new TaskNode(WorkResourceSource, "Work Resource Source");
        }
        private NodeStates WorkResourceSourceAnim() {
            this.character.animator.Play("Attack", 0);
            return NodeStates.SUCCESS;
        }
        public TaskNode WorkResourceSourceAnimNode() {
            return new TaskNode(WorkResourceSourceAnim, "Animation");
        }
        // private NodeStates WorkResourceSourceFinish() {
        //     if (workObject)
        //         this.character.animator.Rebind();
        //     return NodeStates.SUCCESS;
        // }
        // public TaskNode WorkResourceSourceFinishNode() {
        //     return new TaskNode(WorkResourceSourceFinishNode, "Work Finsished");
        // }
        public Sequence WorkResourceSourceSequence() {
            ResourceSource _resourceSource = workObject.GetComponent<ResourceSource>();
            WOD_ResourceSource _wod = (WOD_ResourceSource) _resourceSource.worldObjectData;
            this.stoppingDistance = _wod.harvestDistance;
            return new Sequence(new List<Node> {
                MoveSequence(),
                TurnNode(),
                WorkResourceSourceAnimNode(),
                WorkResourceSourceNode()
            });
        }
    }

}