using UnityEngine;

public class PaperPool : GenericPool<PaperController>
{
    [SerializeField]
    private GameSettingsData gameSettingsData;

    [SerializeField]
    private Transform paperSpawnPoint;

    public override PaperController Get()
    {
        PaperController newPaper = base.Get();
        newPaper.TrailRenderer.emitting = false;
        newPaper.TrailRenderer.Clear();
        newPaper.transform.position = paperSpawnPoint.position;
        newPaper.transform.rotation = Quaternion.identity;
        newPaper.Body.velocity = Vector3.zero;
        newPaper.Body.angularVelocity = Vector3.zero;
        newPaper.Body.useGravity = false;
        newPaper.Body.drag = 0.0f;
        newPaper.Body.isKinematic = true;
        newPaper.RegisterCollisions = true;
        newPaper.Scored = false;
        newPaper.SetVisual(gameSettingsData.PaperVisuals[gameSettingsData.SelectedPaperVisual]);

        return newPaper;
    }
}
