namespace UPR.Samples
{
    public struct CharacterInventoryMemory
    {
        public CharacterInventoryMemory(int activeSlot)
        {
            ActiveSlot = activeSlot;
        }

        public int ActiveSlot { get; set; }
    }
}
