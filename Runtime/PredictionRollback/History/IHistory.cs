﻿namespace UPR
{
    /// <summary>
    /// Stepper for snapshot-based rollbacking.
    /// </summary>
    public interface IHistory
    {
        void SaveStep();
    }
}