using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Game Event", menuName = "MachSavior/GameEvent", order = 0)]
public class GameEvent : ScriptableObject {

    //Lista de todos los objetos que están "escuchando" al evento en cuestion
    private List<GameEventListener> listeners = new List<GameEventListener>();

    //Cuando alguíen llama a Raise del evento, iteramos sobre todos los listeners para que se enteren de que el evento ha sido lanzado
    public void Raise() {
        for (int i = 0; i < listeners.Count; i++) {
            listeners[i].OnEventRaised();
        }
    }

    //Metodos para añadir y quitar listeners del evento
    public void RegisterListener(GameEventListener listener) {
        listeners.Add(listener);
    }

    public void UnregisterListener(GameEventListener listener) {
        listeners.Remove(listener);
    }
}
