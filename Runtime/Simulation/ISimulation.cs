namespace UPR.PredictionRollback
{
	public interface IEntity<TState>
	{
		void Update(int currentTick, ref TState state);
	}

	public interface ISimulation
	{
		void Rollback(int ticks);

		void PlayCommands(int currentTick);
		
		void Update(int currentTick);
        
		void SaveChanges();
	}
}