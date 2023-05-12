namespace Tools
{
	public interface IPoolReturn<in TItem>
	{
		public void Return(TItem item);
	}
}