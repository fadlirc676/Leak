using UnityEngine;

public class PickLighter : MonoBehaviour, IInteractible
{
    public LevelMove_Ref levelMove;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            player.Interactible = this;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && other.TryGetComponent(out PlayerMovement player))
        {
            if (player.Interactible is LevelMove_Ref && levelMove == this)
            {
                player.Interactible = null;
            }
        }
    }

    public void Interact(PlayerMovement player)
    {
        // Call the method in LevelMove_Ref to perform the level move
        levelMove.MoveToNextLevel();
    }
}
