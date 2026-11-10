namespace Speurzoekers.Business
{
    public class Counter
    {
        public int Current { get; set; }
        public int Max { get; set; }

        public Counter(int max)
        {
            Current = 0;
            Max = max;
        }
    }
}
