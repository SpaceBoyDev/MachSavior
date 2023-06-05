using UnityEngine;
public interface ITimeInteractable
{
    bool GetIsStopped();
    //bool GetIsSelected();
    //void SetIsSelected(bool selected);
    void OnHoverEnter();
    void OnHoverExit();
    void ChangeTimeState();
}