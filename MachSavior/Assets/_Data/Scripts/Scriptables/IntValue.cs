using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Int Value", menuName = "MachSavior/Values/Int Value", order = 100)]
public class IntValue : ScriptableObject, ISerializationCallbackReceiver {

    //Este scriptable object serializa un int, para poder referenciarlo en diferentes componentes sin que estos tengan referencia entre si.
    public int intValue;

    //Se accede a runtimeValue para evitar modificar el valor serializado en el editor y conservar su valor
    [NonSerialized]
    public int runtimeValue;

    public void OnAfterDeserialize() {
        //Reseteamos el runtime value al valor serializado
        runtimeValue = intValue;
    }

    public void OnBeforeSerialize() {
    }
}
