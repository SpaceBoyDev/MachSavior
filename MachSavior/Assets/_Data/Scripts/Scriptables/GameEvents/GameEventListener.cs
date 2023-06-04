using UnityEngine;
using UnityEngine.Events;

public class GameEventListener : MonoBehaviour {
    [SerializeField]
    private GameEvent Event;
    public UnityEvent Response;


    //Le decimos al evento que hemos empezado a escuchar
    private void OnEnable() {
        Event.RegisterListener(this);
    }

    //Nos quitamos de escuchar el evento al desactivarnos
    private void OnDisable() {
        Event.UnregisterListener(this);
    }

    //Este método es llamado por el evento cuando se hace un Raise
    public void OnEventRaised() {
        Response.Invoke();
    }
}
