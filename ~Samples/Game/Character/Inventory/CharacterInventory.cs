namespace UPR.Samples
{
    public class CharacterInventory : IMemory<CharacterInventoryMemory>
    {
        private CharacterInventoryMemory _memory;

        public void ChangeActiveSlot(int slot)
        {
            _memory.ActiveSlot = slot;
        }

        public CharacterInventoryMemory Save()
        {
            return _memory;
        }

        public void Load(in CharacterInventoryMemory memory)
        {
            _memory = memory;
        }
    }
}
