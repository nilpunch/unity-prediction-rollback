﻿using System;
using System.Collections.Generic;

namespace UPR.PredictionRollback
{
    public class Rollbacks : IRollback
    {
        private readonly List<IRollback> _rollback = new List<IRollback>();

        public void Add(IRollback simulation)
        {
            if (simulation == null)
                throw new ArgumentNullException(nameof(simulation));

            _rollback.Add(simulation);
        }

        public void Rollback(int steps)
        {
            foreach (var rollback in _rollback)
            {
                rollback.Rollback(steps);
            }
        }
    }
}
