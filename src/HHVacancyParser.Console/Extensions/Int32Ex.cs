namespace HHVacancyParser.Console.Extensions
{
    public static class Int32Ex
    {
        public static int[] GenerateSequence(this int count)
        {
            var sequence = new int[count];

            for (var i = 1; i <= count; i++)
                sequence[i - 1] = i;

            return sequence;
        }
    }
}
