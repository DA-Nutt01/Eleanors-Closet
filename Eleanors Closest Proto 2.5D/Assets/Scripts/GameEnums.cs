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
    }

    public enum InteractableType
    {
        Environment,
        Door,
    }

    public enum InputContext
    {
        Gameplay,
        UIInteraction,
    }

    public enum ItemType
    {
        Key,
        Consumeable
    }
}
