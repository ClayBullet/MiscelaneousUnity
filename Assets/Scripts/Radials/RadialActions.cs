using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


    /// <summary>
    /// Las acciones del radial disponibles para este objeto
    /// </summary>
    public class RadialActions : MonoBehaviour
    {

        /// <summary>
        /// Offset de este Radial Action para que el menu radial no pierda la cohesión entre sus botones
        /// </summary>
        public Vector2 offsetCohesion;

        [TextArea] public string descObj;

        /// <summary>
        /// Indice actual del slot en el que nos ubicamos
        /// </summary>
        public int currentIndexSlot;

        /// <summary>
        /// Guardándolos en slots individuales, resultará más fácil lidiar con aquellos que cuenten con muchas opciones
        /// </summary>
        public RadialSlots[] slotsRadials;

        /// <summary>
        /// Puede que me interese tener algunas funcionalidades desactivadas hasta
        /// poder implementarlas de forma más precisa
        /// </summary>
        public bool unaccesibleBool;

        /// <summary>
        /// Cuando haces la acción de retroceso, desactiva lo que tuvieses para hacer la acción que anulaste
        /// </summary>
        public UnityEvent recoverRadialAction;

        /// <summary>
        /// Cambiamos el 
        /// </summary>
        /// <param name="indexSlot"></param>
        public void ChangeIndexSlot(int indexSlot)
        {
            currentIndexSlot = indexSlot;
            GameManager.instance.interfaceRadial.ChargeNewRadialAction(this);

          //  GameManager.instance.mainCanvasManager.interfaceRadial.NullableActionRadial();
        }

        /// <summary>
        /// Activamos el botón de cancelación por si queremos volver hacia atrás.
        /// </summary>
        public void AvailableCancelableBtn()
        {
          

            GameManager.instance.interfaceRadial.CancellableAction(true);
        }
    }

    /// <summary>
    /// Pasará a veces que un mismo objeto tenga tantas opciones en su menu radial, que no resulte
    /// rentable tenerlas todas a la vista. Por eso podemos fragmentarlos en secciones y construir enlaces entre ellos
    /// para que resulte más fácil de portar
    /// </summary>
    [System.Serializable]
    public class RadialSlots
    {
        /// <summary>
        /// Le ponemos un nombre para que resulte más fácil de identificar entre sí.
        /// </summary>
        public string slotName;
        /// <summary>
        /// Aquí aglutinamos todas las acciones que tiene este InteractableObject
        /// </summary>
        public RadialClass[] classRadials;
    }


