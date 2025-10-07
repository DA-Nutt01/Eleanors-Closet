namespace EC
{
    public enum RoomType
    {
        Null,
        LivingRoom,
        Office,
        Bathroom,
        Bedroom,
        FrontYard,
        BackyYard,
    }

    public enum InteractableState
    {
        Locked,
        Unlocked,
        Interacting,
    }

    public enum InteractableType
    {
        Item,
        Environment,
        Door,
    }

    public enum InputContext
    {
        Gameplay,
        UIInteraction,
    }
}
