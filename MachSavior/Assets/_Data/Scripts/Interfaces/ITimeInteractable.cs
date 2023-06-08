using UnityEngine;
public interface ITimeInteractable
{
    bool GetIsStopped();
    //bool GetIsSelected();
    //void SetIsSelected(bool selected);
    void OnHoverEnter();
    void OnHoverExit();
    bool GetHasTimeCell();
    void UseTimeCell();
    void TakeTimeCell();
}