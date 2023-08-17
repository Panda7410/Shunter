using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
//using UnityEngine.EventSystems;

namespace GsDefaultModule
{
    public class InputManager
    {
        public Action KeyAction = null;
        public Action UpdateAction = null;
        public Action<RaycastHit> RayAction = null;
        public Action<RaycastHit> RayActionUp = null;
        public Action<RaycastHit> RayActionDown = null;

        public Action<RaycastHit[]> RayActionAll = null;
        public Action<RaycastHit[]> RayActionAllUp = null;
        public Action<RaycastHit[]> RayActionAllDown = null;
        public Action<RaycastHit> RayActionOrthoGrapic = null;

        public Action<RaycastHit[]> RayActionRightClickAll = null;
        public Action<RaycastHit[]> RayActionRightClickAllUp = null;
        public Action<RaycastHit[]> RayActionRightClickAllDown = null;

        public void OnUpdate()
        {

            //if (Input.anyKey && KeyAction != null)
            //    KeyAction.Invoke();

            UpdateAction?.Invoke();

            //ActionOnRay();
        }

        void ActionOnRay()
        {
            if ((Input.GetMouseButtonDown(0) && RayActionAllDown != null) || (Input.GetMouseButtonDown(0) && RayActionDown != null)) // 마우스 Click 순간 발동.
            {

                //if (EventSystem.current.IsPointerOverGameObject() == false)
                //{
                //}
                if (RayActionAllDown != null)
                    RayActionAllDown.Invoke(RayScreenAll());
                if (RayActionDown != null)
                    RayActionDown.Invoke(RayScreen());
            }


            if ((Input.GetMouseButton(0) && RayAction != null) || (Input.GetMouseButton(0) && RayActionAll != null)) // 마우스 클릭동안 발동. (주의- 첫타이밍은 클릭순간 도 같은타이밍으로 처리됨.)
            {
                //if (EventSystem.current.IsPointerOverGameObject() == false)
                //{
                //}
                if (RayActionAll != null)
                    RayActionAll.Invoke(RayScreenAll());
                if (RayAction != null)
                    RayAction.Invoke(RayScreen());
            }


            if ((Input.GetMouseButtonUp(0) && RayActionUp != null) || Input.GetMouseButtonUp(0) && RayActionAllUp != null) // 마우스떼는 순간 발동.
            {
                //if (EventSystem.current.IsPointerOverGameObject() == false)
                //{
                //}
                if (RayActionAllUp != null)
                    RayActionAllUp.Invoke(RayScreenAll());
                if (RayActionUp != null)
                    RayActionUp.Invoke(RayScreen());
            }



            if (Input.GetMouseButtonDown(0) && RayActionOrthoGrapic != null) // 2D발동
            {

                //if (EventSystem.current.IsPointerOverGameObject() == false)
                //{
                //}
                if (RayActionOrthoGrapic != null)
                    RayActionOrthoGrapic.Invoke(RayScreenOrthoGrapic());
            }



            if (Input.GetMouseButtonDown(1) && RayActionRightClickAllDown != null)
            {
                RayActionRightClickAllDown.Invoke(RayScreenAll());
            }
            if (Input.GetMouseButton(1) && RayActionRightClickAll != null)
            {
                RayActionRightClickAll.Invoke(RayScreenAll());
            }
            if (Input.GetMouseButtonUp(1) && RayActionRightClickAllUp != null)
            {
                RayActionRightClickAllUp.Invoke(RayScreenAll());
            }

        }


        RaycastHit RayScreen()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Vector3 dir = mousePos - Camera.main.transform.position;
            dir = dir.normalized;

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position, dir, out hit, 1000.0f))
            {
                //hit 처리
                //Debug.Log(hit.collider.name);
            }

            return hit;
        }

        RaycastHit RayScreenOrthoGrapic()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3 dir = mousePos - Camera.main.transform.position;
            dir = dir.normalized;
            Debug.DrawRay(mousePos, Vector3.forward, Color.red, 1000f);


            RaycastHit hit;
            if (Physics.Raycast(mousePos, Vector3.forward, out hit, 3000.0f))
            {
                //hit 처리
                //Debug.Log(hit.collider.name);
            }

            return hit;
        }


        RaycastHit[] RayScreenAll()
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
            Vector3 dir = mousePos - Camera.main.transform.position;
            dir = dir.normalized;

            RaycastHit[] hit = Physics.RaycastAll(Camera.main.transform.position, dir);
            return hit;
        }
    }
}
