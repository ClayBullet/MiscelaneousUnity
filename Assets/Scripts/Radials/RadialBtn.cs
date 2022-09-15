using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;


   /// <summary>
    /// Gestión del botón radial
    /// </summary>
    public class RadialBtn : MonoBehaviour
    {
        public RadialClass currentClassRadial;

    

        [SerializeField] private Color selectedColor;
        [SerializeField] private Color unselectedColor;

        private Image _radialImage
        {
            get
            {
                if (transform.parent == null) return null;

                return transform.parent.GetComponent<Image>();
            }
        }


        private Image _currentImage;

        private Color _currentColorImg;

        private TextMeshProUGUI textMesh;

        private bool _isEnterActionHappenBool;

        private void Awake()
        {
            textMesh = GetComponentInChildren<TextMeshProUGUI>();
            _currentImage = GetComponent<Image>();
        }

        private void Start()
        {
           _currentColorImg = _currentImage.color;
        }
        /// <summary>
        /// Insertamos los datos necesarios para que el menu radial funcione correctamente.
        /// </summary>
        public void InsertRadialClassData(RadialClass classRadial)
        {
            currentClassRadial = classRadial;
            textMesh.text = classRadial.radialDescription;

            _isEnterActionHappenBool = false;
        }

        /// <summary>
        /// Acción que realizo al entrar dentro de este RadialBtn
        /// </summary>
        public void EnterAction()
        {
            if (_isEnterActionHappenBool) return;

          

        }

        /// <summary>
        /// Abandonamos esta sección del menu radial
        /// </summary>
        public void ExitAction()
        {
           // GameManager.instance.mainCanvasManager.mouseAttached.InvisibleInfo();

        }
        /// <summary>
        /// Ejecutamos la acción ligada a este RadialClass
        /// </summary>
        public void ExecuteRadialEvent()
        {
            currentClassRadial.eventRadial.Invoke();

            if (!currentClassRadial.isATransitionActionBool)
                GameManager.instance.interfaceRadial.RemoveRadialMenu();


            ExitAction();

            _isEnterActionHappenBool = true;

        }


        public void SelectCurrentOne()
        {
            _radialImage.color = selectedColor;
        }
      
        public void DiSelectThis()
        {
            _radialImage.color = unselectedColor;
            ExitAction();
            //textMesh.text = "";
        }
        
        /// <summary>
        /// Nos interesa que la superficie a veces no se pueda ver. No podemos simplemente
        /// desactivar el objeto porque eso lo saca de la lista de BFGRadialInterface, así que 
        /// tenemos que buscar otras alternativas para que no sea visible cuando cambiemos de opciones 
        /// en el mismo menu radial
        /// </summary>
        public void InvisibleSurface()
        {

            _currentImage.color = new Color(0,0,0,0);
            textMesh.text = "";
        }

        /// <summary>
        /// Queremos que aparezca en el menu radial
        /// </summary>
        public void VisibleSurface()
        {
            _currentImage.color = _currentColorImg;
            textMesh.text = "";
        }
    }


