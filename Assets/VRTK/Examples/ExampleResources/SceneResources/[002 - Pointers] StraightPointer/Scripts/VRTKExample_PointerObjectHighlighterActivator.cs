﻿namespace VRTK.Examples
{
    using UnityEngine;
    using VRTK.Highlighters;
    using UnityEngine.UI;

    public class VRTKExample_PointerObjectHighlighterActivator : MonoBehaviour
    {
        public VRTK_DestinationMarker pointer;
        public Color hoverColor = Color.cyan;
        public Color selectColor = Color.yellow;
        public bool logEnterEvent = true;
        public bool logHoverEvent = false;
        public bool logExitEvent = true;
        public bool logSetEvent = true;
        GameObject txtObject;
        public Canvas sphereCanvas;
        public Text IText;
        public Text EText;
        public Text LText;
        public Text AText;

        public Canvas cubeCanvas;
        public Text TText;
        public Text RText;


        protected virtual void OnEnable()
        {
            pointer = (pointer == null ? GetComponent<VRTK_DestinationMarker>() : pointer);

            if (pointer != null)
            {
                pointer.DestinationMarkerEnter += DestinationMarkerEnter;
                pointer.DestinationMarkerHover += DestinationMarkerHover;
                pointer.DestinationMarkerExit += DestinationMarkerExit;
                pointer.DestinationMarkerSet += DestinationMarkerSet;
            }
            else
            {
                VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTKExample_PointerObjectHighlighterActivator", "VRTK_DestinationMarker", "the Controller Alias"));
            }
        }

        protected virtual void OnDisable()
        {
            if (pointer != null)
            {
                pointer.DestinationMarkerEnter -= DestinationMarkerEnter;
                pointer.DestinationMarkerHover -= DestinationMarkerHover;
                pointer.DestinationMarkerExit -= DestinationMarkerExit;
                pointer.DestinationMarkerSet -= DestinationMarkerSet;
            }
        }

        protected virtual void DestinationMarkerEnter(object sender, DestinationMarkerEventArgs e)
        {
            ToggleHighlight(e.target, hoverColor);
            if (logEnterEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER ENTER", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }

            if (e.target.name=="Sphere")
            {
                sphereCanvas.enabled=true;
              
                for (int i=0; i<sensors.spheres.Count; i++)
                {
                    if (sensors.spheres[i].transform.position==e.target.transform.position)
                    {
                        IText.text = sensors.Sensors[i].id.ToString();
                        AText.text = sensors.Sensors[i].action.ToString();
                        EText.text = sensors.Sensors[i].energy.ToString();
                        LText.text = sensors.Sensors[i].load.ToString();
                    }
                }
            }

            if (e.target.name == "Cube")
            {
                cubeCanvas.enabled = true;

                for (int i = 0; i < sensors.Packets.Count; i++)
                {
                    if (sensors.Packets[i].item.transform.position == e.target.transform.position)
                    {
                        TText.text = sensors.Packets[i].id.ToString();
                        RText.text = sensors.Packets[i].dest_id.ToString();
                    }
                }
            }

        }

        private void DestinationMarkerHover(object sender, DestinationMarkerEventArgs e)
        {
            if (logHoverEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER HOVER", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }
        }

        protected virtual void DestinationMarkerExit(object sender, DestinationMarkerEventArgs e)
        {
            ToggleHighlight(e.target, Color.clear);
            if (logExitEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER EXIT", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }
            if (e.target.name == "Sphere")
            {
                sphereCanvas.enabled = false;
            }
            if (e.target.name == "Cube")
            {
                cubeCanvas.enabled = false;
            }
        }

        protected virtual void DestinationMarkerSet(object sender, DestinationMarkerEventArgs e)
        {
            ToggleHighlight(e.target, selectColor);
            if (logSetEvent)
            {
                DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "POINTER SET", e.target, e.raycastHit, e.distance, e.destinationPosition);
            }
        }

        protected virtual void ToggleHighlight(Transform target, Color color)
        {
            VRTK_BaseHighlighter highligher = (target != null ? target.GetComponentInChildren<VRTK_BaseHighlighter>() : null);
            if (highligher != null)
            {
                highligher.Initialise();
                if (color != Color.clear)
                {
                    highligher.Highlight(color);
                }
                else
                {
                    highligher.Unhighlight();
                }
            }
        }

        protected virtual void DebugLogger(uint index, string action, Transform target, RaycastHit raycastHit, float distance, Vector3 tipPosition)
        {
            string targetName = (target ? target.name : "<NO VALID TARGET>");
            string colliderName = (raycastHit.collider ? raycastHit.collider.name : "<NO VALID COLLIDER>");
            VRTK_Logger.Info("Controller on index '" + index + "' is " + action + " at a distance of " + distance + " on object named [" + targetName + "] on the collider named [" + colliderName + "] - the pointer tip position is/was: " + tipPosition);
        }
    }
}