namespace Guts
{
    public static class LuxExtensions
    {
        public static int ToInt(this object obj)
        {
            if (!int.TryParse(obj.ToString(), out int val))
            {
                throw new RuntimeException("Int has incorrect format");
            }
            return val;
        }
    }
}