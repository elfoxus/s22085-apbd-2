using apbd_2_s22085.Model;

namespace apbd_2_s22085;

public class StudentEqualityComparer : IEqualityComparer<Student>
{
    public bool Equals(Student? x, Student? y)
    {
        if (x == null || y == null)
        {
            return false;
        }

        return x.indexNumber == y.indexNumber
               && x.fname == y.fname
               && x.lname == y.lname;
    }

    public int GetHashCode(Student obj)
    {
        return (obj.indexNumber, obj.fname, obj.lname).GetHashCode();
    }
}