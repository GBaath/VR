using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;


namespace UnityEngine.XR.Content.Interaction
{
    public class LeverScript : XRLever
    {


        protected override void SetValue(bool isOn, bool forceRotation = false)
        {
            if (m_Value == isOn)
            {
                if (forceRotation)
                    SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);

                return;
            }

            m_Value = isOn;

            if (m_Value)
            {
                m_OnLeverActivate.Invoke();
            }
            else
            {
                m_OnLeverDeactivate.Invoke();
            }

            if (!isSelected && (m_LockToValue || forceRotation))
                SetHandleAngle(m_Value ? m_MaxAngle : m_MinAngle);
        }
    }
}

