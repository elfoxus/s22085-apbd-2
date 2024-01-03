namespace apbd_2_s22085.Model;

public record Student(
    string indexNumber,
    string fname,
    string lname,
    string birthdate,
    string email,
    string mothersName,
    string fathersName,
    Studies studies);