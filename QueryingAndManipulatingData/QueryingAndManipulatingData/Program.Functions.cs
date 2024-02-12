using static System.Console;

partial class Program
{
    private static void DeferredExecution(string[] names)
    {
        SectionTitle("Deferred execution");

        var query1 = names.Where(name => name.EndsWith("m"));

        var query2 = from name in names where name.EndsWith("m") select name;

        string[] result1 = query1.ToArray();

        List<string> result2 = query2.ToList();

        foreach (string name in query1)
        {
            WriteLine(name);

            names[2] = "Jimmy";
        }
    }

    private static void FilteringUsingWhere(string[] names)
    {
        SectionTitle("Filtering entities using Where");

        IOrderedEnumerable<string> query = names
            .Where(name => name.Length > 4)
            .OrderBy(name => name.Length)
            .ThenBy(name => name);

        foreach (string item in query)
        {
            WriteLine(item);
        }
    }

    private static bool NameLongerThanFour(string name)
    {
        return name.Length > 4;
    }

    private static void FilteringByType()
    {
        SectionTitle("Filtering by type");

        List<Exception> exceptions =
            new()
            {
                new ArgumentException(),
                new SystemException(),
                new IndexOutOfRangeException(),
                new InvalidOperationException(),
                new NullReferenceException(),
                new InvalidCastException(),
                new OverflowException(),
                new DivideByZeroException(),
                new ApplicationException()
            };

        IEnumerable<ArithmeticException> arithmeticExceptionsQuery =
            exceptions.OfType<ArithmeticException>();

        foreach (ArithmeticException exception in arithmeticExceptionsQuery)
        {
            WriteLine(exception);
        }
    }

    private static void Output(IEnumerable<string> cohort, string description = "")
    {
        if (!string.IsNullOrEmpty(description))
        {
            WriteLine(description);
        }
        Write(" ");
        WriteLine(string.Join(", ", cohort.ToArray()));
        WriteLine();
    }

    private static void WorkingWithSets()
    {
        string[] cohort1 = { "Rachel", "Gareth", "Jonathan", "George" };

        string[] cohort2 = { "Jack", "Stephen", "Daniel", "Jack", "Jared" };

        string[] cohort3 = { "Declan", "Jack", "Jack", "Jasmine", "Conor" };

        SectionTitle("The cohorts");

        Output(cohort1, "Cohort 1");
        Output(cohort2, "Cohort 2");
        Output(cohort3, "Cohort 3");

        SectionTitle("Set operations");

        Output(cohort2.Distinct(), "cohort2.Distinct()");
        Output(
            cohort2.DistinctBy(name => name.Substring(0, 2)),
            "cohort2.DistinctBy(name => name.Substring(0, 2)):"
        );
        Output(cohort2.Union(cohort3), "cohort2.Union(cohort3)");
        Output(cohort2.Concat(cohort3), "cohort2.Concat(cohort3)");
        Output(cohort2.Intersect(cohort3), "cohort2.Intersect(cohort3)");
        Output(cohort2.Except(cohort3), "cohort2.Except(cohort3)");
        Output(cohort1.Zip(cohort2, (c1, c2) => $"{c1} matched with {c2}"), "cohort1.Zip(cohort2)");
    }
}
