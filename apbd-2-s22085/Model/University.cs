namespace apbd_2_s22085.Model;

public record University(
    string createdAt,
    string author,
    HashSet<Student> studenci,
    List<ActiveStudy> activeStudies
    );