using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;


    /// <summary>
    /// Interfaz radial
    /// </summary>
    public class RadialInterface : MonoBehaviour
    {
        public List<RadialBtn> itemsRadial = new List<RadialBtn>();
        [SerializeField]private List<GameObject> _itemsParentsList = new List<GameObject>();
        [SerializeField] private Image myImage;
        int counter;
        [SerializeField] private Transform containerParent;
        [SerializeField] private GameObject fatherPanelObj;
        public GameObject descriptionObject;
        [SerializeField] private GameObject radialPrefab;


        public Transform currentObjToFocus;

        [SerializeField] private TextMeshProUGUI textMesh;

        private RadialActions _actionRadial;

        /// <summary>
        /// Necesario para cuando queremos recular una acción del menú radial.
        /// </summary>
        private RadialActions _previousRadialAction;
        /// <summary>
        /// Puede que nos interese tener la referencia previa, si tenemos que recuperarla a 
        /// la hora de recular o similar.
        /// </summary>
        private Transform _previousObjToFocus;

        /// <summary>
        /// Vemos si ya hemos hecho uso de alguna funcionalidad del menu radial.
        /// Cada vez que pulsemos sobre un BFGRadialAction este valor se reseteará a true
        /// </summary>
        [HideInInspector]public bool isSelectedNewRadialBtnBool;

        /// <summary>
        /// La composición de los botones en el menu radial estará en constante cambio a medida que se añadan
        /// opciones o se quiten. Es necesario hacer un offset constante sobre ellas para mantener la cohesión.
        /// </summary>
        private Vector2 _currentOffsetBetweenRadialBtns;

        /// <summary>
        /// Forma de evitar que si estamos usando un menu radial, cambiemos a otro por accidente cuando el ratón está
        /// dentro del area del menu radial. 
        /// </summary>
        public bool isActiveForAvoidingRadiusOverlappingBool;

        /// <summary>
        /// Este botón nos permite volver hacia atrás en el menu radial
        /// </summary>
        [SerializeField] private GameObject avoidBTN;


        /// <summary>
        /// No nos interesa que los botones de
        /// </summary>
        [HideInInspector] public bool passDelayForActiveBtnsInsideRadialMenuBool;

        /// <summary>
        /// No queremos enviar infinitos invoke, por lo que con este booleano deberíamos de poder capar su capacidad.
        /// </summary>
        private bool _usedInvokeMethodBool;


       [HideInInspector] public RadioDetection detectionRadio;


        public bool isActivateRadioObj { get { return fatherPanelObj.activeSelf; } }
        private void Awake()
        {
        GameManager.instance.interfaceRadial = this;
            detectionRadio = GetComponentInChildren<RadioDetection>();
        }

        private void Start()
        {
            counter = 0;

            InitializeRadialMenu();

            TickList();
        }


        private void Update()
        {
            
            if(currentObjToFocus != null )
            {
                fatherPanelObj.SetActive(true);

                this.transform.position = Camera.main.WorldToScreenPoint(currentObjToFocus.position);



                if (_actionRadial != currentObjToFocus.GetComponent<RadialActions>() ) 
                {
                    _actionRadial = currentObjToFocus.GetComponent<RadialActions>();
                    _previousRadialAction = _actionRadial;
                    _previousObjToFocus = currentObjToFocus;
                    ChargeNewRadialAction(_actionRadial);
                }
               
            }

            ChangeRadiusObjectStatus(currentObjToFocus != null && isSelectedNewRadialBtnBool);


            if (avoidBTN.activeSelf && _previousObjToFocus != null)
            {
                avoidBTN.transform.position = Camera.main.WorldToScreenPoint(_previousObjToFocus.position);
            }

        }

      
        /// <summary>
        /// Volvemos al menú radial previo
        /// </summary>
        public void BackUpToThePreviousRadius()
        {
            _previousRadialAction.recoverRadialAction.Invoke();
            currentObjToFocus = _previousObjToFocus;
            isSelectedNewRadialBtnBool = true;
            ChangeRadiusObjectStatus(true);
        }

        public void ChangeRadiusObjectStatus(bool isActive)
        {
            if (isActive)
            {
                avoidBTN.SetActive(false);
                if (!passDelayForActiveBtnsInsideRadialMenuBool && !_usedInvokeMethodBool)
                {
                    Invoke("DelayBeforeMiddleBtnWorks", .75f);
                    _usedInvokeMethodBool = true;

                }
            }
            else
            {
                descriptionObject.GetComponent<Button>().interactable = false;
                passDelayForActiveBtnsInsideRadialMenuBool = false;
                _usedInvokeMethodBool = false;
            }

            fatherPanelObj.SetActive(isActive);
            descriptionObject.gameObject.SetActive(isActive);
        }

        /// <summary>
        /// Si pulsamos muy rápido cuando popea, el botón de en medio entiende que queremos cerrar el menu.
        /// </summary>
        private void DelayBeforeMiddleBtnWorks()
        {
            descriptionObject.GetComponent<Button>().interactable = true;
            passDelayForActiveBtnsInsideRadialMenuBool = true;
        }
        public void AddRadialItem()
        {
            if (counter < itemsRadial.Count)
            {
                myImage.fillAmount += 0.1f;
                itemsRadial[counter].gameObject.SetActive(true);
                itemsRadial[counter].GetComponent<RectTransform>().sizeDelta = new Vector2(.25f, .25f);
                itemsRadial[counter].transform.SetParent(containerParent);
                counter++;
            }
        }

        public void EraseRadialItem()
        {
            if (counter > 0)
            {
                myImage.fillAmount -= 0.1f;
                counter--;
                itemsRadial[counter].gameObject.SetActive(false);
            }
        }
        /// <summary>
        /// Cuando pulsamos un nuevo objeto que posee un menu radial, cargamos los nuevos datos, tras deshacernos de los anteriores
        /// </summary>
        /// <param name="radialAction"></param>
        public void ChargeNewRadialAction(RadialActions radialAction)
        {
            if (radialAction.unaccesibleBool) return;

            isSelectedNewRadialBtnBool = true;

            textMesh.text = radialAction.descObj;

            _currentOffsetBetweenRadialBtns = radialAction.offsetCohesion;

            counter = radialAction.slotsRadials[radialAction.currentIndexSlot].classRadials.Length;
            RemovePreviousInfo();
            InsertNewData(radialAction.slotsRadials[radialAction.currentIndexSlot].classRadials);
            TickList();
        }
        /// <summary>
        /// Eliminar toda la info que estuviese previamente.
        /// </summary>
        private void RemovePreviousInfo()
        {
            Debug.Log("accedo a remove previous info");
            for (int i = 0; i < _itemsParentsList.Count; i++)
            {
                DoInvissibleTheBtn(_itemsParentsList[i].gameObject);
            }

            for (int i = 0; i < itemsRadial.Count; i++)
            {
                itemsRadial[i].InvisibleSurface();
                itemsRadial[i].transform.SetParent(fatherPanelObj.transform);
            }

            _itemsParentsList.Clear();
            InitializeRadialMenu();
        }

        public void InitializeRadialMenu()
        {
            RadialBtn[] objectsInsideRadialMenu = itemsRadial.ToArray();

            for (int i = 0; i < counter; i++)
            {
                try
                {
                    if (!objectsInsideRadialMenu[i].gameObject.activeSelf)
                    {
                        GameObject go = objectsInsideRadialMenu[i].gameObject;

                        itemsRadial.Remove(go.GetComponent<RadialBtn>());
                        Destroy(go);


                    }
                }
                catch
                {
                    Debug.Log(" index " + i);
                }
            }
        }



        public void AddedNewRadialObject(GameObject objAssigned)
        {
            InstantiateNewRadialObject(objAssigned, itemsRadial.Count);
        }

        public void SetStateFatherPanel(bool isActive)
        {
            fatherPanelObj.SetActive(isActive);
        }

        public void InsertNewData(RadialClass[] radials)
        {
            for (int i = 0; i < radials.Length; i++)
            {
                itemsRadial[i].VisibleSurface();
                itemsRadial[i].InsertRadialClassData(radials[i]);
            }
        }

        public void TickList()
        {
            for (int i = 0; i < counter; i++)
            {
                InstantiateNewRadialObject(itemsRadial[i].gameObject, i);
            }

          //  ReassignParents();
        }

        public void InstantiateNewRadialObject(GameObject assignedObj, float numberRadials)
        {


            numberRadials += 1;

            GameObject go = Instantiate(radialPrefab, containerParent);


            _itemsParentsList.Add(go);

            go.name = "items " + (_itemsParentsList.Count - 1);
            float currentValue = 360 / numberRadials;

            go.GetComponent<RectTransform>().position = Vector3.zero;
            go.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
            go.GetComponent<RectTransform>().rotation = Quaternion.identity;


            for (int i = 0; i < _itemsParentsList.Count; i++)
            {
                _itemsParentsList[i].transform.localRotation = Quaternion.Euler(new Vector3(0, 0, currentValue * i));
                _itemsParentsList[i].transform.localScale = new Vector3(1.5f, 1.5f, 1.5f);
                _itemsParentsList[i].transform.localPosition = Vector3.zero;
                float calculate = 1 / numberRadials;

                _itemsParentsList[i].GetComponent<Image>().fillAmount = calculate;

                assignedObj.name = "ASSIGNED " + i;

                DoVissibleTheBtn(assignedObj);
            }


            assignedObj.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 0));


           assignedObj.transform.SetParent(go.transform);


            assignedObj.transform.localPosition = new Vector3(_currentOffsetBetweenRadialBtns.x, _currentOffsetBetweenRadialBtns.y, 0);


            assignedObj.transform.localScale = new Vector3(35, 35, 0);
            assignedObj.GetComponent<RectTransform>().sizeDelta = new Vector2(1, 1);
           // go.GetComponent<RectTransform>().rotation = Quaternion.identity;

        }


        public void ReassignParents()
        {
            for (int i = 0; i < _itemsParentsList.Count; i++)
            {
                itemsRadial[i].transform.SetParent(_itemsParentsList[(_itemsParentsList.Count -1) - i].transform);
            }
        }
        /// <summary>
        /// Hacer visible el botón del radial
        /// </summary>
        public void DoVissibleTheBtn(GameObject go)
        {
            go.GetComponent<Image>().color = Color.red;
        }

        /// <summary>
        /// Hacer invisible el botón del radial.
        /// </summary>
        /// <param name="go"></param>
        public void DoInvissibleTheBtn(GameObject go)
        {
            go.GetComponent<Image>().color = new Color(0,0,0,0);
        }

        /// <summary>
        /// Activamos el botón de cancelar, porque entendemos que la acción puede ser cancelable.
        /// </summary>
        public void CancellableAction(bool isActive)
        {
            avoidBTN.SetActive(isActive);
        }

        public void RemoveRadialMenu()
        {
            currentObjToFocus = null;
          isSelectedNewRadialBtnBool = false;
            NullableActionRadial();
        }

        public float ClampAngle(float angle, float from, float to)
        {
            // accepts e.g. -80, 80
            if (angle < 0f) angle = 360 + angle;
            if (angle > 180f) return Mathf.Max(angle, 360 + from);
            return Mathf.Min(angle, to);
        }

        /// <summary>
        /// Volvemos RadialAction null para que refresque la info del Radial.
        /// </summary>
        public void NullableActionRadial()
        {
            _actionRadial = null;
        }

        /// <summary>
        /// Si desde el InputManager queremos acceder a otro menu radial, tenemos que asegurarnos 
        /// que no se haga cuando estemos usando el menu radial actual. 
        /// Basicamente, evitar que halla problemas de overlapping entre los menus
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool identifyLikeRadialActionAndValorateIfIsAvailableToUse(GameObject obj)
        {
            if (obj.GetComponent<RadialActions>() != null && fatherPanelObj.activeSelf)
                return isActiveForAvoidingRadiusOverlappingBool;

                return false;
        }


       
    }

    /// <summary>
    /// De alguna forma tenemos que gestionar las acciones enviadas al menu radial
    /// y creo que una clase que podemos extender con facilidad es la mejor opción.
    /// </summary>
    [System.Serializable]
    public class RadialClass
    {

        [TextArea] public string radialDescription;
        public UnityEvent eventRadial;
        /// <summary>
        /// Marcamos si la acción que vamos a ejecutar no debería de desactivar el menu radial, porque es una acción de
        /// transicionar a un nuevo set de acciones dentro del menu radial.
        /// </summary>
        public bool isATransitionActionBool;

        [Header("INFO LAYER")]
        [Space]
        public UnitData dataUnit;




        public RadialClass(string _desc, UnityEvent _eventRadial)
        {
            radialDescription = _desc;
            _eventRadial = eventRadial;
        }
    }


