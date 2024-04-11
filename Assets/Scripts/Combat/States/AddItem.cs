using System.Collections;
using UnityEngine;

internal class AddItem : State
{
    public AddItem(BattleSystem battleSystem) : base(battleSystem)
    {
    }

    public override IEnumerator Start()
    {
        BattleSystem.combatUIManager.ToggleObjectGenerationPanel(true);
        // ObjectDetection.instance.InitiateWebcam();
        ObjectDetection.instance.picture_taken = false;
        if (ObjectDetection.instance.webcamTexture != null)
        {
            ObjectDetection.instance.webcamTexture.Play();
        }
        ObjectDetection.instance.is_object_generation_done = false;
        yield return new WaitUntil(() => ObjectDetection.instance.is_object_generation_done);
        ObjectDatabase.Instance.AddItem(ObjectDetection.instance.GetGeneratedObjectItem());
        BattleSystem.combatUIManager.ToggleObjectGenerationPanel(false);

        if (BattleSystem.Instance.level == 1)
        {
            BattleSystem.SetState(new FirstTurn(BattleSystem));
        }
        else
        {
            BattleSystem.SetState(new NewCombatRound(BattleSystem));
        }
    }
}


