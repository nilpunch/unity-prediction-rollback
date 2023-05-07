namespace UPR.Samples
{
    public struct CharacterInventoryCommand
    {
        public CharacterInventoryCommand(int activeSlot)
        {
            ActiveSlot = activeSlot;
        }

        public int ActiveSlot { get; }
    }
}