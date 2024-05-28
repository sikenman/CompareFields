using System.Reflection;

namespace CompareFileds
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("*****************");
            Console.WriteLine(" Compare Objects ");
            Console.WriteLine("*****************");

            var student1 = new Student { FirstName = "Alice", LastName = "Dongol", RollNo = 101, Standard = "10th", Division = "A", Fee = 12500.550 };
            var student1b = new Student { FirstName = "Alice", LastName = "Dongol", RollNo = 101, Standard = "10th", Division = "A", Fee = 12500.550 };
            var student2 = new Student { FirstName = "Alice", LastName = "Dongol", RollNo = 102, Standard = "10th", Division = "B", Fee = 13500.450 };
            var student3 = new Student { FirstName = "Ganesh", LastName = "Thapa", RollNo = 103, Standard = "10th", Division = "A", Fee = 14500.450 };

            bool areEqual = student1.Equals(student1b);
            Console.WriteLine("Comparing Student1 with Student1b");
            Console.WriteLine($"Are the students equal? {areEqual}");
            Console.WriteLine();
            areEqual = student1.Equals(student2);
            Console.WriteLine("Comparing Student1 with Student2");
            Console.WriteLine($"Are the students equal? {areEqual}");

            Console.WriteLine("-------------------------------");
            var CheckObjects = GenerateFieldDifference(student1, student2);

            foreach (var differentField in CheckObjects)
            {
                Console.WriteLine(differentField.GetBeforeAfter());
            }

            // Compare students using the CompareTo method.
            //int comparisonResult = student1.CompareTo(student1b);
            //Console.WriteLine(comparisonResult);


            //if (comparisonResult < 0)
            //    Console.WriteLine($"{student1.Name} comes before {student2.Name}");
            //else if (comparisonResult > 0)
            //    Console.WriteLine($"{student2.Name} comes before {student1.Name}");
            //else
            //    Console.WriteLine($"{student1.Name} and {student2.Name} have the same RollNo");

            // Output:
            // Alice comes before Bob

            var students = new List<Student>
            {
                new() { FirstName = "Alice  ", LastName="Dongol", RollNo = 101, Standard = "10th", Division = "A", Fee = 12500.550 },
                new() { FirstName = "Bobby  ", LastName="Bajaj ", RollNo = 102, Standard = "10th", Division = "B", Fee = 14300.450  },
                new() { FirstName = "Charlie", LastName="Khanal", RollNo = 103, Standard = "10th", Division = "A", Fee = 10300.450 }
            };

            // Sort students by Fee in ascending order
            var sortedStudentsAsc = students.OrderBy(s => s.Fee).ToList();

            // Sort students by Fee in descending order
            var sortedStudentsDesc = students.OrderByDescending(s => s.Fee).ToList();
            Console.WriteLine();
            Console.WriteLine("Sorted students (ascending):");
            foreach (var student in sortedStudentsAsc)
            {
                Console.WriteLine($"Name: {student.FirstName}, Fee: {student.Fee}");
            }

            Console.WriteLine("\nSorted students (descending):");
            foreach (var student in sortedStudentsDesc)
            {
                Console.WriteLine($"Name: {student.FirstName}, Fee: {student.Fee}");
            }
        }



        public static IList<DifferentField> GenerateFieldDifference<T>(T originalObject, T changedObject)
        {
            IList<DifferentField> list = new List<DifferentField>();
            //string className = $"[{originalObject.GetType().Name}]";

            foreach (PropertyInfo property in originalObject.GetType().GetProperties())
            {
                Type comparable = property.PropertyType.GetInterface("System.IComparable");
                if (comparable != null)
                {
                    object originalValue = property.GetValue(originalObject, null);
                    object newValue = property.GetValue(changedObject, null);

                    //string originalValue = property.GetValue(originalObject, null) as string;
                    //string newValue = property.GetValue(changedObject, null) as string;

                    Type propertyType = property.PropertyType;

                    //if (!string.Equals(originalValue, newValue))
                    if (!object.Equals(originalValue, newValue))
                    {
                        DifferentField df = new()
                        {
                            FieldName = property.Name,
                            DataType = propertyType.Name,
                            BeforeValue = originalValue.ToString(),
                            AfterValue = newValue.ToString()
                        };

                        list.Add(df);
                    }
                }
            }

            return list;
        }

    }

    class Student : IEquatable<Student>, IComparable<Student>
    {
        public int RollNo { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Standard { get; set; }
        public string Division { get; set; }
        public double Fee { get; set; } = 0;


        //// Implement the Equals method with the Student class as the type parameter.
        public bool Equals(Student other)
        {
            if (other == null)
                return false;

            return FirstName == other.FirstName &&
                   LastName == other.LastName &&
                   RollNo == other.RollNo &&
                   Standard == other.Standard &&
                   Division == other.Division &&
                   Fee == other.Fee;
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            return Equals(obj as Student);
        }

        // Implement the CompareTo method with the Student class as the type parameter.
        public int CompareTo(Student other)
        {
            // If 'other' is not a valid object reference, this instance is greater.
            if (other == null)
                return 1;

            // Compare students based on RollNo.
            return Fee.CompareTo(other.Fee);
        }

        // Override GetHashCode if you override Equals.
        public override int GetHashCode()
        {
            return HashCode.Combine(FirstName, LastName, RollNo, Standard, Division, Fee);
        }

    }

    public class DifferentField
    {
        public required string FieldName { get; set; }
        public string DataType { get; set; } = "string";
        public string? BeforeValue { get; set; } = null;
        public string? AfterValue { get; set; } = null;

        public string GetBeforeAfter()
        {
            return $"[{FieldName}: {DataType}] -> '{BeforeValue}' to '{AfterValue}'";
        }
    }
}
