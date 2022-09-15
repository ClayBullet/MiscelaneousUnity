
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


    /// <summary>
    /// Detectar cuando estoy sobre un area concreta
    /// </summary>
    public class RadioDetection : MonoBehaviour
    {
        private RadialInterface _interfaceRadial;

        [SerializeField] private float maxDistanceToDetection;

        /// <summary>
        /// Una alternativa preferible a hacer el cálculo diametral de la circunferencia.
        /// </summary>
        [SerializeField] private Transform centralPoint, centralPoint2;

        private RadialBtn _lastRadialBTN;




        private void Awake()
        {
            _interfaceRadial = GetComponentInParent<RadialInterface>();

        }


       

        private void Update()
        {
            if (!_interfaceRadial.passDelayForActiveBtnsInsideRadialMenuBool) return;

            Vector2 mouseScreenPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
            Vector2 centralPos = Camera.main.ScreenToViewportPoint(centralPoint.position);
            Vector2 centralPos2 = Camera.main.ScreenToViewportPoint(centralPoint2.position);

            float distance = 10000;
            RadialBtn _currentObj = null;

            CalculateLimitSphereDirection(mouseScreenPos, centralPos);

            float maxDistanceInsideCirlce = Vector2.Distance(centralPos, centralPos2);

            float currentDistanceBetweenCenterScreen = Vector2.Distance(centralPos, mouseScreenPos);



            for (int i = 0; i < _interfaceRadial.itemsRadial.Count; i++)
            {
                Vector2 screenPos = Camera.main.ScreenToViewportPoint(_interfaceRadial.itemsRadial[i].transform.position);

                if(Vector2.Distance(mouseScreenPos, screenPos) < distance &&
                    !GameManager.instance.raycastingGraphics.detectAConcretObject(_interfaceRadial.descriptionObject)
                    && maxDistanceInsideCirlce > currentDistanceBetweenCenterScreen)
                {
                    distance = Vector2.Distance(mouseScreenPos, screenPos);
                    _currentObj = _interfaceRadial.itemsRadial[i];
                }
            }

            if(_currentObj != null)
            {
                _currentObj.EnterAction();
                _currentObj.SelectCurrentOne();

               

            }
            if (_currentObj != _lastRadialBTN)
            {
                if(_lastRadialBTN != null)
                    _lastRadialBTN.DiSelectThis();

                _lastRadialBTN = _currentObj;

            }

            _interfaceRadial.isActiveForAvoidingRadiusOverlappingBool = maxDistanceInsideCirlce > currentDistanceBetweenCenterScreen;

           

        }
        /// <summary>
        /// Presionamos sobre el botón radial actual
        /// </summary>
        public void PressRadialBtn()
        {
            if (!_interfaceRadial.isActivateRadioObj) return;    

                if (_interfaceRadial.isActiveForAvoidingRadiusOverlappingBool && _lastRadialBTN != null)
                    _lastRadialBTN.ExecuteRadialEvent();
                else
                {
                    _interfaceRadial.RemoveRadialMenu();
                }
            
        }

        public void NullableLastRadialBtn() 
        {
            _lastRadialBTN = null; 

        }

        private void CalculateLimitSphereDirection(Vector2 mouse, Vector2 objPos)
        {
          
            float right = objPos.x - (mouse.x);

            float up = objPos.y - (mouse.y);

            float angle =  Mathf.Atan2(up, right) * Mathf.Rad2Deg;

            

            centralPoint.eulerAngles = new Vector3(0, 0, angle);

        }


        public float ClampAngle( float angle)
        {
            if (angle < 0f)
                return angle + (360f * (int)((angle / 360f) + 1));
            else if (angle > 360f)
                return angle - (360f * (int)(angle / 360f));
            else
                return angle;
        }


    }


